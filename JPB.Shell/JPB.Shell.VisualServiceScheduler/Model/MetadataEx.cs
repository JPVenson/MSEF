#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 12:00
#endregion

using System;
using System.Collections.ObjectModel;

namespace JPB.Shell.VisualServiceScheduler.Model
{
    public class MetadataEx : ViewModelBase
    {
        public MetadataEx(IServiceMetadata metadata)
        {
            Metadata = metadata;
        }

        #region Metadata property

        private IServiceMetadata _metadata = default(IServiceMetadata);

        public IServiceMetadata Metadata
        {
            get { return _metadata; }
            set
            {
                _metadata = value;
                SendPropertyChanged(() => Metadata);
                SendPropertyChanged(() => MetadataDescriptor);
                SendPropertyChanged(() => MetadataContract);
                SendPropertyChanged(() => MetadataIsDefauldService);
            }
        }
        #region Adapter propertys

        public string MetadataDescriptor
        {
            get { return Metadata.Descriptor; }
        }

        public Type MetadataContract
        {
            get { return Metadata.Contract; }
        }

        public bool MetadataIsDefauldService
        {
            get { return Metadata.IsDefauldService; }
        }

        #endregion

        #endregion
        
        #region SelectedService property

        private ServiceEx _selectedService = default(ServiceEx);

        public ServiceEx SelectedService
        {
            get { return _selectedService; }
            set
            {
                _selectedService = value;
                if (value.Service != null)
                    value.ReflectService(value.Service);
                SendPropertyChanged(() => SelectedService);
            }
        }

        #endregion

        #region Service property

        private ObservableCollection<ServiceEx> _service = default(ObservableCollection<ServiceEx>);

        public ObservableCollection<ServiceEx> Services
        {
            get { return _service; }
            set
            {
                _service = value;
                SendPropertyChanged(() => Services);
            }
        }

        #endregion

        #region EnumerateServices DelegateCommand

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        public void EnumerateServices(object sender)
        {
            var enumerable = Shell.Services.ServicePool.Instance.GetServices<IService>(Metadata).Select(s => new ServiceEx(s));
            Services = new ObservableCollection<ServiceEx>(enumerable);

            //Services = new ObservableCollection<ServiceEx>(
            //    ManagedLivetimeServicePool.Instance.Services
            //                              .Where(s => s.Item2.Contract == SelectedMetadata.Contract)
            //                              .Select(s => new ServiceEx(s.Item1)));
        }

        #endregion
    }
}