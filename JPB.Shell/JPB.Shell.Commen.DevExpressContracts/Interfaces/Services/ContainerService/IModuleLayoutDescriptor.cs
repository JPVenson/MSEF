using System;
using System.Windows.Media;
using IEADPC.Shell.Contracts.Interfaces.Metadata;

namespace IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ContainerService
{
    public interface IModuleLayoutDescriptor
    {
        bool IsStart { get; }
        bool Visible { get; set; }
        string Name { get; }
        string Page { get; }
        int PageSorter { get; }
        string Group { get; }
        int GroupSorter { get; }
        string Label { get; }
        int LabelSorter { get; }
        ImageSource LargeGlyph { get; }
        Action<IModuleLayoutDescriptor, bool> OnVisibilityChangedDelegate { get; set; }
        IVisualServiceMetadata VisualServieMetadata { get; set; }
    }
}