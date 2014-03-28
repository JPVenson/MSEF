#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 10:56

#endregion

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using JPB.DynamicInputBox.InfoWindow.Wrapper;
using JPB.ErrorValidation.ValidationTyps;
using JPB.WPFBase.MVVM.DelegateCommand;

namespace JPB.DynamicInputBox.InfoWindow.IQuestionModelImp
{
    public class QuestionActionViewModel : QuestionViewModel
    {
        public QuestionActionViewModel(object question, EingabeModus eingabeModus)
            : base(question, eingabeModus)
        {
            base.ErrorProviderSimpleAccessAdapter.Add(
                new Error<QuestionViewModel>("Bitte warten bis die Action abgelaufen ist", "Input", s => IsRuning));
            RunActionCommand = new DelegateCommand(RunAction, CanRunAction);

            if (!(Question is IWaiterWrapper))
                if (!(Question is Func<object>))
                {
                    throw new ArgumentException("can not parse parameter");
                }
                else
                {
                    Question = PreEncapsulateAction(Question as Func<object>);
                }
        }

        #region RunAction DelegateCommand

        public DelegateCommand RunActionCommand { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        private void RunAction(object sender)
        {
            ProgressWaiter(Question as IWaiterWrapper);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        /// <returns>True if you can use it otherwise false</returns>
        private bool CanRunAction(object sender)
        {
            return true;
        }

        #endregion

        private IWaiterWrapper PreEncapsulateAction(Func<object> action)
        {
            var input = new WaiterWrapperImpl(action, "Arbeite . . .") {MaxProgress = 0};
            return input;
        }

        private void ProgressWaiter(IWaiterWrapper waiter)
        {
            IsRuning = true;
            base.ForceRefresh();

            if (waiter.IsAsnc)
            {
                var task = new Task<object>(() => waiter.WorkerFunction.Invoke());
                task.Start();
                task.ContinueWith(s =>
                    {
                        Input = s.Result;
                        IsRuning = false;
                        base.ForceRefresh();
                    });
            }
            else
            {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        var task = new Task<object>(() => waiter.WorkerFunction.Invoke());
                        task.Start();
                        task.ContinueWith(s =>
                            {
                                Input = s.Result;
                                IsRuning = false;
                                base.ForceRefresh();
                            });
                    }));
            }
        }

        #region IsRuning property

        private bool _isRuning = default(bool);

        public bool IsRuning
        {
            get { return _isRuning; }
            set
            {
                _isRuning = value;
                SendPropertyChanged(() => IsRuning);
            }
        }

        #endregion
    }
}