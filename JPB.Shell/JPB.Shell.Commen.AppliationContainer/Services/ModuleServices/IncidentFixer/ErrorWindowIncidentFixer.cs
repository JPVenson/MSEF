#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 17:37
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using JPB.Shell.Contracts.Attributes;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Contracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services;
using JPB.Shell.Contracts.Interfaces.Services.ApplicationServices;

namespace JPB.Shell.CommenAppliationContainer.Services.ModuleServices.IncidentFixer
{
    [ServiceExport("ErrorWindowIncidentFixer", typeof(IIncidentFixerService))]
    public class ErrorWindowIncidentFixer : IIncidentFixerService
    {

        #region Implementation of IIncidentFixerService

        public bool IsResponsibleFor(Type targedType)
        {
            return targedType == typeof(IErrorWindowService);
        }

        public Lazy<IService, IServiceMetadata> OnIncident(IEnumerable<Lazy<IService, IServiceMetadata>> defauldInplementations)
        {
            return defauldInplementations.First();
        }

        #endregion

        #region Implementation of IService

        public void OnStart(IApplicationContext application)
        {
        }

        #endregion
    }
}