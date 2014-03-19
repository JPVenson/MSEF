using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using JPB.Shell.Contracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services;

namespace JPB.Shell.Contracts.Attributes
{
    /// <summary>
    ///     Use this Attribute to mark that class for an Export
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ServiceExportAttribute : ExportAttribute, IServiceMetadata
    {
        public ServiceExportAttribute(string descriptor, params Type[] contract)
            : this(descriptor, false, contract)
        {

        }

        public ServiceExportAttribute(string descriptor, bool isDefauld, params Type[] contract)
            : this(descriptor, isDefauld, false, contract)
        {

        }

        public ServiceExportAttribute(string descriptor, bool isDefauld, bool forceSynchronism, params Type[] contract)
            : this(descriptor, isDefauld, forceSynchronism, int.MaxValue, contract)
        {

        }

        public ServiceExportAttribute(string descriptor, bool isDefauld, bool forceSynchronism, int priority, params Type[] contract)
            : base(typeof(IService))
        {
            Priority = priority;
            IsDefauldService = false;
            IsDefauldService = isDefauld;
            ForceSynchronism = forceSynchronism;
            Contracts = contract;
            Descriptor = descriptor;
        }

        #region IServiceMetadata Members

        [DefaultValue(int.MaxValue)]
        public int Priority { get; private set; }

        [DefaultValue(null)]
        public Type[] Contracts { get; private set; }

        [DefaultValue("")]
        public string Descriptor { get; private set; }

        [DefaultValue(false)]
        public bool IsDefauldService { get; private set; }

        [DefaultValue(false)]
        public bool ForceSynchronism { get; private set; }

        #endregion
    }
}