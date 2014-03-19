using System.Collections.Generic;
using System.ComponentModel;

namespace JPB.Shell.Contracts.Interfaces.Services.ShellServices.Logging
{
    public interface IImportPool
    {
        List<ILogEntry> LogEntries { get; set; }

        /// <summary>
        ///     Raised when a property on this object has a new value
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;
    }
}