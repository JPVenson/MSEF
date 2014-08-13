using System.Collections.ObjectModel;
using JPB.Shell.CommonContracts.Interfaces.Application;
using Microsoft.Windows.Controls.Ribbon;

namespace JPB.Shell.CommonAppliationContainer.Services.ModuleServices.VisualModule
{
    public class RibbonGroup : IRibbonGroup
    {
        public RibbonGroup(int groupIndex, string header, ObservableCollection<RibbonButton> buttons)
        {
            Header = header;
            Buttons = buttons;
            GroupIndex = groupIndex;
        }

        public RibbonGroup(int groupIndex, string header, RibbonButton buttons)
        {
            Header = header;
            Buttons = new ObservableCollection<RibbonButton>(new[] {buttons});
            GroupIndex = groupIndex;
        }

        public int GroupIndex { get; private set; }
        public ObservableCollection<RibbonButton> Buttons { get; private set; }
        public string Header { get; private set; }
    }
}