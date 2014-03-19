#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 15:15

#endregion

using System.Collections.Generic;
using JPB.Shell.Contracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services.ModuleServices;

namespace JPB.Shell.Contracts.Interfaces.Services.ShellServices
{
    public interface IVisualModuleManager : IService
    {
        void NotifyServicesChanged();
        IEnumerable<IVisualServiceMetadata> GetVisualServicesMetadata();

        T CreateService<T>(string serviceMetadataDescriptor) where T : class, IVisualService;
        T CreateService<T>(IVisualServiceMetadata serviceMetadata) where T : class, IVisualService;
        T CreateDefaultVisualService<T>() where T : class, IVisualService;
    }
}