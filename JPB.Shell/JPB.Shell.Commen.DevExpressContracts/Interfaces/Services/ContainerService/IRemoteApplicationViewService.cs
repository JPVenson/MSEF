#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 13:16

#endregion

using System.Collections.ObjectModel;
using IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ModuleServices;
using IEADPC.Shell.Contracts.Interfaces.Services;

namespace IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ContainerService
{
    public interface IRemoteApplicationViewService : IService
    {
        IRibbonModuleAreaProvider SelectedModule { get; }
        void SetModule(string moduleName);
        ObservableCollection<string> TitleParts { get; set; }
    }
}