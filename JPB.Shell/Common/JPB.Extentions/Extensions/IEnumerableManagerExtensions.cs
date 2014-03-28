using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace JPB.Extentions.Extensions
{
    public static class IEnumerableManagerExtensions
    {
        /// <summary>
        ///     Looks for an specitic element in source and return his null based position
        /// </summary>
        /// <typeparam name="T">
        ///     Type of <see>Source</see> and <see>element</see>>
        /// </typeparam>
        /// <param name="source">An IEnumerable</param>
        /// <param name="element">The wanted element</param>
        /// <returns>
        ///     The pos of <see>element</see>"/>
        /// </returns>
        public static int IndexOf<T>(this IEnumerable<T> source, T element)
        {
            for (int i = 0; i < source.Count(); i++)
            {
                if (element.Equals(source.ElementAt(i)))
                    return i;
            }
            return -1;
        }

        /// <summary>
        ///     Add an element to the end of the <c>IEnumerable</c>
        /// </summary>
        /// <typeparam name="T">The source Type</typeparam>
        /// <param name="source">An IEnumerable</param>
        /// <param name="item">The neu element if null a new element</param>
        public static void Add<T>(this IEnumerable<T> source, T item) where T : new()
        {
            var list1 = source as List<T>;
            var list = source as List<T>;

            if (!(item == null))
            {
                if (list != null) list.Add(item);
            }
            else
            {
                if (list1 != null) list1.Add(new T());
            }
        }

        /// <summary cref="Can be Null">
        ///     Adding the <see>ItemToAdd</see> to the <see>sourcelist</see> and return the new added item
        /// </summary>
        /// <typeparam name="T">Type of list item</typeparam>
        /// <param name="sourcelist">
        ///     <see>Sourcelist</see> where the item Action will be effectet
        /// </param>
        /// <param name="ItemToAdd">The new item </param>
        /// <exception cref="ArgumentException">
        ///     Throws if the <see>Selectetitem</see> is NOT in <see>sourcelist</see>
        /// </exception>
        /// <returns>The new Selectet item</returns>
        public static T AddAndSetSelectetItem<T>(this ObservableCollection<T> sourcelist, T ItemToAdd)
            where T : new()
        {
            sourcelist.Add(ItemToAdd != null ? ItemToAdd : new T());
            return sourcelist.Last();
        }

        /// <summary>
        ///     Adding the
        ///     <cref>Can be Null</cref>
        ///     <see>ItemToAdd</see> to the <see>sourcelist</see> and return the new added item
        /// </summary>
        /// <typeparam name="T">Type of list item</typeparam>
        /// <param name="sourcelist">
        ///     <see>Sourcelist</see> where the item Action will be effectet
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws if the <see>Selectetitem</see> is NOT in <see>sourcelist</see>
        /// </exception>
        /// <returns>The new Selectet item</returns>
        public static T AddAndSetSelectetItem<T>(this ObservableCollection<T> sourcelist, Action additionalAddingAction)
        {
            additionalAddingAction.Invoke();
            return sourcelist.Last();
        }

        /// <summary>
        ///     Adding the
        ///     <cref>Can be Null</cref>
        ///     <see>ItemToAdd</see> to the <see>sourcelist</see> and return the new added item
        /// </summary>
        /// <typeparam name="T">Type of list item</typeparam>
        /// <param name="sourcelist">
        ///     <see>Sourcelist</see> where the item Action will be effectet
        /// </param>
        /// <param name="selecteditem"></param>
        /// <exception cref="ArgumentException">
        ///     Throws if the <see>Selectetitem</see> is NOT in <see>sourcelist</see>
        /// </exception>
        /// <returns>The new Selectet item</returns>
        public static void AddAndSetSelectetItem<T>(this ObservableCollection<T> sourcelist,
                                                    Func<T> additionalAddingAction, [Out, In] T selecteditem)
        {
            selecteditem = additionalAddingAction.Invoke();
        }

        //// <summary>
        //// A extention to manage a Remove of an Mananged class
        //// The <see>Selectetitem</see> will be set to the position where the old was after a Remove
        //// </summary>
        //// <typeparam name="T">Type of <see>Selectetitem</see></typeparam>
        //// <param name="sourcelist"><see>Sourcelist</see> where the item Action will be effectet </param>
        //// <param name="Selectetitem">The selected item</param>
        //// <param name="additionalAddingAction">The Additional action to run at the point where the Item should be Removed</param>
        //// <param name="CompareProperty">The name of an Property of <see>T</see> witch will be used to Compare</param>
        //// <exception cref="ArgumentException">Throws if the <see>Selectetitem</see> is NOT in <see>sourcelist</see> </exception>
        //// <returns>The new Selectet item</returns>
        ////public static T RemoveAndSetSelectetItem<T>(this IEnumerable<T> sourcelist, T Selectetitem, Action additionalAddingAction, string CompareProperty)
        ////{
        ////    if (Selectetitem == null)
        ////        return default(T);
        ////    var _compareProperty = typeof(T).GetProperty(CompareProperty);
        ////    var buff = sourcelist.FirstOrDefault(s => _compareProperty.GetValue(s, null).Equals(_compareProperty.GetValue(Selectetitem, null)));
        ////    var index = sourcelist.IndexOf(buff);
        ////    if (buff == null && index != -1)
        ////        throw new ArgumentException("Not found");
        ////    additionalAddingAction.Invoke();
        ////    var buffer = default(T);
        ////    if (!sourcelist.Any())
        ////        return buffer;
        ////    if (index == sourcelist.Count())
        ////        buffer = sourcelist.Last();
        ////    else if (index == 0)
        ////        buffer = sourcelist.Last();
        ////    else if (index <= sourcelist.Count())
        ////        buffer = sourcelist.ElementAt(index);
        ////    return buffer;
        ////}

        /// <summary>
        ///     A extention to manage a Remove of an Mananged class
        ///     The <see>Selectetitem</see> will be set to the position where the old was after a Remove
        /// </summary>
        /// <typeparam name="T">Type of SelectetItem</typeparam>
        /// <param name="sourcelist"></param>
        /// <param name="SelectetItem"></param>
        /// <param name="additionalAddingAction"></param>
        /// <returns></returns>
        public static T RemoveAndSetSelectetItem<T>(this IEnumerable<T> sourcelist, T SelectetItem,
                                                    Func<T, IEnumerable<T>> additionalAddingAction)
        {
            if (SelectetItem == null)
                return default(T);
            T buff = sourcelist.FirstOrDefault(s => s.Equals(SelectetItem));
            int index = sourcelist.IndexOf(buff);
            if (buff == null && index != -1)
                throw new ArgumentException("Not found");
            sourcelist = additionalAddingAction.Invoke(buff);
            T buffer = default(T);
            if (!sourcelist.Any())
                return buffer;
            if (index == sourcelist.Count())
                buffer = sourcelist.Last();
            else if (index == 0)
                buffer = sourcelist.Last();
            else if (index <= sourcelist.Count())
                buffer = sourcelist.ElementAt(index);
            return buffer;
        }

        /// <summary>
        ///     A extention to manage a Remove of an Mananged class
        ///     The <see>Selectetitem</see> will be set to the position where the old was after a Remove
        /// </summary>
        /// <typeparam name="T">Type of list item</typeparam>
        /// <param name="sourcelist">
        ///     <see>Sourcelist</see> where the item will be Removed
        /// </param>
        /// <param name="Selectetitem">The selected item</param>
        /// <exception cref="ArgumentException">
        ///     Throws if the <see>Selectetitem</see> is NOT in <see>sourcelist</see>
        /// </exception>
        /// <returns>The new Selectet item</returns>
        public static void RemoveAndSetSelectetItem<T>(this ICollection<T> sourcelist, ref T Selectetitem)
        {
            if (Selectetitem == null)
            {
                Selectetitem = default(T);
                return;
            }

            T buff = default(T);
            int index = -1;
            for (int i = 0; i < sourcelist.Count; i++)
            {
                if (sourcelist.ElementAt(i).Equals(Selectetitem)) continue;
                index = i;
                buff = sourcelist.ElementAt(index);
                break;
            }

            if (buff == null || index == -1)
                throw new ArgumentException("Not found");
            sourcelist.Remove(buff);
            T buffer = default(T);
            if (index == sourcelist.Count())
                buffer = sourcelist.Last();
            else if (index == 0)
                buffer = sourcelist.Last();
            else if (index <= sourcelist.Count())
                buffer = sourcelist.ElementAt(index);
            Selectetitem = buffer;
        }

        /// <summary>
        ///     A extention to manage a Remove of an Mananged class
        ///     The <see>Selectetitem</see> will be set to the position where the old was after a Remove
        /// </summary>
        /// <typeparam name="T">Type of list item</typeparam>
        /// <param name="sourcelist">
        ///     <see>Sourcelist</see> where the item will be Removed
        /// </param>
        /// <param name="Selectetitem">The selected item</param>
        /// <param name="SetToLast">
        ///     Set to <c>True</c> if the <see>returns</see>should be the last item of <see>sourcelist</see>
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Throws if the <see>Selectetitem</see> is NOT in <see>sourcelist</see>
        /// </exception>
        /// <returns>The new Selectet item</returns>
        public static T RemoveAndSetSelectetItem<T>(this ICollection<T> sourcelist, T Selectetitem, bool SetToLast)
        {
            if (Selectetitem == null)
                return default(T);
            T buff = sourcelist.FirstOrDefault(s => s.Equals(Selectetitem));
            if (buff == null)
                throw new ArgumentException("Not found");
            sourcelist.Remove(buff);
            T buffer = default(T);
            if (sourcelist.IndexOf(buff) < sourcelist.Count && SetToLast && sourcelist.Any())
                buffer = sourcelist.Last();
            return buffer;
        }

        public static T RemoveAndSetSelectetItem<T>(this ICollection<T> sourcelist, IEnumerable<T> listtoremove)
        {
            if (listtoremove == null && !listtoremove.Any())
                return default(T);
            foreach (T item in listtoremove)
            {
                sourcelist.Remove(item);
            }
            return sourcelist.Last();
        }

        public static T RemoveItemsAndSetSelectetItem<T>(this IEnumerable<T> sourcelist,
                                                         ObservableCollection<T> listtoremove,
                                                         Action<T> additionalAddingAction)
        {
            if (listtoremove == null && !listtoremove.Any())
                return default(T);
            foreach (T item in listtoremove)
            {
                additionalAddingAction(item);
            }
            return sourcelist.Last();
        }
    }
}