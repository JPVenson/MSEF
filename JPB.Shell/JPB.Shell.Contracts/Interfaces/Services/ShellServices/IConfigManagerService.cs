using System.Configuration;

namespace JPB.Shell.Contracts.Interfaces.Services.ShellServices
{
    public interface IConfigManagerService : IService
    {
        void Add(ConnectionStringSettings settings);
    }
}