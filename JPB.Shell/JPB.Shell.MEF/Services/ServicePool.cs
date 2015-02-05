
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

// Erstellt von Jean-Pierre Bachmann am 10:32

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Contracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services;
using JPB.Shell.Contracts.Interfaces.Services.ApplicationServices;
using JPB.Shell.Contracts.Interfaces.Services.ModuleServices;
using JPB.Shell.Contracts.Interfaces.Services.ShellServices;
using JPB.Shell.MEF.Factorys;
using JPB.Shell.MEF.Log;
using JPB.Shell.MEF.Log.Model;
using JPB.Shell.MEF.Model;
using JPB.Shell.MEF.Properties;

namespace JPB.Shell.MEF.Services
{
    [DebuggerStepThrough]
    public class ServicePool : IServicePool
    {
        internal ServicePool(string priorityKey, string[] sublookuppaths)
        {
            _strongNameCatalog = new StrongNameCatalog(sublookuppaths, true);
            _strongNameCatalog.PriorityKey = priorityKey;
            _strongNameCatalog.AsyncInit();

            Container = new CompositionContainer(_strongNameCatalog);
            ImportPool.Instance.ServiceLoad += Instance_ServiceLoad;

            _callbacks = new List<Action<IService>>();
        }

        //internal static ServicePool CreateParamServicePool(string priorityKey, params string[] subPaths)
        //{
        //    var pool = new ServicePool(priorityKey, subPaths);
        //    Instance = pool;
        //    if (ApplicationContainer == null)
        //        ApplicationContainer = new ApplicationContext(ImportPool.Instance, MessageBroker.Instance, pool,
        //            DataBroker.Instance, VisualModuleManager.Instance);
        //    pool.InitLoading();
        //    return pool;
        //}

        //internal static ServicePool CreateParamServicePool(string priorityKey)
        //{
        //    return CreateParamServicePool(priorityKey,
        //        new[] {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)});
        //}

        //internal static ServicePool CreateServicePool()
        //{
        //    return CreateParamServicePool(string.Empty);
        //}

        //public static void PreLoadServicePool(string priorityKey)
        //{
        //    Instance = CreateParamServicePool(priorityKey);
        //}

        //public static void PreLoadServicePool(string priorityKey, params string[] sublookuppaths)
        //{
        //    Instance = CreateParamServicePool(priorityKey, sublookuppaths);
        //}

        private List<Action<IService>> _callbacks;

        public void RegisterCallbackForServiceInit(Action<IService> serivce)
        {
            _callbacks.Add(serivce);
        }

        void Instance_ServiceLoad(IService obj)
        {
            foreach (var callback in _callbacks.Where(callback => callback.GetMethodInfo().GetGenericArguments().Contains(obj.GetType())))
            {
                callback(obj);
            }
        }

        /// <summary>
        ///     Used to get all <see cref="IApplicationProvider" /> in all exports to Provide a Initial Loading
        /// </summary>
        internal void InitLoading()
        {
            var serviceInternal = GetServiceInternal(false);

            var enumerable =
                serviceInternal.Where(s => s.Metadata.Contracts.Any(f => f == typeof(IApplicationProvider))).ToList();
            var asyncservices =
                enumerable.Where(s => !s.Metadata.ForceSynchronism).ToList();

            foreach (var asyncservice in asyncservices.OrderBy(s => s.Metadata.Priority))
            {
                try
                {
                    var asyncservice1 = asyncservice;
                    Task.Factory.StartNew(() => asyncservice1.Value);
                }
                catch (Exception ex)
                {
                    if (ApplicationContainer.ImportPool != null)
                        ApplicationContainer.ImportPool.LogEntries.Add(
                            new LogEntry(
                                string.Format("Error on startup of module: {0}", asyncservice.Metadata.Descriptor),
                                new Dictionary<string, object> { { "Exeption", ex } }));
                }
            }

            foreach (var val in enumerable.Except(asyncservices).OrderBy(s => s.Metadata.Priority))
            {
                try
                {
                    var service = val.Value;
                }
                catch (Exception ex)
                {
                    if (ApplicationContainer.ImportPool != null)
                        ApplicationContainer.ImportPool.LogEntries.Add(
                            new LogEntry(string.Format("Error on startup of module: {0}", val.Metadata.Descriptor),
                                new Dictionary<string, object> { { "Exeption", ex } }));
                }
            }
        }

