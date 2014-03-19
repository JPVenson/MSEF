#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 12:51
#endregion

using System.Collections.Generic;
using IEADPC.Shell.ViewModel;

namespace IEADPC.Shell.Log.Model
{
    public class LogEntry : ViewModelBase, ILogEntry
    {
        public LogEntry(string name, Dictionary<string, object> messages)
        {
            Name = name;
            Messages = messages;
        }

        #region Name property

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                SendPropertyChanged(() => Name);
            }
        }

        #endregion

        #region Messages property

        private Dictionary<string, object> _messages;

        public Dictionary<string, object> Messages
        {
            get { return _messages; }
            set
            {
                _messages = value;
                SendPropertyChanged(() => Messages);
            }
        }

        #endregion
    }
}