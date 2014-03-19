#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 12:51

#endregion

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using JPB.Shell.Contracts.Interfaces.Services.ShellServices.Logging;
using JPB.Shell.MEF.Properties;

namespace JPB.Shell.MEF.Log.Model
{
    public class LogEntry : INotifyPropertyChanged, ILogEntry
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
                SendPropertyChanged("Name");
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
                SendPropertyChanged("Messages");
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Name = " + Name);
            for (int i = 0; i < Messages.Count; i++)
            {
                KeyValuePair<string, object> item = Messages.ElementAt(i);
                sb.Append("Element[");
                sb.Append(i);
                sb.Append("] : Item = ");
                sb.Append(item);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void SendPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}