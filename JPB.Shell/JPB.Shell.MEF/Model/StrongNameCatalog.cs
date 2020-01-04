
//By Jean-Pierre Bachmann
//http://www.codeproject.com/Articles/682642/Using-a-Plugin-Based-Application
//Questions? Ask me!
//Microsoft Public License (MS-PL)
/*
This license governs use of the accompanying software. If you use the software, you
accept this license. If you do not accept the license, do not use the software.

1. Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the
same meaning here as under U.S. copyright law.
A "contribution" is the original software, or any additions or changes to the software.
A "contributor" is any person that distributes its contribution under this license.
"Licensed patents" are a contributor's patent claims that read directly on its contribution.

2. Grant of Rights
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

3. Conditions and Limitations
(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.

All at all - Be carefull when using this code and do not remove the License!
*/

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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using JPB.Shell.MEF.Log;
using JPB.Shell.MEF.Properties;

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
        ///         Use the <paramref name="trustedKeys" /> params to search for the Public Keys. Only Assambly with one of that
        ///         keys will included
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
        }

        public string PriorityKey { get; set; }

        public bool WithCheckForDuplicates { get; set; }

        public bool HasChanged { get; set; }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return _aggregateCatalog.Parts; }
        }

        public ConcurrentQueue<string> LowPriorityList { get; set; }

        public void AsyncInit()
        {
            //Debugger.Launch();
            foreach (var internalpath in _paths.Where(Directory.Exists))
            {
                if (_watchDirectorys)
                {
	                _watchers.Add(CreateWatcher(internalpath));
                }

                var files = Directory.GetFiles(internalpath, "*.dll");

                if (WithCheckForDuplicates)
                {
                    var duplicates = files
                        .GroupBy(i => i)
                        .Where(g => g.Count() > 1);

                    foreach (var group in duplicates)
                    {
                        var currass = AssemblyName.GetAssemblyName(@group.First());
                        var exep = new List<string>();

                        foreach (var assamblys in @group)
                        {
                            var buffass = AssemblyName.GetAssemblyName(assamblys);
                            if (buffass.Version > currass.Version)
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
                }

                var parallelLoopResult = Parallel.ForEach(files, CheckAndAddAssambly);

                if (!parallelLoopResult.IsCompleted)
                {
	                throw new ArgumentException("The Strong name Catalog is Broken in his Constructor");
                }

                var thread = new Thread(() =>
                {
                    foreach (var lowpriorityassambly in LowPriorityList)
                    {
	                    CheckAndAddAssambly(lowpriorityassambly, () => { });
                    }
                });
                thread.SetApartmentState(ApartmentState.MTA);
                thread.Priority = ThreadPriority.Highest;
                thread.IsBackground = true;
                thread.Name = "LowPriportyAssamblyCheck";
                thread.Start();
            }
        }

        private void CheckAndAddAssambly([NotNull] string filename, ParallelLoopState state)
        {
            var fileName = Path.GetFileName(filename);

            if (fileName != null &&
                (!string.IsNullOrEmpty(PriorityKey) &&
                 !Regex.IsMatch(fileName, PriorityKey, RegexOptions.CultureInvariant)))
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
                IsHandeld(e, filename);
                Console.WriteLine("first chance exception of type '" + e.GetType() + "' in assambly '" + filename +
                                  "' was NOT Handeled by StrongNameCatalog ...");
                cancel();
            }

            if (assemblyName == null)
            {
	            return;
            }
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
                {
                    ImportPool.AddNotAnImportMessage(assam.Assembly);
                    return;
                }

                if (_aggregateCatalog.Catalogs.Any(s => s.ToString() == assam.ToString()))
                {
	                return;
                }

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
            }
        }

        /// <summary>
        ///     Unloads a Assambly From the Mef Container.
        ///     It does NOT unload it from the Domain
        /// </summary>
        /// <param name="assambly">Your Target assambly</param>
        public void FreeAssambly(Assembly assambly)
        {
            var assemblyName = assambly.Location;
            var all = _aggregateCatalog.Catalogs.Where(s =>
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
                first =
                    assemblyCatalogs.FirstOrDefault(
                        s => AssemblyName.GetAssemblyName(s.Assembly.FullName).Version == version);
            }
            else
            {
	            first = assemblyCatalogs.FirstOrDefault();
            }

            if (first != null)
            {
                var parts = first.Parts;

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
            {
	            return;
            }

            var extension = Path.GetExtension(e.FullPath);
            if (extension != null && extension.ToLower() == ".dll")
            {
	            CheckAndAddAssambly(e.FullPath, () => { });
            }
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