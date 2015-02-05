
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