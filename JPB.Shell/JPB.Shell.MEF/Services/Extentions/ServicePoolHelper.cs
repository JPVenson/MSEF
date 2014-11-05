#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 10:57

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using JPB.Shell.Contracts.Interfaces.Metadata;
using JPB.Shell.Contracts.Interfaces.Services;
using JPB.Shell.Contracts.Interfaces.Services.ShellServices;

namespace JPB.Shell.MEF.Services.Extentions
{
    public static class ServicePoolExtentions
    {
        #region Metadata

        public static IServiceMetadata GetSingelMetadata(this IServicePool source)
        {
            return source.GetServiceInternal().Select(s => s.Metadata).FirstOrDefault();
        }

        public static bool TryGetSingelMetadata(this IServicePool source, [Out] IServiceMetadata output)
        {
            output = default(IServiceMetadata);
            try
            {
                output = source.GetServiceInternal().Select(s => s.Metadata).FirstOrDefault();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region TryGetServices

        public static bool TryGetServices<T>(this IServicePool source, [Out] [Required] IEnumerable<T> output,
                                             string descriptor) where T : class, IService
        {
            output = default(IEnumerable<T>);
            try
            {
                output = source.GetServiceInternal()
                               .Where(
                                   m =>
                                   m.Metadata.Contracts.Any(f => f == typeof (T)) && m.Metadata.Descriptor == descriptor)
                               .Select(m => m.Value as T);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool TryGetServices<T>(this IServicePool source, [Out] [Required] IEnumerable<T> output,
                                             IServiceMetadata metadata) where T : class, IService
        {
            output = default(IEnumerable<T>);
            try
            {
                output = source.GetServiceInternal()
                               .Where(m => m.Metadata == metadata)
                               .Select(m => m.Value as T);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region GetServices

        public static IEnumerable<T> GetServices<T>(this IServicePool source, string descriptor)
            where T : class, IService
        {
            return source.GetServiceInternal()
                         .Where(
                             m => m.Metadata.Contracts.Any(f => f == typeof (T)) && m.Metadata.Descriptor == descriptor)
                         .Select(s => s.Value as T);
        }

        public static IEnumerable<T> GetServices<T>(this IServicePool source, [Required] IServiceMetadata metadata)
            where T : class, IService
        {
            return source.GetServiceInternal()
                         .Where(m => m.Metadata.Contracts == metadata.Contracts
                                     && m.Metadata.Descriptor == metadata.Descriptor
                                     && m.Metadata.IsDefauldService == metadata.IsDefauldService
                                     && m.Metadata.ToString() == metadata.ToString())
                         .Select(s => s.Value as T);
        }

        #endregion

        #region GetSingelService

        public static T GetFirstOrDefault<T>(this IServicePool source, Func<Lazy<IService, IServiceMetadata>, bool> selector) where T:class, IService
        {
            var firstOrDefault = source.GetServiceInternal().FirstOrDefault(selector);

            if (firstOrDefault != null)
                return firstOrDefault.Value as T;

            return null;
        }

        public static T GetSingelService<T>(this IServicePool source, [Required] IServiceMetadata metadata)
            where T : class, IService
        {
            return source.GetFirstOrDefault<T>(
                    m =>
                        m.Metadata.Contracts == metadata.Contracts && m.Metadata.Descriptor == metadata.Descriptor &&
                        m.Metadata.ToString() == metadata.ToString());
        }

        public static T GetSingelService<T>(this IServicePool source, string descriptor) where T : class, IService
        {
            return
                source.GetFirstOrDefault<T>(
                    m => m.Metadata.Contracts.Any(f => f == typeof (T)) && m.Metadata.Descriptor == descriptor);
        }

        private static T GetSingelDefauldService<T>(this IServicePool source) where T : class, IService
        {
            return source.GetFirstOrDefault<T>(m => m.Metadata.Contracts.Any(f => f == typeof (T)));
        }

        #endregion

        #region TryGetSingelService

        public static bool TryGetSingelService<T>(this IServicePool source, [Out] [Required] T output,
                                                  IServiceMetadata metadata) where T : class
        {
            output = default(T);
            try
            {
                output = source.GetServiceInternal()
                               .Where(m => m.Metadata == metadata)
                               .Select(m => m.Value as T).FirstOrDefault();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool TryGetSingelService<T>(this IServicePool source, [Out] [Required] T output, string descriptor)
            where T : class
        {
            //Requires.NotNull<ICompositionService>(compositionService, "compositionService");
            //Requires.NotNull<object>(attributedPart, "attributedPart");

            output = default(T);
            try
            {
                output = source.GetServiceInternal()
                               .Where(
                                   m =>
                                   m.Metadata.Contracts.Any(f => f == typeof (T)) && m.Metadata.Descriptor == descriptor)
                               .Select(m => m.Value as T).FirstOrDefault();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}