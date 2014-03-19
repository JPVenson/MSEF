//#region Jean-Pierre Bachmann

//// Erstellt von Jean-Pierre Bachmann am 12:17

//#endregion

//using System;
//using System.Diagnostics;
//using System.Threading;
//using System.Windows.Threading;
//using IEADPC.Shell.Commen.DevExpress13140.DXSplashScreen;
//using IEADPC.Shell.Contracts.Attributes;
//using IEADPC.Shell.Contracts.Interfaces;
//using IEADPC.Shell.Contracts.Interfaces.Services.ModuleServices;

//namespace IEADPC.Shell.Commen.DevExpress13140.Service.App
//{
//    [ServiceExport("SplashScreenService", typeof(ISplashScreenService), true, true)]
//    public class SplashScreenService : ISplashScreenService
//    {
//        public static DevExSplashScreen _splashScreen;

//        #region Implementation of IService

//        public void OnStart(IApplicationContext application)
//        {
//            Context = application;
//        }

//        #endregion

//        #region Implementation of IApplicationProvider

//        public IApplicationContext Context { get; set; }

//        #endregion

//        #region Implementation of ISplashScreenService

//        public void StartSplashScreen()
//        {
//            _thread = new Thread(StartSplashScreen);
//            _thread.SetApartmentState(ApartmentState.STA);
//            _thread.Start();

//            _dispatcher = Dispatcher.FromThread(_thread) ?? Dispatcher.CurrentDispatcher;
//            _dispatcher.Invoke(new Action(() =>
//                {
//                    _splashScreen = new DevExSplashScreen();
//                    _splashScreen.SetProgressState(true);
//                    _splashScreen.Dispatcher.Invoke(new Action(() => _splashScreen.Show()));
//                }));
//        }

//        public void StopSplashScreen()
//        {
//            _dispatcher.Invoke(new Action(() =>
//                {
//                    _splashScreen.CloseSplashScreen();
//                    _dispatcher.InvokeShutdown();
//                }));
//        }

//        #endregion

//        private Thread _thread;
//        private Dispatcher _dispatcher;

//        ~SplashScreenService()
//        {
//            _dispatcher.InvokeShutdown();
//            _thread.Abort();
//        }
//    }

//    public interface ISplashScreenService : IApplicationProvider
//    {
//        void StartSplashScreen();
//        void StopSplashScreen();
//    }
//}

