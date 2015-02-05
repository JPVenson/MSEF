
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