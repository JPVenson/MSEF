#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 12:14

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;
using JPB.Shell.MEF.Log;

namespace JPB.Shell.MEF.Model
{
    internal class StrongNameCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
    {
        private readonly AggregateCatalog _aggregateCatalog = new AggregateCatalog();
        private readonly IEnumerable<string> _paths;
        private readonly byte[][] _trustedKeys;
        private readonly bool _watchDirectorys;
        private readonly List<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();

        /// <summary>
        ///     StrongNameCatalog Constructor
        ///     <para>
        ///         Use this Ctor to get all dll's that export anything in the spezific paths
        ///     </para>
        ///     <para>
        ///         Use the <paramref name="trustedKeys" /> params to search for the Public Keys. Only Assambly with one of that keys will included
        ///         Set <code>#DEFINE SECLOADING</code> to Compile this function
        ///     </para>
        /// </summary>
        /// <param name="path">The Paths to search</param>
        /// <param name="watchDirectorys">Set this to True to enable the AutoWatch function </param>
        /// <param name="trustedKeys">All Public keys that the application should trust</param>
        public StrongNameCatalog(IEnumerable<string> path, bool watchDirectorys, params byte[][] trustedKeys)
        {
            _paths = path;
            _watchDirectorys = watchDirectorys;
            _trustedKeys = trustedKeys;
            LowPriorityList = new ConcurrentQueue<string>();
            Init();
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return _aggregateCatalog.Parts; }
        }

        public ConcurrentQueue<string> LowPriorityList { get; set; }

        private void Init()
        {
            //Debugger.Launch();
            foreach (string internalpath in _paths)
            {
                if (!Directory.Exists(internalpath))
                    continue;
                if (_watchDirectorys)
                    _watchers.Add(CreateWatcher(internalpath));

                string[] files = Directory.GetFiles(internalpath, "*.dll");

                IEnumerable<IGrouping<string, string>> duplicates = files
                    .GroupBy(i => i)
                    .Where(g => g.Count() > 1);

                foreach (var group in duplicates)
                {
                    Assembly currass = Assembly.LoadFrom(group.First());
                    var exep = new List<string>();

                    foreach (string assamblys in group)
                    {
                        Assembly buffass = Assembly.LoadFrom(assamblys);
                        if (buffass.GetName().Version > currass.GetName().Version)
                            currass = buffass;
                        else
                            exep.Add(assamblys);
                    }
                    files = files.Except(exep).ToArray();
                }

#if !NonParallelEnumeration
                ParallelLoopResult parallelLoopResult = Parallel.ForEach(files, CheckAndAddAssambly);

                if (!parallelLoopResult.IsCompleted)
                    throw new ArgumentException("The Strong name Catalog is Broken in his Constructor");

                var task = new Task(() =>
                {
                    foreach (string lowpriorityassambly in LowPriorityList)
                        CheckAndAddAssambly(lowpriorityassambly, () => { });
                });
                task.Start();
#else
                foreach (var file in files)
                {
                    CheckAndAddAssambly(file, () => { });
                }
#endif
            }
        }

        private void CheckAndAddAssambly(string filename, ParallelLoopState state)
        {
            string fileName = Path.GetFileName(filename);
            if (!fileName.ToUpper().StartsWith("IEADPC"))
            {
                LowPriorityList.Enqueue(fileName);
                return;
            }

            CheckAndAddAssambly(filename, state.Stop);
        }

