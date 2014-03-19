#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 12:47

#endregion

using JPB.Shell.Contracts.Interfaces.Services.ModuleServices;

namespace JPB.Shell.Contracts.Interfaces.Services.ApplicationServices
{
    public interface IApplicationContainer : IVisualService
    {
        IApplicationContext ApplicationContextProxy { get; }
    }
}