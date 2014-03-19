#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 12:14
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using IEADPC.Shell.Log;

namespace IEADPC.Shell.Model
{
    internal class StrongNameCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged
    {
        /// <summary>
        /// StrongNameCatalog Constructor
        /// <para>
        /// Use this Ctor to get all dll's that export anything in the spezific paths
        /// </para>
        /// <para>
        /// Use the <paramref name="trustedKeys"/> params to search for the Public Keys. Only Assambly with one of that keys will included
        /// Set <code>#DEFINE SECLOADING</code> to Compile this function
        /// </para>
        /// </summary>
        /// <param name="path">The Paths to search</param>
        /// <param name="watchDirectorys">Set this to True to enable the AutoWatch function </param>
        /// <param name="trustedKeys">All Public keys that the application should trust</param>
        public StrongNameCatalog(IEnumerable<string> path, bool watchDirectorys, params byte[][] trustedKeys)
        {
            _paths = path;
            _watchDirectorys = watchDirectorys;
            _trustedKeys = trustedKeys;
            Init();
        }

        private readonly IEnumerable<string> _paths;
        private readonly bool _watchDirectorys;
        private readonly byte[][] _trustedKeys;
        private readonly List<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();

        private void Init()
        {
            //Debugger.Launch();
            foreach (var internalpath in _paths)
            {
                if (!Directory.Exists(internalpath))
                    continue;
                if (_watchDirectorys)
                    _watchers.Add(CreateWatcher(internalpath));

                var files = Directory.GetFiles(internalpath, "*.dll");

                var duplicates = files
                    .GroupBy(i => i)
                    .Where(g => g.Count() > 1);

                foreach (var group in duplicates)
                {
                    var currass = Assembly.LoadFrom(group.First());
                    var exep = new List<string>();

                    foreach (var assamblys in group)
                    {
                        var buffass = Assembly.LoadFrom(assamblys);
                        if (buffass.GetName().Version > currass.GetName().Version)
                        {
                            currass = buffass;
                        }
                        else
                        {
                            exep.Add(assamblys);
                        }
                    }
                    files = files.Except(exep).ToArray();
                }

                var parallelLoopResult = Parallel.ForEach(files, CheckAndAddAssambly);

                if(!parallelLoopResult.IsCompleted)
                    throw new ArgumentException("The Strong name Catalog is Broken in his Constructor");
                //foreach (var duplicate in duplicates)
            }
        }

        void CheckAndAddAssambly(string filename, ParallelLoopState state)
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
                Console.WriteLine("first chance exception of type '" + e.GetType().ToString() + "' in assambly '" + filename + "' was NOT Handeled by StrongNameCatalog ...");
                state.Stop();
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

                if (!assam.Parts.Any())
                    return;

                if (Changing != null)
                    Changing.Invoke(this, new ComposablePartCatalogChangeEventArgs(assam.Parts, new List<ComposablePartDefinition>(), null));

                _aggregateCatalog.Catalogs.Add(assam);
                ImportPool.AddSuccessIncludedMessage(filename);

                if (Changed != null)
                    Changed.Invoke(this, new ComposablePartCatalogChangeEventArgs(assam.Parts, new List<ComposablePartDefinition>(), null));

            }
            catch (Exception e)
            {
                IsHandeld(e, filename);
                return;
            }
        }

        /// <summary>
        /// Unloads a Assambly From the Mef Container.
        /// It does NOT unload it from the Domain
        /// </summary>
        /// <param name="assambly">Your Target assambly</param>
        public void FreeAssambly(Assembly assambly)
        {
            var assemblyName = assambly.Location;
            IEnumerable<AssemblyCatalog> all = _aggregateCatalog.Catalogs.Where(s =>
                {
                    var assemblyCatalog = s as AssemblyCatalog;
                    if (assemblyCatalog != null)
                    {
                        var assamblName = assemblyCatalog.Assembly;
                        var fn = assamblName.Location;
                        return assemblyName == fn;
                    }
                    return false;
                }).Cast<AssemblyCatalog>();

            var assemblyCatalogs = all as AssemblyCatalog[] ?? all.ToArray();

            if (!assemblyCatalogs.Any())
            {
                ImportPool.AddImportFailMessage(assambly);
                return;
            }

            AssemblyCatalog first;
            if (assemblyCatalogs.Count() != 1)
            {
                var version = assemblyCatalogs.Max(s => AssemblyName.GetAssemblyName(s.Assembly.FullName).Version);
                first = assemblyCatalogs.FirstOrDefault(s => AssemblyName.GetAssemblyName(s.Assembly.FullName).Version == version);
            }
            else
            {
                first = assemblyCatalogs.FirstOrDefault();
            }

            if (first != null)
            {
                var parts = first.Parts;

                if (Changing != null)
                    Changing.Invoke(this, new ComposablePartCatalogChangeEventArgs(new List<ComposablePartDefinition>(), parts, null));

                _aggregateCatalog.Catalogs.Remove(first);

                ImportPool.AddSuccessExcludedMessage(assambly);

                if (Changed != null)
                    Changed.Invoke(this, new ComposablePartCatalogChangeEventArgs(new List<ComposablePartDefinition>(), parts, null));
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

            var extension = Path.GetExtension(e.FullPath);
            if (extension != null && extension.ToLower() == ".dll")
            {
                CheckAndAddAssambly(e.FullPath, null);
            }
        }

        /// <summary>
        /// Simple helper
        /// </summary>
        /// <param name="e"></param>
        /// <param name="assamblyname"></param>
        void IsHandeld(Exception e, string assamblyname)
        {
            ImportPool.AddFailMessage(assamblyname, e);
        }

        readonly AggregateCatalog _aggregateCatalog = new AggregateCatalog();

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get
            {
                return _aggregateCatalog.Parts;
            }
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