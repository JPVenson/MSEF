#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 16:47
#endregion

using System.Windows;
using IEADPC.Shell.CommenAppliationContainer.View;
using IEADPC.Shell.CommenAppliationContainer.ViewModel;
using IEADPC.Shell.Contracts.Attributes;
using IEADPC.Shell.Contracts.Interfaces.Services.ModuleServices;
using IEADPC.Shell.Contracts.Interfaces.Services.ShellServices;

namespace IEADPC.Shell.CommenAppliationContainer.Services.Shell.VisualModule
{
    [VisualServiceExport("CommenVisualMainWindow", typeof(IVisualService), true)]
    public class VisualMainWindow : IVisualService
    {
        public VisualMainWindow()
        {
            ViewModel = new MainWindowViewModel();
            View = new MainWindowView() { DataContext = ViewModel };
        }

        #region Implementation of IVisualModule

        public object View { get; private set; }

        public object ViewModel { get; private set; }

        public bool OnEnter()
        {
            (View as Window).Show();
            return true;
        }

        public bool OnLeave()
        {
            return true;
        }

        #endregion

        #region Implementation of IService

        public void OnStart(IApplicationContext application)
        {
        }

        #endregion
    }
}