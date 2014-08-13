#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 11:59

#endregion

using JPB.Shell.Contracts.Interfaces.Services;
using JPB.WPFBase.MVVM.ViewModel;

namespace JPB.Shell.VisualServiceScheduler.Model
{
    public class ServiceEx : AsyncViewModelBase
    {
        private IService _service;

        public ServiceEx(IService service)
        {
            Service = service;
        }

        public IService Service
        {
            get { return _service; }
            set
            {
                if (Service != null)
                    ReflectService(Service);
                _service = value;
                SendPropertyChanged(() => Service);
            }
        }

        #region TypeReflection property

        private TypeReflection _typeReflection = default(TypeReflection);

        public TypeReflection TypeReflection
        {
            get { return _typeReflection; }
            set
            {
                _typeReflection = value;
                SendPropertyChanged(() => TypeReflection);
            }
        }

        #endregion

        #region ReflectService DelegateCommand

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        public void ReflectService(IService sender)
        {
            if (!IsWorking)
                base.SimpleWork(() => new TypeReflection(sender),
                s =>
                {
                    TypeReflection = s;
                });
        }

        #endregion
    }
}