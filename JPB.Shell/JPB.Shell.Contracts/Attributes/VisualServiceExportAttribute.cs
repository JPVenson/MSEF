using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using JPB.Shell.Contracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services;

namespace JPB.Shell.Contracts.Attributes
{
    /// <summary>
    ///     Use this Attribute to mark an class for an Visual Export
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class VisualServiceExportAttribute : ExportAttribute, IVisualServiceMetadata
    {
        public VisualServiceExportAttribute(string descriptor, params Type[] contract)
            : this(descriptor, false, contract)
        {

        }

        public VisualServiceExportAttribute(string descriptor, string imageuri, params Type[] contract)
            : this(descriptor, contract)
        {
            ImageUri = imageuri;
        }

        public VisualServiceExportAttribute(string descriptor, bool isDefauld, params Type[] contract)
            : base(typeof(IService))
        {
            IsDefauldService = false;
            IsDefauldService = isDefauld;
            Contracts = contract;
            Descriptor = descriptor;
        }

        #region IVisualServiceMetadata Members

        [DefaultValue(null)]
        public string ImageUri { get; private set; }

        [DefaultValue(null)]
        public Type[] Contracts { get; private set; }

        [DefaultValue("")]
        public string Descriptor { get; private set; }

        [DefaultValue(false)]
        public bool IsDefauldService { get; private set; }

        [DefaultValue(false)]
        public bool ForceSynchronism { get; private set; }

        [DefaultValue(int.MaxValue)]
        public int Priority { get; private set; }

        #endregion
    }
}