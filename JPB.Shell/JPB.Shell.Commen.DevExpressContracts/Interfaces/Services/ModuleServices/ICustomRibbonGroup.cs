#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 14:33

#endregion

using System.Collections.ObjectModel;
using DevExpress.Xpf.Bars;

namespace IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ModuleServices
{
    public interface ICustomRibbonGroup
    {
        string Name { get; }
        ObservableCollection<BarItem> RibbonElements { get; }
    }
}