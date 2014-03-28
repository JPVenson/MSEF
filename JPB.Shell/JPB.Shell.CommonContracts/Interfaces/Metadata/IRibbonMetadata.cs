using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
