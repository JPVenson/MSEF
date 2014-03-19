#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 16:47
#endregion

using System.Windows;
using JPB.Shell.CommenAppliationContainer.View;
using JPB.Shell.CommenAppliationContainer.ViewModel;
using JPB.Shell.Contracts.Attributes;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Contracts.Interfaces.Services.ApplicationServices;
using JPB.Shell.Contracts.Interfaces.Services.ModuleServices;

namespace JPB.Shell.CommenAppliationContainer.Services.Shell.VisualModule
{
    [VisualServiceExport("CommenVisualMainWindow", true, typeof(IApplicationContainer))]
    public class VisualMainWindow : IApplicationContainer
    {
        public VisualMainWindow()
        {

        }

        #region Implementation of IVisualModule

        public object View { get; private set; }

        public object ViewModel { get; private set; }

        public bool OnEnter()
        {
            ViewModel = new MainWindowViewModel();
            View = new MainWindowView() { DataContext = ViewModel };
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
            ApplicationContextProxy = application;
        }

        #endregion

        public IApplicationContext ApplicationContextProxy { get; private set; }
    }
}