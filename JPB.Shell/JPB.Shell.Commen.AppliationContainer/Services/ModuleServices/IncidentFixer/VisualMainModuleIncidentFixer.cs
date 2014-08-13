#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 17:50

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using JPB.Shell.Contracts.Attributes;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Contracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services;
using JPB.Shell.Contracts.Interfaces.Services.ApplicationServices;

namespace JPB.Shell.CommonAppliationContainer.Services.ModuleServices.IncidentFixer
{
    [ServiceExport("VisualMainModuleIncidentFixer", typeof (IIncidentFixerService))]
    public class VisualMainModuleIncidentFixer : IIncidentFixerService
    {
        #region Implementation of IIncidentFixerService

        public bool IsResponsibleFor(Type targedType)
        {
            return targedType == typeof (IApplicationContainer);
        }

        public Lazy<IService, IServiceMetadata> OnIncident(
            IEnumerable<Lazy<IService, IServiceMetadata>> defauldInplementations)
        {
            return defauldInplementations.ElementAt(1);
            //return defauldInplementations.First(s => s.Metadata.Descriptor != "CommenVisualMainWindow");
        }

        #endregion

        #region Implementation of IService

        public void OnStart(IApplicationContext application)
        {
        }

        #endregion
    }
}