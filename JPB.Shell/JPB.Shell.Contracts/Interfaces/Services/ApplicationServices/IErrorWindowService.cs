#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 15:56

#endregion

using System;

namespace JPB.Shell.Contracts.Interfaces.Services.ApplicationServices
{
    public interface IErrorWindowService : IService
    {
        bool OnErrorRecived(Exception ex);
    }
}