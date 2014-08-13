using System.Collections.ObjectModel;
using System.Linq;
using JPB.Shell.CommonAppliationContainer.ViewModel;
using JPB.Shell.CommonContracts.Interfaces.Application;
using JPB.Shell.CommonContracts.Interfaces.Metadata;
using JPB.Shell.CommonContracts.Interfaces.Services.Ribbon;
using JPB.Shell.Contracts.Attributes;
using JPB.Shell.Contracts.Interfaces;
using Microsoft.Windows.Controls.Ribbon;

namespace JPB.Shell.CommonAppliationContainer.Services.ModuleServices.VisualModule
{
    [ServiceExport("Export", typeof (IRibbonContentProviderService))]
    public class RibbonContentProviderService : IRibbonContentProviderService
    {
        public void OnStart(IApplicationContext application)
        {
        }

        public ObservableCollection<IRibbonTab> GenerateTabs(ObservableCollection<IRibbonMetadata> metadatas)
        {
            var tabs = new ObservableCollection<IRibbonTab>();

            var ribbonGroup = new RibbonGroup(0, "Modules",
                new ObservableCollection<RibbonButton>(
                    metadatas.Select(
                        s =>
                            new RibbonButton
                            {
                                Command = MainWindowViewModel.InitModuleCommand,
                                CommandParameter = s,
                                Content = s.Descriptor,
                                Label = s.Descriptor
                            })));
            var ribbonTab = new RibbonTab(0, "Modules", ribbonGroup);
            tabs.Add(ribbonTab);
            return tabs;
        }
    }
}