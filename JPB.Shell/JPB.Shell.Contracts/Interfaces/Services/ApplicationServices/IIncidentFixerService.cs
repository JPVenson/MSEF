#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 16:17

#endregion

using System;
using System.Collections.Generic;
using JPB.Shell.Contracts.Interfaces.Metadata;

namespace JPB.Shell.Contracts.Interfaces.Services.ApplicationServices
{
    public interface IIncidentFixerService : IService
    {
        bool IsResponsibleFor(Type targedType);

        Lazy<IService, IServiceMetadata> OnIncident(
            IEnumerable<Lazy<IService, IServiceMetadata>> defauldInplementations);
    }
}