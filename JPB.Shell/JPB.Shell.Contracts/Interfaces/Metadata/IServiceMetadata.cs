using System;

namespace JPB.Shell.Contracts.Interfaces.Metadata
{
    /// <summary>
    ///     The Based Metadata interface
    /// </summary>
    public interface IServiceMetadata
    {
        Type[] Contracts { get; }
        string Descriptor { get; }
        bool IsDefauldService { get; }
        bool ForceSynchronism { get; }
        int Priority { get; }
    }
}