using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JPB.Tasking.TaskManagement.Threading
{
    public class SingelSeriellTaskFactory
    {
        private Thread _thread;

        private bool _working;

        public SingelSeriellTaskFactory()
        {
            ConcurrentQueue = new ConcurrentQueue<Tuple<Action, object>>();
        }

        public ConcurrentQueue<Tuple<Action, object>> ConcurrentQueue { get; set; }

        public void Add(Action action, object key)
        {
            if (key != null && ConcurrentQueue.Any(s => s.Item2 == key))
            {
                var task = new Task(() =>
                    {
                        Thread.Sleep(500);
                        if (ConcurrentQueue.Any(s => s.Item2 == key))
                            return;
                        ConcurrentQueue.Enqueue(new Tuple<Action, object>(action, key));
                        StartScheduler();
                    });
                task.Start();
                return;
            }

            ConcurrentQueue.Enqueue(new Tuple<Action, object>(action, key));
            StartScheduler();
        }

        private void StartScheduler()
        {
            if (_working)
                return;
            _working = true;
            _thread = new Thread(Worker);
            _thread.SetApartmentState(ApartmentState.MTA);
            _thread.Start();
        }

        internal void Worker()
        {
            while (ConcurrentQueue.Any())
            {
                Tuple<Action, object> action;
                if (ConcurrentQueue.TryDequeue(out action))
                {
                    action.Item1.Invoke();
                    action = null;
                }
            }
            _working = false;
        }
    }
}