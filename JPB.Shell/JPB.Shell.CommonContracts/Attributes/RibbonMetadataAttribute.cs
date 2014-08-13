using System;
using JPB.Shell.CommonContracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Attributes;

namespace JPB.Shell.CommonContracts.Attributes
{
    public class RibbonMetadataAttribute : VisualServiceExportAttribute, IRibbonMetadata
    {
        public RibbonMetadataAttribute(string descriptor, int pageNumber, int groupNumber, string label,
            params Type[] contract)
            : base(descriptor, contract)
        {
            Label = label;
            GroupNumber = groupNumber;
            PageNumber = pageNumber;
        }

        public RibbonMetadataAttribute(string descriptor, bool isDefauld, int pageNumber, int groupNumber, string label,
            params Type[] contract)
            : base(descriptor, isDefauld, contract)
        {
            Label = label;
            GroupNumber = groupNumber;
            PageNumber = pageNumber;
        }

        public RibbonMetadataAttribute(string descriptor, string imageuri, int pageNumber, int groupNumber, string label,
            params Type[] contract)
            : base(descriptor, imageuri, contract)
        {
            Label = label;
            GroupNumber = groupNumber;
            PageNumber = pageNumber;
        }

        public int PageNumber { get; private set; }
        public int GroupNumber { get; private set; }
        public string Label { get; private set; }
    }
}