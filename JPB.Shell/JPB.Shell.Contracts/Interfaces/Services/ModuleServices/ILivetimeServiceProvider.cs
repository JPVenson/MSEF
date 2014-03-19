#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 15:31
#endregion

using System;
using System.Collections.Generic;
using IEADPC.Shell.Contracts.Interfaces.Metadata;

namespace IEADPC.Shell.Contracts.Interfaces.Services.ModuleServices
{
    public interface ILivetimeServiceProvider : IService
    {
        IService AddLivetimeService(Lazy<IService, IServiceMetadata> service);
        bool DestroyLivetimeService(Lazy<IService, IServiceMetadata> service);
        bool Contains(IServiceMetadata service);
        long Count(IServiceMetadata service);
    }
}