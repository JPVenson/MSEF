#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 10:35

#endregion

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using DevExpress.Xpf.Bars;
using IEADPC.Shell.Contracts.Interfaces.Services;

namespace IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ContainerService
{
    public interface IRibbonAppMenu : IService, INotifyPropertyChanged
    {
        ObservableCollection<BarItem> LeftMenuPart { get; }
        IRibbonAppMenuButton RightMenuPart { get; }
        IRibbonAppMenuButton BottomMenuPart { get; }

        event EventHandler OnButtonClicked;
    }

    public interface IRibbonAppMenuButton
    {
        ICommand Command { get; }
        string ImageSource { get; }
        string Descriptor { get; }
        object View { get; }
        object ViewModel { get; }
    }
}