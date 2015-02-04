#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 12:54

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using JPB.Shell.Contracts.Interfaces.Services;
using JPB.Shell.Contracts.Interfaces.Services.ShellServices.Logging;
using JPB.Shell.MEF.Log.Model;
using JPB.Shell.MEF.Properties;

namespace JPB.Shell.MEF.Log
{
    public class ImportPool : IImportPool
    {
        private static ImportPool _instance = new ImportPool();

        private ImportPool()
        {
            LogEntries = new ObservableCollection<ILogEntry>();
        }

        public static ImportPool Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        #region IImportPool Members

        public event PropertyChangedEventHandler PropertyChanged;

        public event Action<IService> ServiceLoad;
        public event Action<IService> ServiceInitLoad;

        internal virtual void OnServiceInitLoad(IService obj)
        {
            var handler = ServiceInitLoad;
            if (handler != null) handler(obj);
        }

        internal virtual void OnServiceLoad(IService obj)
        {
            var handler = ServiceLoad;
            if (handler != null) 
                handler(obj);
        }

        #endregion

        private static string GetSuccessIncludedMessage(Assembly assembly)
        {
            return string.Format("Assambly '{0}' is Included and Fully Trusted", assembly);
        }

        private static string GetSuccessExcludedMessage(Assembly assembly)
        {
            return string.Format("Assambly '{0}' is Excluded", assembly);
        }

        private static string GetFailMessage(Exception e, Assembly assembly)
        {
            return string.Format("first chance exception of type '{0}' was Handeled by StrongNameCatalog ... Assambly '{1}' is Excluded", e.GetType(), assembly);
        }

        private static string GetFailMessage(Exception e, string assembly)
        {
            return string.Format("first chance exception of type '{0}' was Handeled by StrongNameCatalog ... Assambly '{1}' is Excluded", e.GetType(), assembly);
        }

        private static object GetSuccessIncludedMessage(string filename)
        {
            return string.Format("Assambly '{0}' is Included and Fully Trusted", filename);
        }

        private static object GetImportFailMessage(Assembly assembly)
        {
            return string.Format("Assambly '{0}' is not Excluded from the Project", assembly);
        }

        private static object GetNotAnImportMessage(Assembly assembly)
        {
            return string.Format("Assambly '{0}' is Excluded from the Project because it has no Imports", assembly);
        }

        public static void AddSuccessMessage(Assembly assembly, string descriptor)
        {
            Instance.LogEntries.Add(new LogEntry(assembly.FullName,
                new Dictionary<string, object>
                {
                    {"Success", true},
                    {"Descriptor", descriptor}
                }));
        }

        public static void AddSuccessExcludedMessage(Assembly assembly)
        {
            Instance.LogEntries.Add(new LogEntry(assembly.FullName,
                new Dictionary<string, object>
                {
                    {"Success", true},
                    {"Descriptor", GetSuccessExcludedMessage(assembly)}
                }));
        }

        public static void AddSuccessIncludedMessage(Assembly assembly)
        {
            Instance.LogEntries.Add(new LogEntry(assembly.FullName,
                new Dictionary<string, object>
                {
                    {"Success", true},
                    {"Descriptor", GetSuccessIncludedMessage(assembly)}
                }));
        }

        public static void AddNotAnImportMessage(Assembly assembly)
        {
            Instance.LogEntries.Add(new LogEntry(assembly.FullName,
                new Dictionary<string, object>
                {
                    {"Success", null},
                    {"Descriptor", GetNotAnImportMessage(assembly)}
                }));
        }


        public static void AddImportFailMessage(Assembly assembly)
        {
            Instance.LogEntries.Add(new LogEntry(assembly.FullName,
                new Dictionary<string, object>
                {
                    {"Success", false},
                    {"Descriptor", GetImportFailMessage(assembly)}
                }));
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
        }


        public static void AddSuccessIncludedMessage(string filename)
        {
            Instance.LogEntries.Add(new LogEntry(filename,
                new Dictionary<string, object>
                {
                    {"Success", true},
                    {"Descriptor", GetSuccessIncludedMessage(filename)}
                }));
        }

        public ObservableCollection<ILogEntry> LogEntries { get; set; }
    }
}