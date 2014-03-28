#region Jean-Pierre Bachmann
// Erstellt von Jean-Pierre Bachmann am 15:55
#endregion

using System;
using System.Windows;
using JPB.Shell.Contracts.Attributes;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Contracts.Interfaces.Services.ApplicationServices;

namespace JPB.Shell.CommonAppliationContainer.Services.Shell
{
    [ServiceExport("ErrorWindow", true, typeof(IErrorWindowService))]
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