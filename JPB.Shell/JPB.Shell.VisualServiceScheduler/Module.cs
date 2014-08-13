#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 18:38

#endregion

using JPB.Shell.CommonContracts.Attributes;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Contracts.Interfaces.Services.ModuleServices;
using JPB.Shell.VisualServiceScheduler.ViewModel;

namespace JPB.Shell.VisualServiceScheduler
{
    [RibbonMetadata("Shell.VisualServiceScheduler", 0, 0, "VisualServiceScheduler", typeof (IVisualService))]
    public class Module : IVisualService
    {
        public static IApplicationContext Context;

        #region Implementation of IVisualModule

        public object View
        {
            get { return new View.VisualServiceScheduler {DataContext = ViewModel}; }
        }

        public object ViewModel
        {
            get { return new MainWindowViewModel(); }
        }

        public bool OnEnter()
        {
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
            Context = application;
        }

        #endregion
    }
}