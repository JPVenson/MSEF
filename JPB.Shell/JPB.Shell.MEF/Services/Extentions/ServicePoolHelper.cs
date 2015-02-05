
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