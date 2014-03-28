using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.Windows.Controls.Ribbon;

namespace JPB.Shell.CommonContracts.Interfaces.Application
{
    public interface IRibbonGroup
    {
        int GroupIndex { get; }
        ObservableCollection<RibbonButton> Buttons { get; }
        string Header { get; }
    }
}