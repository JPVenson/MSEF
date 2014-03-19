using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using IEADPC.Shell.Contracts.Interfaces.Services.ApplicationServices;
using IEADPC.Shell.Services;

namespace IEADPC.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        static App()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var app = Current as App;
            if (app != null) InvokeErrorWindow(e.ExceptionObject as Exception);
        }

        public App()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = InvokeErrorWindow(e.Exception);
        }

        public static bool InvokeErrorWindow(Exception ex)
        {
            var errorReportingService = ServicePool.Instance.GetDefaultSingelService<IErrorWindowService>();
            return errorReportingService != null && errorReportingService.OnErrorRecived(ex);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                var defaultSingelService = ServicePool.Instance.GetDefaultSingelService<IApplicationContainer>();

                if (defaultSingelService == null)
                    throw new CompositionException("Could not load the Default IVisualMainWindow");
                
                defaultSingelService.OnStart(ServicePool.ApplicationContainer);

                if (!defaultSingelService.OnEnter())
                {
                    MessageBox.Show("The application could not be Startet");
                    this.Shutdown(1);
                }
            }
            catch (System.ComponentModel.LicenseException lex)
            {
                MessageBox.Show(lex.Message, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            var defaultSingelService = ServicePool.Instance.GetDefaultSingelService<IApplicationContainer>();

            if (defaultSingelService == null)
                throw new CompositionException("Could not load the Default IVisualMainWindow");

            defaultSingelService.OnLeave();

            base.OnExit(e);
        }
    }
}
