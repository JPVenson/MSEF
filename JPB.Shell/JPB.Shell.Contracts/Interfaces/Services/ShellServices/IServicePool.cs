using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.InteropServices;
using JPB.Shell.Contracts.Interfaces.Metadata;

namespace JPB.Shell.Contracts.Interfaces.Services.ShellServices
{
    public interface IServicePool
    {
        void FreeAssambly(Assembly assambly);
        bool RegisterExportsChanged(EventHandler<ExportsChangeEventArgs> exportsChange);
        bool RegisterExportsChanging(EventHandler<ExportsChangeEventArgs> exportsChangeing);
        bool Import([Required] ComposablePart part);
        bool Compose([Required] CompositionBatch composition);
        IEnumerable<Lazy<IService, IServiceMetadata>> GetServiceInternal(bool ignoreDefauld = true);
        T GetDefaultSingelService<T>() where T : class, IService;
        T GetSingelService<T>() where T : class, IService;
        bool TryGetSingelService<T>([Out] [Required] T output) where T : class, IService;
        IEnumerable<T> GetServices<T>() where T : class, IService;
        bool TryGetServices<T>([Out] [Required] IEnumerable<T> output) where T : class, IService;
        bool TryGetMetadata([Out] [Required] IEnumerable<IServiceMetadata> output);
        IEnumerable<T> GetMetadatas<T>() where T : class, IServiceMetadata;
        IEnumerable<IServiceMetadata> GetMetadatas();
        IEnumerable<IServiceMetadata> GetAllMetadata();
    }
}