        private void CheckAndAddAssambly(string filename, Action cancel)
        {
            AssemblyName assemblyName = null;

            try
            {
                assemblyName = AssemblyName.GetAssemblyName(filename);
            }
            catch (ArgumentException e)
            {
                IsHandeld(e, filename);
                return;
            }
            catch (BadImageFormatException e)
            {
                IsHandeld(e, filename);
                return;
            }
            catch (FileLoadException e)
            {
                IsHandeld(e, filename);
                return;
            }
            catch (FileNotFoundException e)
            {
                IsHandeld(e, filename);
                return;
            }
            catch (PlatformNotSupportedException e)
            {
                IsHandeld(e, filename);
                return;
            }
            catch (SecurityException e)
            {
                IsHandeld(e, filename);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine("first chance exception of type '" + e.GetType() + "' in assambly '" + filename +
                                  "' was NOT Handeled by StrongNameCatalog ...");
                cancel();
            }

            if (assemblyName == null)
                return;
#if (SECLOADING)
                                    var publicKey = assemblyName.GetPublicKey();

                                    if (publicKey == null) 
                                        continue;

                                    var trusted = _trustedKeys.Any(trustedKey => assemblyName.GetPublicKey().SequenceEqual(trustedKey));

                                    if (!trusted)
                                        continue;
#endif
            try
            {
                var assam = new AssemblyCatalog(filename);

                if (assam.Parts.Count() == 0)
                {
                    ImportPool.AddNotAnImportMessage(assam.Assembly);
                    return;
                }

                if (_aggregateCatalog.Catalogs.Any(s => s.ToString() == assam.ToString()))
                    return;

                if (Changing != null)
                {
                    Changing.Invoke(this,
                                    new ComposablePartCatalogChangeEventArgs(assam.Parts,
                                                                             new List<ComposablePartDefinition>(), null));
                }

                _aggregateCatalog.Catalogs.Add(assam);
                ImportPool.AddSuccessIncludedMessage(filename);

                if (Changed != null)
                {
                    Changed.Invoke(this,
                                   new ComposablePartCatalogChangeEventArgs(assam.Parts,
                                                                            new List<ComposablePartDefinition>(), null));
                }
            }
            catch (Exception e)
            {
                IsHandeld(e, filename);
                return;
            }
        }

        /// <summary>
        ///     Unloads a Assambly From the Mef Container.
        ///     It does NOT unload it from the Domain
        /// </summary>
        /// <param name="assambly">Your Target assambly</param>
        public void FreeAssambly(Assembly assambly)
        {
            string assemblyName = assambly.Location;
            IEnumerable<AssemblyCatalog> all = _aggregateCatalog.Catalogs.Where(s =>
            {
                var assemblyCatalog = s as AssemblyCatalog;
                if (assemblyCatalog != null)
                {
                    Assembly assamblName = assemblyCatalog.Assembly;
                    string fn = assamblName.Location;
                    return assemblyName == fn;
                }
                return false;
            }).Cast<AssemblyCatalog>();

            AssemblyCatalog[] assemblyCatalogs = all as AssemblyCatalog[] ?? all.ToArray();

            if (!assemblyCatalogs.Any())
            {
                ImportPool.AddImportFailMessage(assambly);
                return;
            }

            AssemblyCatalog first;
            if (assemblyCatalogs.Count() != 1)
            {
                Version version = assemblyCatalogs.Max(s => AssemblyName.GetAssemblyName(s.Assembly.FullName).Version);
                first =
                    assemblyCatalogs.FirstOrDefault(
                        s => AssemblyName.GetAssemblyName(s.Assembly.FullName).Version == version);
            }
            else
                first = assemblyCatalogs.FirstOrDefault();

            if (first != null)
            {
                IQueryable<ComposablePartDefinition> parts = first.Parts;

                if (Changing != null)
                {
                    Changing.Invoke(this,
                                    new ComposablePartCatalogChangeEventArgs(new List<ComposablePartDefinition>(), parts,
                                                                             null));
                }

                _aggregateCatalog.Catalogs.Remove(first);

                ImportPool.AddSuccessExcludedMessage(assambly);

                if (Changed != null)
                {
                    Changed.Invoke(this,
                                   new ComposablePartCatalogChangeEventArgs(new List<ComposablePartDefinition>(), parts,
                                                                            null));
                }
            }
        }

        private FileSystemWatcher CreateWatcher(string path)
        {
            var watcher = new FileSystemWatcher(path)
                {
                    IncludeSubdirectories = false,
                    EnableRaisingEvents = true,
                    NotifyFilter = NotifyFilters.FileName
                };
            watcher.Created += OnChanged;
            return watcher;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Created)
                return;

            string extension = Path.GetExtension(e.FullPath);
            if (extension != null && extension.ToLower() == ".dll")
                CheckAndAddAssambly(e.FullPath, () => { });
        }

        /// <summary>
        ///     Simple helper
        /// </summary>
        /// <param name="e"></param>
        /// <param name="assamblyname"></param>
        private void IsHandeld(Exception e, string assamblyname)
        {
            ImportPool.AddFailMessage(assamblyname, e);
        }

        public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>>
            GetExports(ImportDefinition definition)
        {
            return _aggregateCatalog.GetExports(definition);
        }

        #region Implementation of INotifyComposablePartCatalogChanged

        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed;
        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing;

        #endregion
    }
}