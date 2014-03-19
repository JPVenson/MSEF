using System.Collections.ObjectModel;
using DevExpress.Xpf.Bars;

namespace IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ModuleServices
{
    public class CustomRibbonGroup : ICustomRibbonGroup
    {
        public CustomRibbonGroup(string name, ObservableCollection<BarItem> ribbonElements)
        {
            RibbonElements = ribbonElements;
            Name = name;
        }

        public CustomRibbonGroup(string name, BarItem ribbonElements)
        {
            RibbonElements = new ObservableCollection<BarItem> {ribbonElements};
            Name = name;
        }

        #region Implementation of ICustomRibbonGroup

        public string Name { get; private set; }

        public ObservableCollection<BarItem> RibbonElements { get; private set; }

        #endregion
    }
}