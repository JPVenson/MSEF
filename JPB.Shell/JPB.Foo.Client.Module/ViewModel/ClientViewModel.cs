using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JPB.Foo.ClientCommenContracts.Service;

namespace JPB.Foo.Client.Module.ViewModel
{
    public class ClientViewModel
    {
        public ClientViewModel()
        {
            var fooDatabaseService = Module.Context.ServicePool.GetSingelService<IFooDatabaseService>();
            
            Console.WriteLine(fooDatabaseService);
        }
    }
}
