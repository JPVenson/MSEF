#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 16:47
#endregion

using System.Windows;
using JPB.Shell.CommonAppliationContainer.View;
using JPB.Shell.CommonAppliationContainer.ViewModel;
using JPB.Shell.Contracts.Attributes;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Contracts.Interfaces.Services.ApplicationServices;

namespace JPB.Shell.CommonAppliationContainer.Services.Shell.VisualModule
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

        public IApplicationContext ApplicationContextProxy
        {
            get { return ApplicationProxy; }
            set
            {
                ApplicationProxy = value;
            }
        }
        public static IApplicationContext ApplicationProxy { get; private set; }
    }
}