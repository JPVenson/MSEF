#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 18:38
#endregion

using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition.Hosting;
using JPB.Shell.VisualServiceScheduler.Model;

namespace JPB.Shell.VisualServiceScheduler.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Action(null, null);

            Module.Context.ServicePool.RegisterExportsChanging(Action);
            RemoveAssamblyCommand = new DelegateCommand(RemoveAssambly, CanRemoveAssambly);
        }

        private void Action(object sender, ExportsChangeEventArgs exportsChangeEventArgs)
        {
            VisualServiceMetadatas = new ObservableCollection<MetadataEx>(Module.Context.ServicePool.GetMetadatas<IVisualServiceMetadata>().Select(s => new MetadataEx(s)));
            ServiceMetadatas = new ObservableCollection<MetadataEx>(Module.Context.ServicePool.GetAllMetadata().Select(s => new MetadataEx(s)).Except(VisualServiceMetadatas));
        }

        #region VisualServiceMetadatas property

        private ObservableCollection<MetadataEx> _visualServiceMetadatas = default(ObservableCollection<MetadataEx>);

        public ObservableCollection<MetadataEx> VisualServiceMetadatas
        {
            get { return _visualServiceMetadatas; }
            set
            {
                _visualServiceMetadatas = value;
                SendPropertyChanged(() => VisualServiceMetadatas);
            }
        }

        #endregion


        #region ImportLogEx property

        private ImportLogEx _importLogEx = new ImportLogEx();

        public ImportLogEx ImportLogEx
        {
            get { return _importLogEx; }
            set
            {
                _importLogEx = value;
                SendPropertyChanged(() => ImportLogEx);
            }
        }

        #endregion




        #region ShowAllServices property

        private bool _showAllServices = default(bool);

        public bool ShowAllServices
        {
            get { return _showAllServices; }
            set
            {
                _showAllServices = value;
                //ServiceMetadatas = ShowAllServices ? Module.Context.ServicePool.GetAllMetadata().ToList() : Module.Context.ServicePool.GetMetadata().ToList();
                SendPropertyChanged(() => ShowAllServices);
            }
        }

        #endregion

        #region RemoveAssambly DelegateCommand

        public DelegateCommand RemoveAssamblyCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        private void RemoveAssambly(object sender)
        {
            var serviceMetadata = SelectedMetadata.Metadata;
            var serice = ServicePool.Instance.GetSingelService<IService>(serviceMetadata);

            if (serice == null)
                return;

            Action<IApplicationContext> lambdaFunc = serice.OnStart;
            Delegate del = lambdaFunc;
            var assam = del.Method.ReflectedType.Assembly;
            ServicePool.Instance.FreeAssambly(assam);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        /// <returns>True if you can use it otherwise false</returns>
        private bool CanRemoveAssambly(object sender)
        {
            return SelectedMetadata != null;
        }

        #endregion

        #region SelectedMetadata property

        private MetadataEx _selectedMetadata = default(MetadataEx);

        public MetadataEx SelectedMetadata
        {
            get { return _selectedMetadata; }
            set
            {
                _selectedMetadata = value;
                if (value != null && value.Metadata != null)
                    SelectedMetadata.EnumerateServices(value.Metadata);
                SendPropertyChanged(() => SelectedMetadata);
            }
        }

        #endregion

        #region ServiceMetadatas property

        private ObservableCollection<MetadataEx> _serviceMetadatas = default(ObservableCollection<MetadataEx>);

        public ObservableCollection<MetadataEx> ServiceMetadatas
        {
            get { return _serviceMetadatas; }
            set
            {
                _serviceMetadatas = value;
                SendPropertyChanged(() => ServiceMetadatas);
            }
        }

        #endregion
    }
}