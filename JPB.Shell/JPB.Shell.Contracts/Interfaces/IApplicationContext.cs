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
        IDataBroker DataBroker { get; set; }
        IServicePool ServicePool { get; set; }
        IMessageBroker MessageBroker { get; set; }
        IImportPool ImportPool { get; set; }
        IVisualModuleManager VisualModuleManager { get; set; }
    }
}