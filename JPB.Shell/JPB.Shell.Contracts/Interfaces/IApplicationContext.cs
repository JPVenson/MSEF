#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 11:42

#endregion

using JPB.Shell.Contracts.Interfaces.Services.ModuleServices;
using JPB.Shell.Contracts.Interfaces.Services.ShellServices;
using JPB.Shell.Contracts.Interfaces.Services.ShellServices.Logging;

namespace JPB.Shell.Contracts.Interfaces
{
    public interface IApplicationContext
    {
        IDataBroker DataBroker { get; }
        IServicePool ServicePool { get; }
        IMessageBroker MessageBroker { get; }
        IImportPool ImportPool { get; }
        IVisualModuleManager VisualModuleManager { get; }
    }
}