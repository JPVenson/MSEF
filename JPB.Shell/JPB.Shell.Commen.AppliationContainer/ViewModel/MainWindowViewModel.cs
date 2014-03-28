#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 16:56
#endregion

using System.Collections.ObjectModel;
using JPB.Shell.CommonAppliationContainer.Command;
using JPB.Shell.CommonAppliationContainer.Services.Shell.VisualModule;
using JPB.Shell.CommonContracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services.ModuleServices;

namespace JPB.Shell.CommonAppliationContainer.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            InitModuleCommand = new DelegateCommand(InitModule, CanInitModule);
            VisualServieMetadatas = new ObservableCollection<IRibbonMetadata>(VisualMainWindow.ApplicationProxy.ServicePool.GetMetadatas<IRibbonMetadata>());
        }

        #region VisualServieMetadatas property

        private ObservableCollection<IRibbonMetadata> _visualServieMetadatas = default(ObservableCollection<IRibbonMetadata>);

        public ObservableCollection<IRibbonMetadata> VisualServieMetadatas
        {
            get { return _visualServieMetadatas; }
            set
            {
                _visualServieMetadatas = value;
                SendPropertyChanged(() => VisualServieMetadatas);
            }
        }

        #endregion

        #region SelectedIVisualServieMetadata property

        private IVisualService _selectedIVisualIVisualModule = default(IVisualService);

        public IVisualService SelectedVisualIVisualModule
        {
            get { return _selectedIVisualIVisualModule; }
            set
            {
                _selectedIVisualIVisualModule = value;
                SendPropertyChanged(() => SelectedVisualIVisualModule);
            }
        }

        #endregion

        #region InitModule DelegateCommand

        public static DelegateCommand InitModuleCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        private void InitModule(object sender)
        {
            var send = sender as IVisualServiceMetadata;
            var module = VisualMainWindow.ApplicationProxy.VisualModuleManager.CreateService<IVisualService>(send.Descriptor);

            if (SelectedVisualIVisualModule != null && !SelectedVisualIVisualModule.OnLeave())
                return;
            if (module.OnEnter())
            {
                this.SelectedVisualIVisualModule = module;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        /// <returns>True if you can use it otherwise false</returns>
        private bool CanInitModule(object sender)
        {
            return sender is IVisualServiceMetadata;
        }

        #endregion
    }
}