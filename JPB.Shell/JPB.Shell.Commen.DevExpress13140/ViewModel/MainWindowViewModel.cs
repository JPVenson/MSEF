#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 15:42

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DevExpress.Xpf.Core;
using IEADPC.Shell.Commen.DevExpress13140.MVVM.DelegateCommand;
using IEADPC.Shell.Commen.DevExpress13140.MVVM.ViewModel;
using IEADPC.Shell.Commen.DevExpress13140.Model;
using IEADPC.Shell.Commen.DevExpress13140.Ribbon;
using IEADPC.Shell.Commen.DevExpress13140.Service.App;
using IEADPC.Shell.Commen.DevExpress13140.Service.Shell.VisualModule;
using IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ContainerService;
using IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ModuleServices;
using IEADPC.Shell.Contracts.Attributes;
using IEADPC.Shell.Contracts.Interfaces;
using IEADPC.Shell.Contracts.Interfaces.Metadata;
using IEADPC.Shell.Contracts.Interfaces.Services.ShellServices.Logging;

namespace IEADPC.Shell.Commen.DevExpress13140.ViewModel
{
    [ServiceExport("ApplicationViewControler", true, typeof(IRemoteApplicationViewService))]
    public class MainWindowViewModel : ViewModelBase, IRemoteApplicationViewService
    {
        public MainWindowViewModel()
        {
            OnLoading = false;
            BarButtonItemCommand = new DelegateCommand(BarButtonItem, CanBarButtonItem);
            InfoTrayButtonCommand = new DelegateCommand(InfoTrayButton, CanInfoTrayButton);
            try
            {
                VisualServieMetadatas =
                    new ObservableCollection<IVisualServiceMetadata>(
                        VisualMainWindow.ApplicationProxy.VisualModuleManager.GetVisualServicesMetadata());
                VisualMainWindow.ApplicationProxy.ServicePool.RegisterExportsChanging(ExportsChangeing);
                VisualMainWindow.ApplicationProxy.ImportPool.PropertyChanged += ImportPoolOnPropertyChanged;
                var appmenu = VisualMainWindow.ApplicationProxy.ServicePool.GetSingelService<IRibbonAppMenu>();
                if (appmenu != null)
                {
                    RibbonAppMenu = new RibbonAppMenuManager(appmenu);
                    IsAppButtonImpl = true;
                }
                TitleParts = new ObservableCollection<string>();
                TitleParts.CollectionChanged += TitlePartsOnCollectionChanged;
                TitleParts.Add("IEA DPC Shell");
                if (string.IsNullOrEmpty(SelectedTheme))
                    SelectedTheme = "Office2010Black";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        private void TitlePartsOnCollectionChanged(object sender,
                                                   NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            Title = TitleParts.Aggregate((s, e) => s + " | " + e);
        }

        private void ImportPoolOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "LogEntries")
                return;

            var infoservice = VisualMainWindow.ApplicationProxy.ServicePool.GetSingelService<IInfoTrayProvider>();
            if (infoservice == null)
                return;

            var logEntry = VisualMainWindow.ApplicationProxy.ImportPool.LogEntries.Last();

            infoservice.DisplayMessage(logEntry.Messages.ElementAt(1).ToString(),
                                       () => System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                                       {
                                           var window = new System.Windows.Window
                                               {
                                                   Owner = System.Windows.Application.Current.MainWindow,
                                                   Title = "info",
                                                   SizeToContent = SizeToContent.WidthAndHeight,
                                                   Content = logEntry.ToString()
                                               };
                                           window.ShowDialog();
                                       })));
        }

        private void ExportsChangeing(object sender, ExportsChangeEventArgs exportsChangeEventArgs)
        {
            VisualServieMetadatas =
                new ObservableCollection<IVisualServiceMetadata>(VisualMainWindow.ApplicationProxy.VisualModuleManager.GetVisualServicesMetadata().OrderBy(s => s.Descriptor));
        }

        public void OnLeave()
        {
            if (SelectedModule != null)
                SelectedModule.OnLeave();
        }

        #region RibbonAppMenu property

        private RibbonAppMenuManager _ribbonAppMenu = default(RibbonAppMenuManager);

        public RibbonAppMenuManager RibbonAppMenu
        {
            get { return _ribbonAppMenu; }
            set
            {
                _ribbonAppMenu = value;
                SendPropertyChanged(() => RibbonAppMenu);
            }
        }

        #endregion

        #region IsAppButtonImpl property

        private bool _isAppButtonImpl = default(bool);

        public bool IsAppButtonImpl
        {
            get { return _isAppButtonImpl; }
            set
            {
                _isAppButtonImpl = value;
                SendPropertyChanged(() => IsAppButtonImpl);
            }
        }

        #endregion

        #region VisualServieMetadatas property

        private ObservableCollection<IVisualServiceMetadata> _visualServieMetadatas =
            default(ObservableCollection<IVisualServiceMetadata>);

        public ObservableCollection<IVisualServiceMetadata> VisualServieMetadatas
        {
            get { return _visualServieMetadatas; }
            set
            {
                _visualServieMetadatas = value;
                SendPropertyChanged(() => VisualServieMetadatas);
            }
        }

        #endregion

        #region BarButtonItem DelegateCommand

        #region SelectedModuleLayoutDescriptor property

        private ModuleLayoutDescriptor _selectedModuleLayoutDescriptor = default(ModuleLayoutDescriptor);

        public ModuleLayoutDescriptor SelectedModuleLayoutDescriptor
        {
            get { return _selectedModuleLayoutDescriptor; }
            set
            {
                _selectedModuleLayoutDescriptor = value;
                SendPropertyChanged(() => SelectedModuleLayoutDescriptor);
            }
        }

        #endregion

        public DelegateCommand BarButtonItemCommand { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        private void BarButtonItem(object sender)
        {
            var mld = sender as ModuleLayoutDescriptor;
            if (null == mld)
                return;

            OnLoading = true;
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                if (SelectedModule != null && !SelectedModule.OnLeave())
                {
                    OnLoading = false;
                    return;
                }
                try
                {
                    var instance =
                        VisualMainWindow.ApplicationProxy.VisualModuleManager
                                        .CreateService<IRibbonModuleAreaProvider>(
                                            mld.VisualServieMetadata.Descriptor);
                    if (instance.OnEnter())
                    {
                        if (SelectedModuleLayoutDescriptor != null)
                            TitleParts.Remove("Module: " + SelectedModuleLayoutDescriptor.Label);
                        TitleParts.Add("Module: " + mld.Label);
                        SelectedModuleLayoutDescriptor = mld;
                        SelectedModule = instance;
                    }
                }
                catch (Exception)
                {
                    mld.Visible = false;
                }

                CommandManager.InvalidateRequerySuggested();

                OnLoading = false;
            }));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        /// <returns>True if you can use it otherwise false</returns>
        private bool CanBarButtonItem(object sender)
        {
            return SelectedModuleLayoutDescriptor != sender;
        }

        #endregion

        #region SelectedModule property

        private IRibbonModuleAreaProvider _selectedModule = default(IRibbonModuleAreaProvider);

        public IRibbonModuleAreaProvider SelectedModule
        {
            get { return _selectedModule; }
            set
            {
                _selectedModule = value;
                SendPropertyChanged(() => SelectedModule);
            }
        }

        public void SetModule(string module)
        {
            var moduleSorterService =
             VisualMainWindow.ApplicationProxy.ServicePool.GetSingelService<IModuleSorterService>();
            var moduleLayoutDescriptor = moduleSorterService.GenerateDescriptor(Application.VisualModuleManager.GetVisualServicesMetadata().FirstOrDefault(s => s.Descriptor == module));
            BarButtonItem(moduleLayoutDescriptor);
        }

        public ObservableCollection<string> TitleParts { get; set; }

        #endregion

        #region Title property

        private string _title = default(string);

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => SendPropertyChanged(() => Title)));
            }
        }

        #endregion

        #region OnLoading property

        private string _message;
        private bool _onLoading = default(bool);

        public bool OnLoading
        {
            get { return _onLoading; }
            set
            {
                _onLoading = !value;
                SendPropertyChanged(() => OnLoading);
            }
        }

        #endregion

        #region Message

        public string Message
        {
            get
            {
                if (ProgressInfo != null)
                    return ProgressInfo.ProgressDescriptor + " : " + ProgressInfo.Progress + "%";
                return _message;
            }
            set
            {
                _message = value.Split('\n')[0];
                SendPropertyChanged(() => Message);
            }
        }

        #endregion

        #region Implementation of IInfoTrayProvider

        public IProgressInfo ProgressInfo { get; set; }

        public void DisplayMessage(string message)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => Message = message));
        }

        public IProgressInfo DisplayProgress()
        {
            ProgressInfo =
                new ProgressInfo(
                    () => System.Windows.Application.Current.Dispatcher.Invoke(new Action(() => SendPropertyChanged(() => Message))),
                    () => ProgressInfo = null);
            return ProgressInfo;
        }

        #endregion

        #region InfoTrayButton DelegateCommand

        public DelegateCommand InfoTrayButtonCommand { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        private void InfoTrayButton(object sender)
        {
            InfoTrayButtonAction();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        /// <returns>True if you can use it otherwise false</returns>
        private bool CanInfoTrayButton(object sender)
        {
            return InfoTrayButtonAction != null;
        }

        #endregion

        #region InfoTrayButton

        public Action InfoTrayButtonAction { get; set; }

        #endregion

        #region SelectedTheme property

        public string SelectedTheme
        {
            get { return ThemeManager.ApplicationThemeName; }
            set
            {
                ThemeManager.ApplicationThemeName = value;
                SendPropertyChanged(() => SelectedTheme);
            }
        }

        #endregion

        #region Themes property

        public IEnumerable<string> Themes
        {
            get { return Theme.Themes.Select(s => s.Name); }
        }

        #endregion

        #region Implementation of IService

        public static IApplicationContext Application;

        public void OnStart(IApplicationContext application)
        {
            Application = application;
        }

        #endregion
    }
}