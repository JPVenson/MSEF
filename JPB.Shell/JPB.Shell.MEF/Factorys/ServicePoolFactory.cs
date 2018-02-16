using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JPB.Shell.Contracts.Interfaces.Services.ShellServices;
using JPB.Shell.MEF.Log;
using JPB.Shell.MEF.Services;

namespace JPB.Shell.MEF.Factorys
{
    public class ServicePoolFactory
    {
        public static IServicePool CreatePool(string priorityKey = null)
        {
            return CreatePool(priorityKey, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        public static IServicePool CreatePool(string priorityKey, params string[] sublookuppaths)
        {
            var pool = new ServicePool(priorityKey, sublookuppaths);
            ServicePool.Instance = pool;
            if (ServicePool.ApplicationContainer == null)
                ServicePool.ApplicationContainer = new ApplicationContext(ImportPool.Instance, MessageBroker.Instance, pool, null, VisualModuleManager.Instance);
            pool.InitLoading();
            return pool;
        }
    }
}
