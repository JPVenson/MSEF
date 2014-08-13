#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 15:19

#endregion

using System.Collections.Generic;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Contracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services.ApplicationServices;
using JPB.Shell.Contracts.Interfaces.Services.ModuleServices;
using JPB.Shell.Contracts.Interfaces.Services.ShellServices;
using JPB.Shell.MEF.Services.Extentions;

namespace JPB.Shell.MEF.Services
{
    public class VisualModuleManager : IVisualModuleManager
    {
        private static IVisualModuleManager _instance = new VisualModuleManager();

        private VisualModuleManager()
        {
        }

        /// <summary>
        ///     The Current Instace of <see cref="IVisualModuleManager" />
        /// </summary>
        public static IVisualModuleManager Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        #region Implementation of IVisualModuleProvider

        /// <summary>
        ///     TO BE SUPPLIED
        /// </summary>
        public void NotifyServicesChanged()
        {
        }

        /// <summary>
        ///     Gets all <see cref="IVisualServiceMetadata" />
        /// </summary>
        /// <returns>
        ///     All found <see cref="IVisualServiceMetadata" />
        /// </returns>
        public IEnumerable<IVisualServiceMetadata> GetVisualServicesMetadata()
        {
            IEnumerable<IVisualServiceMetadata> metadata = ServicePool.Instance.GetMetadatas<IVisualServiceMetadata>();
            return metadata;
        }

        /// <summary>
        ///     Gets a Service on base of this Descriptor
        ///     If there are more than one IVisualService with that descriptor, only the First will be Select
        ///     Ignores all else
        ///     Ignores all <see cref="IServiceMetadata.IsDefauldService" />
        /// </summary>
        /// <typeparam name="T">Your Service interface</typeparam>
        /// <param name="descriptor">The Targed Descriptor that is Equals to the Service you want [CASE SENSETIV]</param>
        /// <returns>The Service [Can be Null]</returns>
        public T CreateService<T>(string descriptor) where T : class, IVisualService
        {
            var service = ServicePool.Instance.GetSingelService<T>(descriptor);
            if (service != null)
                service.OnStart(ServicePool.ApplicationContainer);
            return service;
        }

        /// <summary>
        ///     Gets a Service on based of his Metadata
        /// </summary>
        /// <typeparam name="T">Your Service interface</typeparam>
        /// <param name="serviceMetadata">Your Service Metadata</param>
        /// <returns>The Service [Can be Null]</returns>
        public T CreateService<T>(IVisualServiceMetadata serviceMetadata) where T : class, IVisualService
        {
            var service = ServicePool.Instance.GetSingelService<T>(serviceMetadata);
            if (service != null)
                service.OnStart(ServicePool.ApplicationContainer);
            return service;
        }

        /// <summary>
        ///     Gets the Service where the <see cref="IServiceMetadata.IsDefauldService" /> property is true
        ///     If more than one Service match this condition, this Function will call the <see cref="IIncidentFixerService" />
        ///     Service
        /// </summary>
        /// <typeparam name="T">
        ///     Your <see cref="IVisualService" />
        /// </typeparam>
        /// <returns></returns>
        public T CreateDefaultVisualService<T>() where T : class, IVisualService
        {
            return ServicePool.Instance.GetDefaultSingelService<T>();
        }

        #endregion

        #region Implementation of IService

        public void OnStart(IApplicationContext application)
        {
        }

        #endregion
    }
}