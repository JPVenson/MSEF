using System.Collections.ObjectModel;
using JPB.Shell.CommonContracts.Interfaces.Application;
using JPB.Shell.CommonContracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services;

namespace JPB.Shell.CommonContracts.Interfaces.Services.Ribbon
{
    public interface IRibbonContentProviderService : IService
    {
        ObservableCollection<IRibbonTab> GenerateTabs(ObservableCollection<IRibbonMetadata> metadatas);
    }
}