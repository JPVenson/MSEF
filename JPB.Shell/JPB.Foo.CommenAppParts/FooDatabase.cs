using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JPB.Foo.ClientCommenContracts.Service;
using JPB.Shell.Contracts.Attributes;
using JPB.Shell.Contracts.Interfaces;

namespace JPB.Foo.CommenAppParts
{
    [ServiceExport("FooDatabase", false, typeof(IFooDatabaseService))]
    public class FooDatabase : IFooDatabaseService
    {
        public void OnStart(IApplicationContext application)
        {

        }

        public void Insert<T>(T entity)
        {

        }

        public T Select<T>(string @where)
        {
            return default(T);
        }

        public void Update<T>(T entity)
        {

        }

        public bool Delete<T>(T entity)
        {
            return true;
        }
    }
}
