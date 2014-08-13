using System;
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