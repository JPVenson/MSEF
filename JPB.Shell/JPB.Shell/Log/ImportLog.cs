#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 12:54
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using IEADPC.Shell.Contracts.Interfaces.Services.ShellServices;
using IEADPC.Shell.Contracts.Interfaces.Services.ShellServices.Logging;
using IEADPC.Shell.Log.Model;
using IEADPC.Shell.ViewModel;

namespace IEADPC.Shell.Log
{
    public class ImportPool : ViewModelBase, IImportPool
    {
        private ImportPool()
        {

        }

        private static ImportPool _instance = new ImportPool();

        public static ImportPool Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        private static string GetSuccessIncludedMessage(Assembly assembly)
        {
            return ("Assambly '" + assembly + "' is Included and Fully Trusted");
        }

        private static string GetSuccessExcludedMessage(Assembly assembly)
        {
            return ("Assambly '" + assembly + "' is Excluded");
        }

        private static string GetFailMessage(Exception e, Assembly assembly)
        {
            return ("first chance exception of type '" + e.GetType().ToString() + "' was Handeled by StrongNameCatalog ... Assambly '" + assembly + "' is Excluded");
        }

        private static string GetFailMessage(Exception e, string assembly)
        {
            return ("first chance exception of type '" + e.GetType() + "' was Handeled by StrongNameCatalog ... Assambly '" + assembly + "' is Excluded");
        }
        
        private static object GetSuccessIncludedMessage(string filename)
        {
            return("Assambly '" + filename + "' is Included and Fully Trusted");
        }

        private static object GetImportFailMessage(Assembly assembly)
        {
            return "Assambly '" + assembly + "' is not Excluded from the Project";
        }

        public static void AddSuccessMessage(Assembly assembly, string descriptor)
        {
            Instance.LogEntries.Add(new LogEntry(assembly.FullName,
                                                 new Dictionary<string, object>
                                                     {
                                                         {"Success", true},
                                                         {"Descriptor", descriptor}
                                                     }));
            Instance.SendPropertyChanged("LogEntries");
        }

        public static void AddSuccessExcludedMessage(Assembly assembly)
        {
            Instance.LogEntries.Add(new LogEntry(assembly.FullName,
                                                 new Dictionary<string, object>
                                                     {
                                                         {"Success", true},
                                                         {"Descriptor", GetSuccessExcludedMessage(assembly)}
                                                     }));
            Instance.SendPropertyChanged("LogEntries");
        }

        public static void AddSuccessIncludedMessage(Assembly assembly)
        {
            Instance.LogEntries.Add(new LogEntry(assembly.FullName,
                                                 new Dictionary<string, object>
                                                     {
                                                         {"Success", true},
                                                         {"Descriptor", GetSuccessIncludedMessage(assembly)}
                                                     }));
            Instance.SendPropertyChanged("LogEntries");
        }


        public static void AddImportFailMessage(Assembly assembly)
        {
            Instance.LogEntries.Add(new LogEntry(assembly.FullName,
                                                 new Dictionary<string, object>
                                                     {
                                                         {"Success", false},
                                                         {"Descriptor", GetImportFailMessage(assembly)}
                                                     }));
            Instance.SendPropertyChanged("LogEntries");
        }

       

        public static void AddFailMessage(Assembly assembly, Exception exception)
        {
            Instance.LogEntries.Add(new LogEntry(assembly.FullName,
                                                 new Dictionary<string, object>
                                                     {
                                                         {"Success", false},
                                                         {"Exception", exception},
                                                         {"Descriptor", GetFailMessage(exception, assembly)}
                                                     }));
            Instance.SendPropertyChanged("LogEntries");
        }

        public static void AddFailMessage(string assemblyname, Exception exception)
        {
            Instance.LogEntries.Add(new LogEntry(assemblyname,
                                                 new Dictionary<string, object>
                                                     {
                                                         {"Success", false},
                                                         {"Exception", exception},
                                                         {"Descriptor", GetFailMessage(exception, assemblyname)}
                                                     }));
            Instance.SendPropertyChanged("LogEntries");
        }


        public static void AddSuccessIncludedMessage(string filename)
        {
            Instance.LogEntries.Add(new LogEntry(filename,
                                     new Dictionary<string, object>
                                                     {
                                                         {"Success", true},
                                                         {"Descriptor", GetSuccessIncludedMessage(filename)}
                                                     }));
            Instance.SendPropertyChanged("LogEntries");
        }


        #region LogEntries property

        private List<ILogEntry> _logEntries = new List<ILogEntry>();

        public List<ILogEntry> LogEntries
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