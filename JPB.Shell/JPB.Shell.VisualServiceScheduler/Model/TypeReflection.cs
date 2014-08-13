using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JPB.Shell.Contracts.Interfaces.Services;
using JPB.WPFBase.MVVM.ViewModel;

namespace JPB.Shell.VisualServiceScheduler.Model
{
    public class TypeReflection : AsyncViewModelBase
    {
        public TypeReflection(IService instance)
        {
            Propertys = PropertysFromType(instance);
            MethodInfos = MethodsFromInstance(instance);
            FieldsInfos = FieldsFromInstance(instance);
            EventInfos = EventsFromInstance(instance);
            Interfaces = InterfacesFromInstance(instance);
        }

        #region FieldsInfos property

        private Dictionary<string, object> _fieldsInfos;

        public Dictionary<string, object> FieldsInfos
        {
            get { return _fieldsInfos; }
            set
            {
                _fieldsInfos = value;
                SendPropertyChanged(() => FieldsInfos);
            }
        }

        #endregion

        #region EventInfos property

        private EventInfo[] _eventInfos = default(EventInfo[]);

        public EventInfo[] EventInfos
        {
            get { return _eventInfos; }
            set
            {
                _eventInfos = value;
                SendPropertyChanged(() => EventInfos);
            }
        }

        #endregion

        #region Interfaces property

        private Type[] _interfaces = default(Type[]);

        public Type[] Interfaces
        {
            get { return _interfaces; }
            set
            {
                _interfaces = value;
                SendPropertyChanged(() => Interfaces);
            }
        }

        #endregion

        #region MethodInfos property

        private MethodInfo[] _methodInfos = default(MethodInfo[]);

        public MethodInfo[] MethodInfos
        {
            get { return _methodInfos; }
            set
            {
                _methodInfos = value;
                SendPropertyChanged(() => MethodInfos);
            }
        }

        #endregion

        #region Propertys property

        private IDictionary<string, object> _propertys = default(IDictionary<string, object>);

        public IDictionary<string, object> Propertys
        {
            get { return _propertys; }
            set
            {
                _propertys = value;
                SendPropertyChanged(() => Propertys);
            }
        }

        #endregion

        public static Dictionary<string, object> FieldsFromInstance(IService instance)
        {
            Type t = instance.GetType();
            FieldInfo[] props = t.GetFields();
            var dict = new Dictionary<string, object>();
            foreach (FieldInfo prp in props)
            {
                object value = prp.GetValue(instance);
                dict.Add(prp.Name, value);
            }
            return dict;
        }

        public static EventInfo[] EventsFromInstance(IService instance)
        {
            Type t = instance.GetType();
            return t.GetEvents();
        }

        public static Type[] InterfacesFromInstance(IService instance)
        {
            Type t = instance.GetType();
            return t.GetInterfaces();
        }

        public static MethodInfo[] MethodsFromInstance(object instance)
        {
            Type t = instance.GetType();
            return t.GetMethods();
        }

        public IDictionary<string, object> PropertysFromType(object instance)
        {
            Type t = instance.GetType();
            PropertyInfo[] props = t.GetProperties();
            var dict = new ConcurrentDictionary<string, object>();
            props.AsParallel().ForAll(s =>
            {
                object value = null;
                try
                {
                    value = s.GetValue(instance, new object[] { });
                }
                catch (InvalidOperationException)
                {
                    try
                    {
                        PropertyInfo prp1 = s;
                        base.ThreadSaveAction(() =>
                        {
                            value = prp1.GetValue(instance, new object[] { });
                        });
                    }
                    catch (Exception)
                    {
                        value = null;
                    }
                }
                dict.TryAdd(s.Name, value);
            });

            return dict;
        }
    }
}