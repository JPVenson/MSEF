#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 14:59

#endregion

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JPB.Shell.Contracts.Interfaces.Services.ShellServices.Logging;
using JPB.WPFBase.MVVM.ViewModel;

namespace JPB.Shell.VisualServiceScheduler.Model
{
    public class ImportLogEx : ViewModelBase
    {
        public ImportLogEx()
        {
            ImportPool = Module.Context.ImportPool;
        }

        #region SelectetLogMessages property

        private ILogEntry _selectetLogMessages = default(ILogEntry);

        public ILogEntry SelectetLogMessages
        {
            get { return _selectetLogMessages; }
            set
            {
                _selectetLogMessages = value;
                SendPropertyChanged(() => SelectetLogMessages);
            }
        }

        #endregion

        #region ImportLog property

        private IImportPool _importPool = default(IImportPool);

        public IImportPool ImportPool
        {
            get { return _importPool; }
            private set
            {
                if (value == null)
                    return;
                value.PropertyChanged += ValueOnPropertyChanged;
                _importPool = value;
                LogEntries = value.LogEntries;
                SendPropertyChanged(() => ImportPool);
            }
        }

        private void ValueOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            LogEntries = ImportPool.LogEntries.ToArray();
            SendPropertyChanged(() => LogEntries);
        }

        #endregion

        #region LogEntries property

        private IEnumerable<ILogEntry> _logEntries = default(IEnumerable<ILogEntry>);

        public IEnumerable<ILogEntry> LogEntries
        {
            get { return _logEntries; }
            set
            {
                _logEntries = value;
                SendPropertyChanged(() => LogEntries);
            }
        }

        #endregion
    }
}