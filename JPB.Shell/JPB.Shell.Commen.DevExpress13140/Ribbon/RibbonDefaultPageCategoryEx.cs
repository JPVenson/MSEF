#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 15:33

#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Ribbon;
using IEADPC.Shell.Commen.DevExpress13140.Service.App;
using IEADPC.Shell.Commen.DevExpress13140.Service.Shell.VisualModule;
using IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ContainerService;
using IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ModuleServices;
using IEADPC.Shell.Contracts.Interfaces.Metadata;

namespace IEADPC.Shell.Commen.DevExpress13140.Ribbon
{
    public class RibbonDefaultPageCategoryEx : RibbonDefaultPageCategory
    {
        public static readonly DependencyProperty ModulesProperty =
            DependencyProperty.Register("Modules",
                                        typeof (ObservableCollection<IVisualServiceMetadata>),
                                        typeof (RibbonDefaultPageCategoryEx),
                                        new FrameworkPropertyMetadata(
                                            new ObservableCollection<IVisualServiceMetadata>(),
                                            OnModulesPropertyChanged,
                                            OnCoerceModulesProperty),
                                        OnValidateModulesProperty);

        public static readonly DependencyProperty SelectedModuleProperty =
            DependencyProperty.Register("SelectedModule",
                                        typeof (ModuleLayoutDescriptor),
                                        typeof (RibbonDefaultPageCategoryEx),
                                        new FrameworkPropertyMetadata(
                                            null,
                                            OnModulePropertyChanged,
                                            OnCoerceModuleProperty),
                                        OnValidateModuleProperty);

        public ModuleLayoutDescriptor SelectedModule
        {
            get { return (ModuleLayoutDescriptor) GetValue(SelectedModuleProperty); }
            set
            {
                SetValue(SelectedModuleProperty, value);
                OnCommandsChanged();
            }
        }

        public ObservableCollection<IVisualServiceMetadata> Modules
        {
            get { return (ObservableCollection<IVisualServiceMetadata>) GetValue(ModulesProperty); }
            set { SetValue(ModulesProperty, value); }
        }

        private static bool OnValidateModuleProperty(object value)
        {
            return value is ModuleLayoutDescriptor || value == null;
        }

        private static object OnCoerceModuleProperty(DependencyObject d, object basevalue)
        {
            return basevalue;
        }

        private static void OnModulePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = d as RibbonDefaultPageCategoryEx;
            if (null == @this) return;

            var modules = e.NewValue as ModuleLayoutDescriptor;
            if (null == modules) return;

            @this.SelectedModule = modules;
        }

        public void OnCommandsChanged()
        {
            var ribbonservice =
                VisualMainWindow.ApplicationProxy.VisualModuleManager.CreateService<IRibbonModuleAreaProvider>(
                    SelectedModule.VisualServieMetadata.Descriptor);

            if (ribbonservice == null)
                return;

            RibbonPage page = Pages.FirstOrDefault(p => p.Caption.ToString() == SelectedModule.Page /*Page.Name*/);
            if (page == null)
                return;

            Ribbon.ToolbarItemLinks.Clear();

            List<RibbonPageGroup> groups = page.Groups.Skip(1).ToList();

            foreach (RibbonPageGroup ribbonPageGroup in groups)
                page.Groups.Remove(ribbonPageGroup);

            foreach (ICustomRibbonGroup cusomeCustomeUserAreaVisualService in ribbonservice.RibbonElements)
            {
                var group = new RibbonPageGroup {Caption = cusomeCustomeUserAreaVisualService.Name};
                page.Groups.Add(group);
                group.ItemLinks.Clear();
                foreach (BarItem ribbonModuleAreaProvider in cusomeCustomeUserAreaVisualService.RibbonElements)
                {
                    if (ribbonModuleAreaProvider is BarButtonItem)
                    {
                        var itemlink = new BarButtonItemLink
                            {
                                BarItemName = ribbonModuleAreaProvider.Name,
                                RibbonStyle = RibbonItemStyles.SmallWithText
                            };
                        //base.Ribbon.ToolbarItemLinks.Add(itemlink);
                    }

                    group.ItemLinks.Add(ribbonModuleAreaProvider);
                }
            }

            if (ribbonservice.RibbonElements == null)
                return;

            ribbonservice.RibbonElementsChanged -= OnCommandsOnCollectionChanged;
            ribbonservice.RibbonElementsChanged += OnCommandsOnCollectionChanged;
        }

        private void OnCommandsOnCollectionChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            OnCommandsChanged();
        }

        private void OnModulesChanged(IEnumerable<IVisualServiceMetadata> modules)
        {
            var moduleSorterService =
                VisualMainWindow.ApplicationProxy.ServicePool.GetSingelService<IModuleSorterService>();
            List<ModuleLayoutDescriptor> arrModules =
                modules.Select(moduleSorterService.GenerateDescriptor)
                       .Except(new List<ModuleLayoutDescriptor> {null})
                       .ToList();

            var barManager = Parent as RibbonControl;
            if (null == barManager) return;

            Pages.Clear();

            foreach (var mod in arrModules)
            {
                mod.OnVisibilityChangedDelegate = OnVisibilityChanged;

                RibbonPage page = Pages.FirstOrDefault(p => p.Caption.ToString() == mod.Page /*Page.Name*/);
                if (null == page)
                {
                    page = new RibbonPage {Caption = mod.Page};
                    Pages.Add(page);
                }

                RibbonPageGroup group = page.Groups.FirstOrDefault(g => g.Caption == mod.Group);
                if (null == group)
                {
                    group = new RibbonPageGroup {Caption = mod.Group};
                    page.Groups.Add(group);
                }

                string name = mod.Label.Replace(" ", "_");

                var bbi = new BarButtonItem();
                bbi.SetBinding(BarItem.CommandProperty, new Binding("BarButtonItemCommand"));
                bbi.SetValue(BarItem.CommandParameterProperty, mod);
                bbi.Name = name;
                bbi.Tag = mod;
                bbi.IsEnabled = true; // mod.Enabled;
                bbi.IsVisible = mod.Visible;
                if (mod.LargeGlyph != null)
                {
                    bbi.RibbonStyle = RibbonItemStyles.Large;
                    bbi.LargeGlyph = mod.LargeGlyph;
                }
                bbi.Content = mod.Label;
                group.ItemLinks.Add(bbi);
            }

            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            foreach (RibbonPage page in Pages)
            {
                foreach (RibbonPageGroup group in page.Groups)
                    group.IsVisible = group.ItemLinks.Cast<BarButtonItemLink>().Any(x => x.IsVisible);
                page.IsVisible = page.Groups.Any(x => x.IsVisible);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="mld"></param>
        /// <param name="bVisible"></param>
        private void OnVisibilityChanged(IModuleLayoutDescriptor mld, bool bVisible)
        {
            var ribbonControl = Parent as RibbonControl;
            if (null == ribbonControl) return;

            var dockPanel = ribbonControl.Parent as DockPanel;
            if (null == dockPanel) return;

            var barManager = dockPanel.Parent as BarManager;
            if (null == barManager) return;

            BarItem bbi = barManager.Items.FirstOrDefault(i => i.Tag == mld);
            if (bbi != null)
                bbi.IsVisible = bVisible;

            bool bDone = false;
            foreach (RibbonPage page in Pages)
            {
                foreach (RibbonPageGroup group in page.Groups)
                {
                    foreach (
                        BarButtonItemLink itemLink in
                            @group.ItemLinks.Cast<BarButtonItemLink>().Where(itemLink => itemLink.Tag == mld))
                    {
                        itemLink.IsVisible = bVisible;
                        bDone = true;
                        break;
                    }
                    group.IsVisible = group.ItemLinks.Cast<BarButtonItemLink>().Any(x => x.IsVisible);
                    if (bDone) break;
                }
                page.IsVisible = page.Groups.Any(x => x.IsVisible);
                if (bDone) break;
            }
        }

        private static void OnModulesPropertyChanged(DependencyObject source,
                                                     DependencyPropertyChangedEventArgs e)
        {
            var @this = source as RibbonDefaultPageCategoryEx;
            if (null == @this) return;

            var modules = e.NewValue as ObservableCollection<IVisualServiceMetadata>;
            if (null == modules) return;

            @this.OnModulesChanged(modules);
        }

        private static object OnCoerceModulesProperty(DependencyObject sender, object data)
        {
            return data ?? new ObservableCollection<IVisualServiceMetadata>();
        }

        private static bool OnValidateModulesProperty(object data)
        {
            return data is ObservableCollection<IVisualServiceMetadata>;
        }
    }
}