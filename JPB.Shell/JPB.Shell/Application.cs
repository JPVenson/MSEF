#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 15:50

#endregion

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using JPB.Shell.Contracts.Interfaces.Services.ApplicationServices;
using JPB.Shell.MEF.Services;

namespace JPB.Shell
{
    public class Program : Application
    {
        [STAThread]
        public static void Main(string[] param)
        {
            Main2();
        }

        [DebuggerStepThrough]
        public static void Main2()
        {
            var app = new Program();
            app.Run();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                ServicePool.PreLoadServicePool("JPB");
                var defaultSingelService = ServicePool.Instance.GetDefaultSingelService<IApplicationContainer>();

                if (defaultSingelService == null)
                    throw new CompositionException("Could not load the Default IVisualMainWindow");

                if (!defaultSingelService.OnEnter())
                    Shutdown(1);
            }
            catch (LicenseException lex)
            {
                MessageBox.Show(lex.Message, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //[DebuggerStepThrough]
        protected override void OnExit(ExitEventArgs e)
        {
            var defaultSingelService = ServicePool.Instance.GetDefaultSingelService<IApplicationContainer>();

            if (defaultSingelService == null)
                throw new CompositionException("Could not load the Default IVisualMainWindow");

            if (defaultSingelService.OnLeave())
                base.Shutdown();
        }
    }
}