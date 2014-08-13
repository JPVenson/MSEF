#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 15:56

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using JPB.Shell.CommonAppliationContainer.Services.Shell.VisualModule;
using JPB.Shell.Contracts.Attributes;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Contracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services;
using JPB.Shell.Contracts.Interfaces.Services.ApplicationServices;

namespace JPB.Shell.CommonAppliationContainer.Services.ModuleServices
{
    [ServiceExport("DefauldIncidentFixerService", true, typeof (IIncidentFixerService))]
    public class DefauldIncidentFixerService : IIncidentFixerService
    {
        #region Implementation of IService

        public bool IsResponsibleFor(Type targedType)
        {
            return false;
        }

        public Lazy<IService, IServiceMetadata> OnIncident(
            IEnumerable<Lazy<IService, IServiceMetadata>> defauldInplementations)
        {
            IEnumerable<IIncidentFixerService> typedFinder =
                VisualMainWindow.ApplicationProxy.ServicePool.GetServices<IIncidentFixerService>();

            Lazy<IService, IServiceMetadata>[] inplementations =
                defauldInplementations as Lazy<IService, IServiceMetadata>[] ?? defauldInplementations.ToArray();
            IIncidentFixerService responsiv = inplementations.Select(defauldInplementation =>
                typedFinder.FirstOrDefault(incidentFixerService =>
                    defauldInplementation.Metadata.Contracts.Any(incidentFixerService.IsResponsibleFor)))
                .FirstOrDefault(firstOrDefault => firstOrDefault != null);

            //var responsiv =
            //    typedFinder.FirstOrDefault(incidentFixerService => incidentFixerService.IsResponsibleFor());
            return responsiv != null ? responsiv.OnIncident(inplementations) : null;
        }

        #endregion

        #region Implementation of IService

        public void OnStart(IApplicationContext application)
        {
            //_context = application;
        }

        #endregion
    }
}