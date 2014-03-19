#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 15:36

#endregion

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ContainerService;
using IEADPC.Shell.Contracts.Interfaces.Metadata;

namespace IEADPC.Shell.Commen.DevExpress13140.Ribbon
{
    public class ModuleLayoutDescriptor : IModuleLayoutDescriptor
    {
        public ModuleLayoutDescriptor()
        {
            IsStart = false;
            Visible = true;
        }

        public ModuleLayoutDescriptor(string name,
                                      string page, int pageSorter,
                                      string group, int groupSorter,
                                      string label, int labelSorter,
                                      IVisualServiceMetadata visualServieMetadata)
        {
            IsStart = false;
            Visible = true;

            Name = name;

            Page = page;
            PageSorter = pageSorter;

            Group = group;
            GroupSorter = groupSorter;

            Label = label;
            LabelSorter = labelSorter;
            VisualServieMetadata = visualServieMetadata;
            if (visualServieMetadata.ImageUri != null)
                LargeGlyph = new BitmapImage(new Uri(visualServieMetadata.ImageUri));
        }

        public string XXX
        {
            get { return "XXXX"; }
        }

        #region IModuleLayoutDescriptor Members

        public bool IsStart { get; protected set; }

        public bool Visible { get; set; }

        public string Name { get; private set; }

        public string Page { get; private set; }
        public int PageSorter { get; private set; }

        public string Group { get; private set; }
        public int GroupSorter { get; private set; }

        public string Label { get; private set; }
        public int LabelSorter { get; private set; }

        public ImageSource LargeGlyph { get; private set; }

        public Action<IModuleLayoutDescriptor, bool> OnVisibilityChangedDelegate { get; set; }

        #endregion

        #region VisualServieMetadata property

        private IVisualServiceMetadata _visualServieMetadata = default(IVisualServiceMetadata);

        public IVisualServiceMetadata VisualServieMetadata
        {
            get { return _visualServieMetadata; }
            set { _visualServieMetadata = value; }
        }

        #endregion
    }
}