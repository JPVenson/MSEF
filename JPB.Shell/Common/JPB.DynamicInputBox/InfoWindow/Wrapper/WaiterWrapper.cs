#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 09:11

#endregion

using System;
using JPB.WPFBase.MVVM.ViewModel;

namespace JPB.DynamicInputBox.InfoWindow.Wrapper
{
    public abstract class WaiterWrapper : ViewModelBase, IWaiterWrapper
    {
        protected WaiterWrapper()
        {
            MaxProgress = 100;
            CurrentProgress = 0;
        }

        #region MaxProgress property

        private int _maxProgress = default(int);

        public int MaxProgress
        {
            get { return _maxProgress; }
            set
            {
                _maxProgress = value;
                SendPropertyChanged(() => MaxProgress);
            }
        }

        #endregion

        #region CurrentProgress property

        private int _currentProgress = default(int);

        public int CurrentProgress
        {
            get { return _currentProgress; }
            set
            {
                _currentProgress = value;
                SendPropertyChanged(() => CurrentProgress);
            }
        }

        #endregion

        #region IsAsnc property

        public bool IsAsnc
        {
            get { return true; }
            set { }
        }

        #endregion

        #region WorkerFunction property

        private Func<object> _workerFunction = default(Func<object>);

        public Func<object> WorkerFunction
        {
            get { return _workerFunction; }
            set
            {
                _workerFunction = value;
                SendPropertyChanged(() => WorkerFunction);
            }
        }

        #endregion

        #region WaiterText property

        private string _waiterText = default(string);

        public string WaiterText
        {
            get { return _waiterText; }
            set
            {
                _waiterText = value;
                SendPropertyChanged(() => WaiterText);
            }
        }

        #endregion

        public override string ToString()
        {
            return WaiterText;
        }
    }

    public class WaiterWrapperImpl : WaiterWrapper
    {
        public WaiterWrapperImpl(Func<object> workerFunction, string waiterText)
        {
            base.WaiterText = waiterText;
            base.WorkerFunction = workerFunction;
        }

        public override string ToString()
        {
            return WaiterText;
        }
    }
}