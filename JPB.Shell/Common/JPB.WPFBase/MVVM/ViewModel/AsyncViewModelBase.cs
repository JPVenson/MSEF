using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace JPB.WPFBase.MVVM.ViewModel
{
    public abstract class AsyncViewModelBase : ViewModelBase
    {
        protected AsyncViewModelBase()
        {
            Dispatcher = Application.Current.Dispatcher;
        }

        protected AsyncViewModelBase(Dispatcher targetDispatcher)
        {
            Dispatcher = targetDispatcher;
        }

        #region IsWorking property

        private bool _isWorking = default(bool);

        public bool IsWorking
        {
            get { return _isWorking; }
            set
            {
                _isWorking = value;

                if (Dispatcher.CheckAccess())
                {
                    SendPropertyChanged(() => IsWorking);
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() => SendPropertyChanged(() => IsWorking)),
                                           DispatcherPriority.DataBind);
                }
            }
        }

        #endregion

        public Dispatcher Dispatcher { get; set; }

        private void StartWork()
        {
            IsWorking = true;
        }

        private void EndWork()
        {
            IsWorking = false;
        }

        public virtual bool OnTaskException(Exception exception)
        {
            return false;
        }

        public void SimpleWorkWithSyncContinue<T>(Func<T> delegatetask, Action<T> continueWith)
        {
            if (delegatetask != null)
            {
                var task = new Task<T>(delegatetask.Invoke);
                SimpleWork(task, new Action<Task<T>>(s => Dispatcher.Invoke(new Action(() => continueWith(s.Result)))));
            }
        }

        public void SimpleWork(Action delegatetask)
        {
            if (delegatetask != null)
            {
                var task = new Task(delegatetask.Invoke);
                SimpleWork(task);
            }
        }

        public void SimpleWork<T>(Func<T> delegatetask, Action<T> continueWith)
        {
            if (delegatetask != null)
            {
                var task = new Task<T>(delegatetask.Invoke);
                SimpleWork(task, new Action<Task<T>>(s => continueWith(s.Result)));
            }
        }

        public void SimpleWork(Delegate delegatetask, Delegate continueWith)
        {
            if (delegatetask != null)
            {
                var task = new Task(() => delegatetask.DynamicInvoke());
                SimpleWork(task, continueWith);
            }
        }

        public void SimpleWork(Delegate delegatetask)
        {
            SimpleWork(new Task(() => delegatetask.DynamicInvoke()));
        }

        public void SimpleWork(Task task, Delegate continueWith)
        {
            if (task != null)
            {
                StartWork();
                task.ContinueWith(s =>
                    {
                        try
                        {
                            if (s.IsFaulted)
                            {
                                if (s.Exception != null)
                                {
                                    s.Exception.Handle(OnTaskException);
                                }
                                else
                                {
                                    if (!OnTaskException(s.Exception))
                                    {
                                        return;
                                    }
                                }
                            }
                            if (continueWith != null)
                                continueWith.DynamicInvoke(s);
                        }
                        finally
                        {
                            EndWork();
                        }
                    });
                task.Start();
            }
        }

        public void SimpleWork(Task task)
        {
            if (task != null)
            {
                StartWork();
                task.ContinueWith(s => EndWork());
                task.Start();
            }
        }
    }
}