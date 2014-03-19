#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 11:08

#endregion

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IEADPC.Shell.Commen.DevExpressContracts.Interfaces
{
    public interface IObservableCollectionChangedEvent<T>
    {
        ObservableCollection<T> Elements { get; }
        event PropertyChangedEventHandler ElementsChanged;
        void SendRibbonElementsChanged();
    }
}