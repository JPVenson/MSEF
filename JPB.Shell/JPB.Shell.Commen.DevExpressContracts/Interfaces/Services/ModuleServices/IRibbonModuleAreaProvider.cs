#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 09:58

#endregion

using System.Collections.ObjectModel;
using System.ComponentModel;
using IEADPC.Shell.Contracts.Interfaces.Services.ModuleServices;

namespace IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ModuleServices
{
    public interface IRibbonModuleAreaProvider : IVisualService
    {
        ObservableCollection<ICustomRibbonGroup> RibbonElements { get; }
        event PropertyChangedEventHandler RibbonElementsChanged;
        void SendRibbonElementsChanged();
    }
}