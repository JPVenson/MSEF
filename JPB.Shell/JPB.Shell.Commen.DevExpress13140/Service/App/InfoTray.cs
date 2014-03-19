#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 10:59

#endregion

using System;
using IEADPC.Shell.Commen.DevExpress13140.Model;
using IEADPC.Shell.Commen.DevExpress13140.Service.Shell.VisualModule;
using IEADPC.Shell.Commen.DevExpress13140.ViewModel;
using IEADPC.Shell.Commen.DevExpressContracts.Interfaces.Services.ContainerService;
using IEADPC.Shell.Contracts.Attributes;
using IEADPC.Shell.Contracts.Interfaces;
using IEADPC.Shell.Contracts.Interfaces.Services.ApplicationServices;

namespace IEADPC.Shell.Commen.DevExpress13140.Service.App
{
    [ServiceExport("InfoTray", typeof (IInfoTrayProvider))]
    public class InfoTray : IInfoTrayProvider
    {
        private IApplicationContext _context;

        public ProgressInfo Info { get; set; }

        #region Implementation of IService

        public void OnStart(IApplicationContext application)
        {
            _context = application;
        }

        #endregion

        #region Implementation of IInfoTrayProvider

        public void DisplayMessage(string message)
        {
            var viewmodel =
                VisualMainWindow.ApplicationProxy.ServicePool.GetDefaultSingelService<IApplicationContainer>().ViewModel
                as MainWindowViewModel;
            if (viewmodel == null)
                return;

            viewmodel.InfoTrayButtonAction = null;
            viewmodel.Message = message;
        }

        public void DisplayMessage(string message, Action responsive)
        {
            var viewmodel =
                VisualMainWindow.ApplicationProxy.ServicePool.GetDefaultSingelService<IApplicationContainer>().ViewModel
                as MainWindowViewModel;
            if (viewmodel == null)
                return;

            viewmodel.InfoTrayButtonAction = responsive;
            viewmodel.Message = message;
        }

        public IProgressInfo DisplayProgress()
        {
            var viewmodel =
                VisualMainWindow.ApplicationProxy.ServicePool.GetDefaultSingelService<IApplicationContainer>().ViewModel
                as MainWindowViewModel;
            viewmodel.InfoTrayButtonAction = null;
            return Info = new ProgressInfo(updateProgress, onend);
        }

        public IProgressInfo DisplayProgress(Action responsive)
        {
            var viewmodel =
                VisualMainWindow.ApplicationProxy.ServicePool.GetDefaultSingelService<IApplicationContainer>().ViewModel
                as MainWindowViewModel;
            viewmodel.InfoTrayButtonAction = responsive;
            return Info = new ProgressInfo(updateProgress, onend);
        }

        private void updateProgress()
        {
            DisplayMessage(Info.ProgressDescriptor + " : " + Info.Progress + " %");
        }

        private void onend()
        {
            DisplayMessage(Info.ProgressDescriptor + " : is Done");
        }

        #endregion
    }
}