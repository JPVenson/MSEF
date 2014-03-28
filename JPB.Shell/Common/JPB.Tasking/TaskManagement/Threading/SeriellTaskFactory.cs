#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 11:06

#endregion

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace JPB.Tasking.TaskManagement.Threading
{
    public class SeriellTaskFactory
    {
        private Thread _thread;

        private bool _working;

        public SeriellTaskFactory()
        {
            ConcurrentQueue = new ConcurrentQueue<Action>();
        }

        public ConcurrentQueue<Action> ConcurrentQueue { get; set; }

        public void Add(Action action)
        {
            ConcurrentQueue.Enqueue(action);
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
                Action action;
                if (ConcurrentQueue.TryDequeue(out action))
                {
                    action.Invoke();
                }
            }
            _working = false;
        }
    }
}