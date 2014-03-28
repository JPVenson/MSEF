using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JPB.Extentions.Extensions
{
    public static class Extensions
    {
        public static bool PublicInstancePropertiesEqual<T>(this T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                Type type = typeof (T);
                var ignoreList = new List<string>(ignore);
                ignoreList.Add("Item");
                return
                    !(from pi in
                          type.GetProperties(BindingFlags.Public |
                                             BindingFlags.Instance)
                      where self.GetType() == to.GetType()
                      where !ignoreList.Contains(pi.Name)
                      let selfValue = type.GetProperty(pi.Name).GetValue(self, null)
                      let toValue = type.GetProperty(pi.Name).GetValue(to, null)
                      where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
                      select selfValue).Any();
            }
            return self == to;
        }


        public static T Test<Q, T, R>() where T : new()
        {
            return new T();
        }

        public static T ConverTo<T>(this object value) where T : IConvertible
        {
            return (T) Convert.ChangeType(value, typeof (T));
        }
    }
}