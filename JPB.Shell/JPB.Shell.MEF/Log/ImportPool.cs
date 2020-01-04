
//By Jean-Pierre Bachmann
//http://www.codeproject.com/Articles/682642/Using-a-Plugin-Based-Application
//Questions? Ask me!
//Microsoft Public License (MS-PL)
/*
This license governs use of the accompanying software. If you use the software, you
accept this license. If you do not accept the license, do not use the software.

1. Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the
same meaning here as under U.S. copyright law.
A "contribution" is the original software, or any additions or changes to the software.
A "contributor" is any person that distributes its contribution under this license.
"Licensed patents" are a contributor's patent claims that read directly on its contribution.

2. Grant of Rights
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

3. Conditions and Limitations
(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.

All at all - Be carefull when using this code and do not remove the License!
*/

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
            if (handler != null)
            {
	            handler(obj);
            }
        }

        internal virtual void OnServiceLoad(IService obj)
        {
            var handler = ServiceLoad;
            if (handler != null)
            {
	            handler(obj);
            }
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