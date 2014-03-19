using System;
using System.Collections.Generic;
using System.Reflection;

namespace JPB.Shell.VisualServiceScheduler.Model
{
    public class TypeReflection : ViewModelBase
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

        private Dictionary<string, object> _propertys = default(Dictionary<string, object>);

        public Dictionary<string, object> Propertys
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
            var t = instance.GetType();
            var props = t.GetFields();
            var dict = new Dictionary<string, object>();
            foreach (var prp in props)
            {
                object value = prp.GetValue(instance);
                dict.Add(prp.Name, value);
            }
            return dict;
        }

        public static EventInfo[] EventsFromInstance(IService instance)
        {
            var t = instance.GetType();
            return t.GetEvents();
        }

        public static Type[] InterfacesFromInstance(IService instance)
        {
            var t = instance.GetType();
            return t.GetInterfaces();
        }

        public static MethodInfo[] MethodsFromInstance(object instance)
        {
            var t = instance.GetType();
            return t.GetMethods();
        }

        public static Dictionary<string, object> PropertysFromType(object instance)
        {
            var t = instance.GetType();
            var props = t.GetProperties();
            var dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(instance, new object[] { });
                dict.Add(prp.Name, value);
            }
            return dict;
        }
    }
}