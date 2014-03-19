#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 12:06

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Contracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services;

namespace JPB.Shell.MEF.Services
{
    //[ServiceExport("ManagedLivetimeServicePool", typeof(ILivetimeServiceProvider), true)]
    /// <summary>
    ///     This class is used to store Livetime ( quasi Singeltons ) Services
    /// </summary>
    public class ManagedLivetimeServicePool
    {
        private static ManagedLivetimeServicePool _instance;

        private ManagedLivetimeServicePool()
        {
            Services = new List<Tuple<IService, IServiceMetadata, WeakReference>>();
        }

        #region Instance property

        public static ManagedLivetimeServicePool Instance
        {
            get { return _instance ?? (_instance = new ManagedLivetimeServicePool()); }
        }

        #endregion

        #region Implementation of ILivetimeServiceProvider

        /// <summary>
        ///     All Stored Services
        /// </summary>
        public List<Tuple<IService, IServiceMetadata, WeakReference>> Services { get; set; }

        /// <summary>
        ///     Add a Service to the managed livetime Provider
        /// </summary>
        /// <param name="service">The service pack you want to add</param>
        /// <returns>The Instantiated service</returns>
        public void AddLivetimeService(Tuple<IService, IServiceMetadata> service)
        {
            var reff = new WeakReference(service.Item1, false);
            Services.Add(new Tuple<IService, IServiceMetadata, WeakReference>(service.Item1, service.Item2, reff));
        }

        /// <summary>
        ///     Destroy a Service and remove him from the Provider
        /// </summary>
        /// <param name="service">The Service you want to Destroy</param>
        /// <returns>If the Operation was Successful</returns>
        public bool DestroyLivetimeService(Tuple<IService, IServiceMetadata> service)
        {
            if (!Contains(service.Item2))
                return false;
            Tuple<IService, IServiceMetadata, WeakReference> firstOrDefault =
                Services.FirstOrDefault(s => s.Item2.Equals(service.Item2));
            return Services.Remove(firstOrDefault);
        }

        /// <summary>
        ///     Check if one of the Services exists
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public bool Contains(IServiceMetadata service)
        {
            return Services.FirstOrDefault(s => s.Item2.Equals(service)) != null;
        }

        /// <summary>
        ///     Count all Existing Services that match the Specific Metadata
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public long Count(IServiceMetadata service)
        {
            return Services.Count(s => s.Item2.Contracts == service.Contracts);
        }

        #endregion

        #region Implementation of IService

        public void OnStart(IApplicationContext application)
        {
        }

        #endregion
    }
}