#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 12:05

#endregion

namespace JPB.Shell.Contracts.Interfaces.Services.ModuleServices
{
    /// <summary>
    /// Defines a Service that is able to start an Application
    /// Will be called when the Enumeration inside the ServicePool is done
    /// </summary>
    public interface IApplicationProvider : IService
    {
        IApplicationContext Context { get; set; }
    }
}