        #region Propertys

        #region Container property

        /// <summary>
        ///     The Internal MEF Container.
        ///     This code is Internal and should be not used from your code!
        /// </summary>
        internal CompositionContainer Container { get; set; }

        #endregion

        #endregion

        #region Static's

        /// <summary>
        ///     The Defauld Application Container
        ///     This is used from ALL <see cref="IService" /> to comunicate with the <see cref="ServicePool" />
        /// </summary>
        public static IApplicationContext ApplicationContainer { get; set; }

        /// <summary>
        ///     The Current <see cref="IServicePool" /> Instance
        /// </summary>
        public static IServicePool Instance
        {
            get { return _instance ?? (_instance = ServicePoolFactory.CreatePool()); }
            internal set { _instance = value; }
        }

        #endregion

        #region Default Functions

        public void FreeAssambly(Assembly assambly)
        {
            _strongNameCatalog.FreeAssambly(assambly);
        }

        /// <summary>
        ///     Register a Handler for the internal <see cref="Container" />
        /// </summary>
        /// <param name="exportsChange">Your Eventhandler</param>
        /// <returns>True if the Operation was Successful</returns>
        public bool RegisterExportsChanged(EventHandler<ExportsChangeEventArgs> exportsChange)
        {
            try
            {
                Container.ExportsChanged += exportsChange;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Register a Handler for the internal <see cref="Container" />
        /// </summary>
        /// <param name="exportsChangeing">Your Eventhandler</param>
        /// <returns>True if the Operation was Successful</returns>
        public bool RegisterExportsChanging(EventHandler<ExportsChangeEventArgs> exportsChangeing)
        {
            try
            {
                Container.ExportsChanging += exportsChangeing;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Import object that are marked with the Export Attrebute into your <paramref name="part" /> where your class
        ///     container Import Flags
        ///     <a
        ///         href="http://msdn.microsoft.com/en-us/library/system.componentmodel.composition.importattribute%28v=vs.100%29.aspx">
        ///         ImportAttribute
        ///     </a>
        /// </summary>
        /// <param name="part">
        ///     your <paramref name="part" /> item
        /// </param>
        /// <returns>True if the Operation was Successful</returns>
        public bool Import([NotNull] ComposablePart part)
        {
            try
            {
                Container.SatisfyImportsOnce(part);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Compose your <paramref name="composition" /> with the current <see cref="Container" />
        /// </summary>
        /// <param name="composition">
        ///     Your <see cref="CompositionBatch" />
        /// </param>
        /// <returns>True if the Operation was Successful</returns>
        public bool Compose([Required] CompositionBatch composition)
        {
            try
            {
                Container.Compose(composition);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        // This Code is Internal and should not be used directly from your code

        /// <summary>
        ///     Gets the Service where the <see cref="IServiceMetadata.IsDefauldService" /> property is true
        ///     If more than one Service match this condition, this Function will call the <see cref="IIncidentFixerService" />
        ///     Service
        /// </summary>
        /// <typeparam name="T">Your Service interface</typeparam>
        /// <returns>
        ///     The found <typeparamref name="T" />
        /// </returns>
        /// <exception cref="NotImplementedException">
        ///     Thrown when There is no Unique <typeparamref name="T" /> and no Incident Fixer for <typeparamref name="T" />
        /// </exception>
        public T GetDefaultSingelService<T>() where T : class, IService
        {
            var exports = GetServiceInternal(false);
            var defaultservice = exports.Where(m => m.Metadata.IsDefauldService && m.Metadata.Contracts.Any(f => f == typeof(T)));

            var defauldInplementations = defaultservice as Lazy<IService, IServiceMetadata>[] ?? defaultservice.ToArray();

            if (defauldInplementations.Count() == 1)
            {
                var service = defauldInplementations.First();
                if (service != null)
                    return service.Value as T;
            }

            if (IsIncident || !defauldInplementations.Any())
                ThrowNoInplementationFoundEx<T>();

            Console.WriteLine("On Incident TargetType:" + typeof(T));

            IsIncident = true;

            var rightservice = GetDefaultSingelService<IIncidentFixerService>();

            if (rightservice != null)
            {
                Lazy<IService, IServiceMetadata> service = rightservice.OnIncident(defauldInplementations);

                if (service == null)
                    ThrowNoInplementationFoundEx<T>();

                IsIncident = false;

                if (service != null)
                    return service.Value as T;
            }

            IsIncident = false;
            ThrowNoInplementationFoundEx<T>();
            return null;
        }

        /// <summary>
        ///     Gets the first <typeparamref name="T" />
        ///     Ignores all else
        ///     Ignores all <see cref="IServiceMetadata.IsDefauldService" />
        /// </summary>
        /// <typeparam name="T">Your Service interface</typeparam>
        /// <returns>
        ///     <typeparamref name="T" />
        /// </returns>
        public T GetSingelService<T>() where T : class, IService
        {
            var serv = GetServiceInternal().FirstOrDefault(m => m.Metadata.Contracts.Any(f => f == typeof(T)));
            if (serv == null)
                return null;
            return serv.Value as T;

        }

        /// <summary>
        ///     Gets the first <typeparamref name="T" />
        ///     Ignores all else
        ///     Ignores all <see cref="IServiceMetadata.IsDefauldService" />
        /// </summary>
        /// <typeparam name="T">Your Service interface</typeparam>
        /// <param name="output">
        ///     The found <typeparamref name="T" /> will be Marshaled back to you
        /// </param>
        /// <returns>True if the Operation was Successful</returns>
        public bool TryGetSingelService<T>([Out] [Required] T output) where T : class, IService
        {
            output = default(T);
            try
            {
                output = GetSingelService<T>();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private T GetCreatedValue<T>(Lazy<IService, IServiceMetadata> source) where T : class, IService
        {
            if (source == null)
                return null;

            if (!source.IsValueCreated)
            {
                var service = source.Value;
                ImportPool.Instance.OnServiceInitLoad(service);
                source.Value.OnStart(ApplicationContainer);
            }
            return (T)source.Value;
        }

        /// <summary>
        ///     Get all Services that match <typeparamref name="T" />
        ///     Ignores all <see cref="IServiceMetadata.IsDefauldService" />
        /// </summary>
        /// <typeparam name="T">Your Service interface</typeparam>
        /// <returns>All found Services</returns>
        public IEnumerable<T> GetServices<T>() where T : class, IService
        {
            return GetServiceInternal()
                .Where(m => m.Metadata.Contracts.Any(f => f == typeof(T)))
                .Select(s => GetCreatedValue<T>(s));
        }

        /// <summary>
        ///     Get all <typeparamref name="T" />
        ///     Ignores all <see cref="IServiceMetadata.IsDefauldService" />
        /// </summary>
        /// <typeparam name="T">Your Service interface</typeparam>
        /// <param name="output">
        ///     The found <typeparamref name="T" /> will be Marshaled back to you
        /// </param>
        /// <returns>True if the Operation was Successful</returns>
        public bool TryGetServices<T>([Out] [Required] IEnumerable<T> output) where T : class, IService
        {
            output = default(IEnumerable<T>);
            try
            {
                output = GetServices<T>();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Gets all Metadata
        /// </summary>
        /// <param name="output">The found Metadata will be Marshaled back to you</param>
        /// <returns>True if the Operation was Successful</returns>
        public bool TryGetMetadata([Out] [Required] IEnumerable<IServiceMetadata> output)
        {
            output = default(IEnumerable<IServiceMetadata>);
            try
            {
                output = GetMetadatas();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        ///     Gets all Metadata of type <typeparamref name="T" />
        /// </summary>
        /// <typeparam name="T">Your ServiceMetadata</typeparam>
        /// <returns>All found metadata</returns>
        public IEnumerable<T> GetMetadatas<T>() where T : class, IServiceMetadata
        {
            IEnumerable<Lazy<IService, T>> export = Container.GetExports<IService, T>();
            return export.Where(s => !s.Metadata.IsDefauldService).Select(s => s.Metadata);
        }

        /// <summary>
        ///     Gets all Metadata of type <see cref="IServiceMetadata" />
        ///     Ignores all <see cref="IServiceMetadata.IsDefauldService" />
        /// </summary>
        /// <returns>All found IServiceMetadata</returns>
        public IEnumerable<IServiceMetadata> GetMetadatas()
        {
            return GetServiceInternal().Select(s => s.Metadata);
        }

        /// <summary>
        ///     Gets all Metadata of type <see cref="IServiceMetadata" />
        /// </summary>
        /// <returns>All found IServiceMetadata</returns>
        public IEnumerable<IServiceMetadata> GetAllMetadata()
        {
            return GetServiceInternal(false).Select(s => s.Metadata);
        }

        private IEnumerable<Lazy<IService, IServiceMetadata>> _exportRef;

        /// <summary>
        ///     The General method to get Services without any kind of Filtering
        ///     This Code is Internal and should not be used directly from your code
        /// </summary>
        /// <param name="ignoreDefauld">
        ///     Ignore all services where the <see cref="IServiceMetadata.IsDefauldService" /> property true is
        /// </param>
        /// <returns>All found Services that match the Condition</returns>
        public IEnumerable<Lazy<IService, IServiceMetadata>> GetServiceInternal(bool ignoreDefauld = true)
        {
            if (_exportRef == null)
                _exportRef =
                    Container.GetExports<IService, IServiceMetadata>().Select(s => new Lazy<IService, IServiceMetadata>(
                        () =>
                        {
                            //Repack the Lazy to this one to support OnStart
                            var createdValue = this.GetCreatedValue<IService>(s);
                            return createdValue;
                        }, s.Metadata));

            if (ignoreDefauld)
            {
                return _exportRef.Where(s => !s.Metadata.IsDefauldService);
            }

            return _exportRef;
        }

        // This Code is private and should not be used directly from your code
        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /// <typeparam name="T"></typeparam>
        private static void ThrowNoInplementationFoundEx<T>()
        {
            throw new NotImplementedException("There is no unique inmplementation of the Service with the type : \r\n" +
                                              typeof(T) +
                                              "\r\nThe program can not Iditify one Module and tried to load the service but\r\n it allso does not found only one Defauld inplementation of the DefauldIncentFixerService.\r\n\r\n See Data")
            {
                Data = { { "Type", typeof(T) } }
            };
        }

        /// <summary>
        ///     Gets all Metadata of type <see cref="IServiceMetadata" />
        ///     Ignores all <see cref="IServiceMetadata.IsDefauldService" />
        ///     Ignores all non instantiated
        /// </summary>
        /// <returns>All found IServiceMetadata</returns>
        public IServiceMetadata GetMetadata(IService service)
        {
            return
                GetServiceInternal()
                    .Where(s => s.IsValueCreated && s.Value == service)
                    .Select(s => s.Metadata)
                    .FirstOrDefault();
        }

        #endregion

        #region Member

        private static IServicePool _instance;

        private readonly StrongNameCatalog _strongNameCatalog = default(StrongNameCatalog);

        /// <summary>
        ///     Used to Prevend the ServicePool for an StackOverflow
        ///     This Code is Internal and should not be used directly from your code
        ///     <seealso cref="GetDefaultSingelService{T}" />
        /// </summary>
        internal bool IsIncident = false;

        #endregion
    }
}