#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 15:56
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using IEADPC.Shell.Contracts.Attributes;
using IEADPC.Shell.Contracts.Interfaces.Metadata;
using IEADPC.Shell.Contracts.Interfaces.Services;
using IEADPC.Shell.Contracts.Interfaces.Services.ApplicationServices;
using IEADPC.Shell.Contracts.Interfaces.Services.ShellServices;
using IEADPC.Shell.Services;

namespace IEADPC.Shell.CommenAppliationContainer.Services.ModuleServices
{
    [ServiceExport("DefauldIncidentFixerService", typeof(IIncidentFixerService), true)]
    public class DefauldIncidentFixerService : IIncidentFixerService
    {
        #region Implementation of IService

        public bool IsResponsibleFor(Type targedType)
        {
            return false;
        }

        public Lazy<IService, IServiceMetadata> OnIncident(IEnumerable<Lazy<IService, IServiceMetadata>> defauldInplementations)
        {
            var typedFinder = ServicePool.Instance.GetServices<IIncidentFixerService>();

            var responsiv = defauldInplementations.Select(defauldInplementation => typedFinder.FirstOrDefault(incidentFixerService => incidentFixerService.IsResponsibleFor(defauldInplementation.Metadata.Contract))).FirstOrDefault(firstOrDefault => firstOrDefault != null);

            //var responsiv =
            //    typedFinder.FirstOrDefault(incidentFixerService => incidentFixerService.IsResponsibleFor());
            return responsiv != null ? responsiv.OnIncident(defauldInplementations) : null;
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