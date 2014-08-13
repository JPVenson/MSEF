using JPB.Shell.Contracts.Interfaces.Services;

namespace JPB.Foo.ClientCommenContracts.Service
{
    public interface IFooDatabaseService : IService
    {
        //CRUD
        void Insert<T>(T entity);
        T Select<T>(string where);
        void Update<T>(T entity);
        bool Delete<T>(T entity);
    }
}