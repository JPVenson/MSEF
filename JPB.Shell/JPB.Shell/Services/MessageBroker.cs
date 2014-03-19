#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 10:28
#endregion

using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using IEADPC.Shell.Contracts.Interfaces.Services.ShellServices;

namespace IEADPC.Shell.Services
{
    public class MessageBroker : IMessageBroker
    {
        private static MessageBroker _instance;
        private static readonly object LockObject = new object();
        private static Dictionary<Type, List<Delegate>> _receivers;

        private MessageBroker()
        {
            _receivers = new Dictionary<Type, List<Delegate>>();
        }
        /// <summary>
        /// The Current MassageBroker Instance
        /// </summary>
        public static MessageBroker Instance
        {
            get
            {
                lock (LockObject)
                {
                    return _instance ?? (_instance = new MessageBroker());
                }
            }
        }
        /// <summary>
        /// Notify all Receiver that <typeparamref name="T"/> has been changed
        /// </summary>
        /// <typeparam name="T">Your targed type</typeparam>
        /// <param name="message">What your Receiver will get</param>
        public void Publish<T>(T message)
        {
            List<Delegate> list = null;
            if (_receivers.ContainsKey(typeof(T)))
            {
                list = _receivers[typeof(T)];
                foreach (var receiver in list)
                {
                    receiver.DynamicInvoke(message);
                }
            }
        }

        /// <summary>
        /// Add a Receiver to the <see cref="MessageBroker"/>
        /// </summary>
        /// <typeparam name="T">Your targed type</typeparam>
        /// <param name="callback">A Callback that will used for a Notify if anyone will call <see cref="MessageBroker.Publish{T}"/> /></param>
        public void AddReceiver<T>(Action<T> callback)
        {
            List<Delegate> list = null;
            if (_receivers.ContainsKey(typeof(T)))
            {
                list = _receivers[typeof(T)];
            }
            else
            {
                list = new List<Delegate>();
                _receivers.Add(typeof(T), list);
            }
            list.Add(callback);
        } 
    }
}