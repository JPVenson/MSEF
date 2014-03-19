#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 10:22
#endregion

using IEADPC.Shell.Contracts.Interfaces;
using IEADPC.Shell.Contracts.Interfaces.Services.ModuleServices;
using IEADPC.Shell.Contracts.Interfaces.Services.ShellServices;
using IEADPC.Shell.Contracts.Interfaces.Services.ShellServices.Logging;
using IEADPC.Shell.Log;

namespace IEADPC.Shell
{
    public class ApplicationContext : IApplicationContext
    {
        internal ApplicationContext(IImportPool importPool, IMessageBroker messageBroker, IServicePool servicePool, IDataBroker dataBroker, IVisualModuleManager visualModuleManager)
        {
            VisualModuleManager = visualModuleManager;
            DataBroker = dataBroker;
            ServicePool = servicePool;
            MessageBroker = messageBroker;
            ImportPool = importPool;
        }

        public static ApplicationContext Instance { get; set; }

        #region Implementation of IApplicationContext

        public IDataBroker DataBroker { get; private set; }

        public IServicePool ServicePool { get; private set; }

        public IMessageBroker MessageBroker { get; private set; }

        public IImportPool ImportPool { get; private set; }

        public IVisualModuleManager VisualModuleManager { get; private set; }

        #endregion
    }
}