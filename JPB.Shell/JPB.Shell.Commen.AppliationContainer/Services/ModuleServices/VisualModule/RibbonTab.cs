using System.Collections.ObjectModel;
using JPB.Shell.CommonContracts.Interfaces.Application;

namespace JPB.Shell.VisualServiceScheduler.Services
{
    public class RibbonTab : IRibbonTab
    {
        public RibbonTab(int tabIndex, string header, ObservableCollection<IRibbonGroup> groups)
        {
            Groups = groups;
            TabIndex = tabIndex;
            Header = header;
        }

        public RibbonTab(int tabIndex, string header, IRibbonGroup group)
        {
            Groups = new ObservableCollection<IRibbonGroup>(new[] { group });
            TabIndex = tabIndex;
            Header = header;
        }

        public int TabIndex { get; private set; }
        public string Header { get; private set; }
        public ObservableCollection<IRibbonGroup> Groups { get; private set; }
    }
}