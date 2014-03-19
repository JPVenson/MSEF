#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 10:21

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using IEADPC.Shell.Contracts.Attributes;
using IEADPC.Shell.Contracts.Interfaces;
using IEADPC.Shell.Contracts.Interfaces.Services.ApplicationServices;

namespace IEADPC.Shell.Commen.DevExpress13140.Service.Shell.ErrorWindow
{
    [ServiceExport("ErrorWindowService", true, typeof (IErrorWindowService))]
    public class ErrorWindowService : IErrorWindowService
    {
        private IApplicationContext _context;

        #region Implementation of IService

        private static Exception _lastRecivedEx;
        private static System.Windows.Window _window;
        private static int trace;
        private static bool Traced;

        public void OnStart(IApplicationContext application)
        {
            _context = application;
        }

        public bool OnErrorRecived(Exception ex)
        {
            _lastRecivedEx = ex;
            if (_window != null)
            {
                if (_lastRecivedEx == ex)
                    trace++;
                _window.Title = string.Format("{0} {1}", "Application Error", trace);
                _window.Background = Brushes.Red;
                _window.InvalidateProperty(System.Windows.Window.TitleProperty);
                Traced = false;

                if (trace >= 50)
                {
                    Application.Current.Shutdown();
                    return false;
                }

                return true;
            }
            Task task = new Task(() =>
            {
                DispatchActionToMainWindow(() =>
                {
                    _window = new System.Windows.Window
                    {
                        Owner = Application.Current.MainWindow,
                        Title = "Application Error",
                        SizeToContent = SizeToContent.WidthAndHeight,
                        Content = ex.ToString()
                    };
                });
                invoketraced();
            });
            invoketraced();

            return Traced;
        }

        void invoketraced()
        {
            if (_window != null)
            {
                _window.ShowDialog();
                Traced = true;
                _window = null;
            }
        }

        public void DispatchActionToMainWindow(Action message)
        {
            bool loaded = false;

            if (Application.Current.Dispatcher.HasShutdownStarted)
                message();

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (Application.Current.MainWindow != null)
                {
                    loaded = Application.Current.MainWindow.IsLoaded;
                    return;
                }
                loaded = false;
            }));
            if (!loaded)
            {
                var handle = new AutoResetEvent(false);
                EventHandler set = delegate { handle.Set(); };
                Application.Current.Dispatcher.Invoke(
                    new Action(() =>
                    {
                        if (Application.Current.MainWindow != null)
                            Application.Current.MainWindow.Loaded += (sender, args) => handle.Set();
                        else
                            Application.Current.Activated += set;
                    }));

                handle.WaitOne();
                Application.Current.Activated -= set;
            }

            Application.Current.Dispatcher.Invoke(message);
        }

        #endregion
    }
}