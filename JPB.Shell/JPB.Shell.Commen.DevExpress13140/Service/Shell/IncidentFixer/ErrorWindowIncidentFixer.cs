#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 17:37

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using IEADPC.Shell.Contracts.Attributes;
using IEADPC.Shell.Contracts.Interfaces;
using IEADPC.Shell.Contracts.Interfaces.Metadata;
using IEADPC.Shell.Contracts.Interfaces.Services;
using IEADPC.Shell.Contracts.Interfaces.Services.ApplicationServices;

namespace IEADPC.Shell.Commen.DevExpress13140.Service.Shell.IncidentFixer
{
    [ServiceExport("ErrorWindowIncidentFixer", typeof (IIncidentFixerService))]
    public class ErrorWindowIncidentFixer : IIncidentFixerService
    {
        #region Implementation of IIncidentFixerService

        public bool IsResponsibleFor(Type targedType)
        {
            return targedType == typeof (IErrorWindowService);
        }

        public Lazy<IService, IServiceMetadata> OnIncident(
            IEnumerable<Lazy<IService, IServiceMetadata>> defauldInplementations)
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