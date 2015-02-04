using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JPB.Shell.Contracts.Interfaces.Services;

namespace JPB.Shell.Example.Console.Contracts
{
    public interface IConsoleAction : IService
    {
        string ID { get; }
        bool HandleInput(string args);
    }
}
