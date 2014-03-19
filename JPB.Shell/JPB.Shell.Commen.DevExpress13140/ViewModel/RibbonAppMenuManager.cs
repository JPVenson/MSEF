using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using DevExpress.Utils;
using DevExpress.Xpf.Bars;
using IEADPC.Shell.Commen.DevExpress13140.MVVM.ViewModel;
using IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ContainerService;

namespace IEADPC.Shell.Commen.DevExpress13140.ViewModel
{
    public class RibbonAppMenuManager : ViewModelBase
    {
        public RibbonAppMenuManager()
        {
        }

        public RibbonAppMenuManager(IRibbonAppMenu menu)
        {
            Menu = menu;
            Menu.OnButtonClicked += MenuOnOnButtonClicked;
            Menu.LeftMenuPart.CollectionChanged += MenuOnRibbonElementsChanged;
            ApplicationMenu = new ApplicationMenuEx
                {
                    IgnoreMenuDropAlignment = DefaultBoolean.True,
                    ShowRightPane = true
                };
            ApplicationMenu.Closing += ApplicationMenuOnClosing;
            ApplicationMenu.RightPaneWidth = 500;
            ApplicationMenu.MinHeight = 300;
            ApplicationMenu.Opening += ApplicationMenuOnOpening;
            AddNewItems(Menu.LeftMenuPart);
            Menu.PropertyChanged += MenuOnPropertyChanged;
        }

        public IRibbonAppMenu Menu { get; set; }

        public bool OnButtonClicked { get; set; }

        public IRibbonAppMenuButton SelectedButton { get; set; }

        #region ApplicationMenu property

        private ApplicationMenuEx _applicationMenu = default(ApplicationMenuEx);

        public ApplicationMenuEx ApplicationMenu
        {
            get { return _applicationMenu; }
            set
            {
                _applicationMenu = value;
                SendPropertyChanged(() => ApplicationMenu);
            }
        }

        #endregion

        private void ApplicationMenuOnOpening(object sender, CancelEventArgs cancelEventArgs)
        {
            if (SelectedButton != null)
                ApplicationMenu.RightPane = (FrameworkElement) SelectedButton.View;
        }

        private void ApplicationMenuOnClosing(object sender,
                                              RibbonMenuCloseingEventArgs ribbonMenuCloseingEventArgs)
        {
            if (OnButtonClicked)
                ribbonMenuCloseingEventArgs.CancelClose = false;
        }

        private void MenuOnOnButtonClicked(object sender, EventArgs eventArgs)
        {
            OnButtonClicked = true;
            ApplicationMenu.IsOpen = true;
            if (sender != null)
                SelectedButton = ((IRibbonAppMenuButton) sender);
        }

        private void MenuOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == "RightMenuPart")
                ApplicationMenu.RightPane = (FrameworkElement) Menu.RightMenuPart.View;
            if (propertyChangedEventArgs.PropertyName == "BottomMenuPart")
                ApplicationMenu.BottomPane = (FrameworkElement) Menu.BottomMenuPart.View;
        }

        public void AddNewItems(IEnumerable<BarItem> items)
        {
            foreach (BarItem item in items)
                ApplicationMenu.ItemLinks.Add(item);
        }

        private void MenuOnRibbonElementsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            IEnumerable<BarItem> items = e.NewItems.Cast<BarItem>();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddNewItems(items);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (BarItem item in items)
                    {
                        foreach (
                            BarItemLinkBase link in item.Links.Where(link => ApplicationMenu.ItemLinks.Remove(link)))
                            break;
                    }
                    break;
            }
        }
    }
}