using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using JPB.Shell.CommonAppliationContainer.Services.Shell.VisualModule;
using JPB.Shell.CommonContracts.Interfaces.Metadata;
using JPB.Shell.CommonContracts.Interfaces.Services.Ribbon;
using Microsoft.Windows.Controls.Ribbon;

namespace JPB.Shell.CommonAppliationContainer.Ribbon
{
    public class RibbonEx : Microsoft.Windows.Controls.Ribbon.Ribbon
    {
        public RibbonEx()
        {
            
        }

        public static readonly DependencyProperty MetadatasProperty = DependencyProperty.Register(
            "Metadatas", typeof(ObservableCollection<IRibbonMetadata>), typeof(RibbonEx), new PropertyMetadata(default(ObservableCollection<IRibbonMetadata>), MetadatasChanged, MetadataCoreValidata));

        private static object MetadataCoreValidata(DependencyObject dependencyObject, object baseValue)
        {
            return baseValue;
        }

        private static void MetadatasChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var sender = dependencyObject as RibbonEx;
            sender.Metadatas.CollectionChanged += (s, e) => sender.OnMetadataChanged();
            sender.OnMetadataChanged();
        }

        public ObservableCollection<IRibbonMetadata> Metadatas
        {
            get { return (ObservableCollection<IRibbonMetadata>)GetValue(MetadatasProperty); }
            set { SetValue(MetadatasProperty, value); }
        }

        public void OnMetadataChanged()
        {
            Items.Clear();

            var ribbonProvider =
                VisualMainWindow.ApplicationProxy.ServicePool.GetSingelService<IRibbonContentProviderService>();

            if (ribbonProvider != null)
            {
                foreach (var generateTab in ribbonProvider.GenerateTabs(Metadatas).OrderBy(s => s.TabIndex))
                {
                    var tab = new RibbonTab();
                    tab.Header = generateTab.Header;
                    foreach (var ribbonGroup in generateTab.Groups.OrderBy(s => s.GroupIndex))
                    {
                        var group = new RibbonGroup();
                        group.Header = ribbonGroup.Header;
                        foreach (var button in ribbonGroup.Buttons)
                        {
                            group.Items.Add(button);
                        }
                        tab.Items.Add(group);
                    }
                    Items.Add(tab);
                }
            }
        }
    }
}