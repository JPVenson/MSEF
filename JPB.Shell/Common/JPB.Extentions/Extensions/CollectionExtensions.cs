using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace JPB.Extentions.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> CastWhere<T, TE>(this ICollection<TE> source)
        {
            return source.Where(e => e is T).Cast<T>();
        }

        public static string[] ToStringArray(this List<string> source)
        {
            return source.ToArray();
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<Object> enumerableList)
            where T : class
        {
            var obCollection = new ObservableCollection<T>();

            foreach (object item in enumerableList)
            {
                obCollection.Add(item as T);
            }

            return obCollection;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerableList)
            where T : class
        {
            var obCollection = new ObservableCollection<T>();

            foreach (T item in enumerableList)
            {
                obCollection.Add(item);
            }

            return obCollection;
        }

        public static ICollection<T> Cast<T, E>(this ICollection<E> enumerableList, string property)
            where E : class
            where T : class
        {
            PropertyInfo _compareProperty = typeof (E).GetProperty(property);

            //var buff = sourcelist
            //    .FirstOrDefault(s => _compareProperty.GetValue(s, null)
            //        .Equals(_compareProperty.GetValue(Selectetitem, null)));

            var buff = new ObservableCollection<T>();

            foreach (E item in enumerableList)
            {
                buff.Add(_compareProperty.GetValue(item, null));
            }

            return buff;
        }

        public static IEnumerable<T> Cast<T, E>(this IEnumerable<E> enumerableList, string property)
            where E : class
            where T : class
        {
            PropertyInfo _compareProperty = typeof (E).GetProperty(property);

            //var buff = sourcelist
            //    .FirstOrDefault(s => _compareProperty.GetValue(s, null)
            //        .Equals(_compareProperty.GetValue(Selectetitem, null)));

            var buff = new List<T>();

            foreach (E item in enumerableList)
            {
                buff.Add(_compareProperty.GetValue(item, null));
            }

            return buff;
        }
    }
}