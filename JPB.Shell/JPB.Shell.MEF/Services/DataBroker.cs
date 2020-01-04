
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

// Erstellt von Jean-Pierre Bachmann am 13:17

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Contracts.Interfaces.Services.ModuleServices;

namespace JPB.Shell.MEF.Services
{
    public class DataBroker : IDataBroker
    {
        private static DataBroker _instance = new DataBroker();
        private string _filename;
        private List<SettingsBrokerInstance> _settingsBrokerInstances;

        private DataBroker()
        {
            var filepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                              @"\Data\ShellDatastore.xml";
            Filename = filepath;

            AddAppEnvironmentPath();
        }

        public List<SettingsBrokerInstance> SettingsBrokerInstances
        {
            get
            {
                if (_settingsBrokerInstances == null)
                {
	                LoadFromFile(Filename);
                }

                return _settingsBrokerInstances;
            }
            set { _settingsBrokerInstances = value; }
        }

        public static DataBroker Instance
        {
            get { return _instance; }
            private set { _instance = value; }
        }

        #region Implementation of IService

        public void OnStart(IApplicationContext application)
        {
            //NOT CALLED
        }

        #endregion

        #region Implementation of IDataBroker

        [XmlIgnore]
        public object this[string index]
        {
            get { return GetData<object>(index); }
            set { SetData(index, value); }
        }

        #endregion

        private void AddAppEnvironmentPath()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Data\";
            if (!Directory.Exists(path))
            {
	            Directory.CreateDirectory(path);
            }

            var filepath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                              @"\Data\ShellDatastore.xml";

            if (string.IsNullOrEmpty(GetData<string>("Environment.ApplicationFolder")))
            {
	            SetData("Environment.ApplicationFolder", path);
            }

            if (string.IsNullOrEmpty(GetData<string>("Environment.ApplicationSaveFile")))
            {
	            SetData("Environment.ApplicationSaveFile", filepath);
            }
        }

        ~DataBroker()
        {
            SaveToFile();
        }

        public IEnumerator GetEnumerator()
        {
            return new DataBrokerEnumerator(SettingsBrokerInstances.ToArray());
        }

        #region Implementation of ISettingsDataBroker

        public string Filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                LoadFromFile(value);
            }
        }

        public void ChangeFileWithCopy(string filename)
        {
            var deepcopy = SettingsBrokerInstances.ToArray();

            Filename = filename;

            foreach (var settingsBrokerInstance in deepcopy)
            {
                if (GetData<object>(settingsBrokerInstance.Key) != null)
                {
	                OverrideData(settingsBrokerInstance.Key, settingsBrokerInstance.Value);
                }
                else
                {
	                SetData(settingsBrokerInstance.Key, settingsBrokerInstance.Value);
                }
            }
        }

        public void LoadFromFile(string file)
        {
            LoadGeneric(file);
        }

        public void SaveToFile()
        {
            SaveGeneric(Filename);
        }

        public T GetData<T>(string key) where T : class
        {
            if (SettingsBrokerInstances.All(s => s.Key != key))
            {
	            return default(T);
            }

            return SettingsBrokerInstances.FirstOrDefault(s => key == s.Key).Value as T;
        }

        public void SetData<T>(string key, T value) where T : class
        {
            if (SettingsBrokerInstances.Any(s => s.Key == key))
            {
	            throw new NotSupportedException("Allready contains this item");
            }

            SettingsBrokerInstances.Add(new SettingsBrokerInstance {Key = key, Value = value});
        }

        public void OverrideData<T>(string key, T value) where T : class
        {
            RemoveData<T>(key);
            SetData(key, value);
        }

        public bool RemoveData<T>(string key) where T : class
        {
            if (SettingsBrokerInstances.All(s => s.Key != key))
            {
	            throw new NotSupportedException("Item does not exists");
            }

            return SettingsBrokerInstances.Remove(SettingsBrokerInstances.FirstOrDefault(s => s.Key == key));
        }

        public void LoadGeneric(string filename)
        {
            if (File.Exists(filename) && File.ReadAllBytes(filename).Length != 0)
            {
                var fs = new FileStream(filename + ".typestore", FileMode.Open);
                var formatter = new BinaryFormatter();
                var ts = (TypeStore) formatter.Deserialize(fs);

                using (var textReader = new StreamReader(filename))
                {
                    var deserializer = new XmlSerializer(typeof (List<SettingsBrokerInstance>), ts.Typen.ToArray());
                    SettingsBrokerInstances = (List<SettingsBrokerInstance>) (deserializer.Deserialize(textReader));
                    return;
                }
            }
            SettingsBrokerInstances = new List<SettingsBrokerInstance>();
        }

        public void SaveGeneric(string filename)
        {
            var ts = new TypeStore();
            foreach (var settingsBrokerInstance in SettingsBrokerInstances)
            {
	            ts.Typen.Add(settingsBrokerInstance.Value.GetType());
            }

            using (var fs = new FileStream(filename + ".typestore", FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(fs, ts);
            }

            var serializer = new XmlSerializer(typeof (List<SettingsBrokerInstance>), ts.Typen.ToArray());
            using (var textWriter = new StreamWriter(filename))
            {
                serializer.Serialize(textWriter, SettingsBrokerInstances);
                textWriter.Close();
            }
        }

        #endregion

        #region Nested type: DataBrokerEnumerator

        public class DataBrokerEnumerator : IEnumerator<SettingsBrokerInstance>
        {
            private SettingsBrokerInstance[] _array;
            private int _pointer = -1;

            public DataBrokerEnumerator(SettingsBrokerInstance[] array)
            {
                _array = array;
            }

            #region Implementation of IEnumerator

            public bool MoveNext()
            {
                _pointer++;
                return _pointer < _array.Length;
            }

            public void Reset()
            {
                _pointer = -1;
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public SettingsBrokerInstance Current
            {
                get
                {
                    try
                    {
                        return _array[_pointer];
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        _array = null;
                    }
                }
            }

            #endregion

            #region Implementation of IDisposable

            public void Dispose()
            {
                _array = null;
            }

            #endregion
        }

        #endregion

        #region Nested type: TypeStore

        [Serializable]
        public class TypeStore
        {
            private List<Type> _typen = new List<Type>();

            public List<Type> Typen
            {
                get { return _typen; }
                set { _typen = value; }
            }
        }

        #endregion
    }

    public class SettingsBrokerInstance //<T>
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }
}