using System;
using DevExpress.Xpf.Core;

namespace IEADPC.Shell.Commen.DevExpress13140.DXSplashScreen
{
    /// <summary>
    ///     Interaction logic for DevExSplashScreen.xaml
    /// </summary>
    public partial class DevExSplashScreen : ISplashScreen
    {
        public DevExSplashScreen()
        {
            InitializeComponent();
            board.Completed += OnAnimationCompleted;
        }

        #region ISplashScreen

        public void Progress(double value)
        {
            progressBar.Value = value;
        }

        public void CloseSplashScreen()
        {
            Dispatcher.BeginInvoke(new Action(() => board.Begin(this)));
        }

        public void SetProgressState(bool isIndeterminate)
        {
            progressBar.IsIndeterminate = isIndeterminate;
        }

        #endregion

        #region Event Handlers

        private void OnAnimationCompleted(object sender, EventArgs e)
        {
            board.Completed -= OnAnimationCompleted;
            Close();
        }

        #endregion
    }
}