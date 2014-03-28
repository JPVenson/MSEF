using System.Collections.ObjectModel;

namespace JPB.Shell.CommonContracts.Interfaces.Application
{
    public interface IRibbonTab
    {
        int TabIndex { get; }
        string Header { get; }
        ObservableCollection<IRibbonGroup> Groups { get; }
    }
}