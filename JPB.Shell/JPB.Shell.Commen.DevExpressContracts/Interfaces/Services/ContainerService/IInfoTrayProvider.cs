using System;
using IEADPC.Shell.Contracts.Interfaces.Services;

namespace IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ContainerService
{
    public interface IInfoTrayProvider : IService
    {
        void DisplayMessage(string message);
        void DisplayMessage(string message, Action responsive);
        IProgressInfo DisplayProgress();
        IProgressInfo DisplayProgress(Action responsive);
    }
}