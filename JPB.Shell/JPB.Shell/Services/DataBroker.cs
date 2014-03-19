#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 13:17
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using IEADPC.Shell.Contracts.Interfaces;
using IEADPC.Shell.Contracts.Interfaces.Services.ModuleServices;
using IEADPC.Shell.Contracts.Interfaces.Services.ShellServices;

namespace IEADPC.Shell.Services
{
    public class DataBroker : IDataBroker
    {
        private DataBroker()
        {
            AddAppEnvironmentPath();
        }

        private IApplicationContext _context;
        
        private static DataBroker _instance = new DataBroker();

        public static DataBroker Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        #region Implementation of IService

        public void OnStart(IApplicationContext application)
        {
            _context = application;
        }

        #endregion

        #region Implementation of IDataBroker

        object IDataBroker.this[int index]
        {
            get { return ValueHolder.ElementAt(index); }
            set { ValueHolder.Add(index.ToString(), value); }
        }

        public object this[string index]
        {
            get { return ValueHolder[index]; }
            set { ValueHolder.Add(index, value); }
        }

        #endregion

        #region ValueHolder property

        private Dictionary<string, object> _valueHolder = new Dictionary<string, object>();

        public Dictionary<string, object> ValueHolder
        {
            get { return _valueHolder; }
            set
            {
                _valueHolder = value;
            }
        }

        #endregion


        private void AddAppEnvironmentPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" +
                          Assembly.GetExecutingAssembly() + @"\Data\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            ValueHolder["Environment.ApplicationFolder"] = path;
        }
    }
}