using JPB.Shell.Contracts.Interfaces.Metadata;

namespace JPB.Shell.CommonContracts.Interfaces.Metadata
{
    public interface IRibbonMetadata : IVisualServiceMetadata
    {
        int PageNumber { get; }
        int GroupNumber { get; }
        string Label { get; }
    }
}