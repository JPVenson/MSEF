#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 16:56
#endregion

using System.Collections.Generic;
using System.Linq;
using JPB.Shell.CommenAppliationContainer.Command;
using JPB.Shell.Contracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services.ModuleServices;
using JPB.Shell.MEF.Services;

namespace JPB.Shell.CommenAppliationContainer.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            InitModuleCommand = new DelegateCommand(InitModule, CanInitModule);

            VisualServieMetadatas = VisualModuleManager.Instance.GetVisualServicesMetadata().ToList();
        }

        #region VisualServieMetadatas property

        private List<IVisualServiceMetadata> _visualServieMetadatas = default(List<IVisualServiceMetadata>);

        public List<IVisualServiceMetadata> VisualServieMetadatas
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

        public DelegateCommand InitModuleCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        private void InitModule(object sender)
        {
            var send = sender as IVisualServiceMetadata;
            this.SelectedVisualIVisualModule = VisualModuleManager.Instance.CreateService<IVisualService>(send.Descriptor);
            SelectedVisualIVisualModule.OnEnter();
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