#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 10:32

#endregion

using System;

namespace JPB.Shell.Contracts.Interfaces.Services.ShellServices
{
    public interface IMessageBroker
    {
        void Publish<T>(T message);
        void AddReceiver<T>(Action<T> callback);
    }
}