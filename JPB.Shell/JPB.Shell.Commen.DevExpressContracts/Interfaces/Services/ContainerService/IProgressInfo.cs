#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 13:14

#endregion

using System;

namespace IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ContainerService
{
    public interface IProgressInfo
    {
        Action UpdateProgress { get; }
        Double Progress { get; set; }
        string ProgressDescriptor { get; set; }
        Action OnEnd { get; }
    }
}