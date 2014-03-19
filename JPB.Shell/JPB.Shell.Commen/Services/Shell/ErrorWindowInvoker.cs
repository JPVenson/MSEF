#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 15:55
#endregion

using System;
using System.Windows;
using IEADPC.Shell.Contracts.Attributes;
using IEADPC.Shell.Contracts.Interfaces.Services.ApplicationServices;
using IEADPC.Shell.Contracts.Interfaces.Services.ShellServices;

namespace IEADPC.Shell.CommenAppliationContainer.Services.Shell
{
    [ServiceExport("ErrorWindow", typeof(IErrorWindowService), true)]
    public class ErrorWindowServiceInvoker : IErrorWindowService
    {
        public ErrorWindowServiceInvoker()
        {

        }

        #region Implementation of IErrorWindow

        public bool OnErrorRecived(Exception ex)
        {
            MessageBox.Show(ex.Message);
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