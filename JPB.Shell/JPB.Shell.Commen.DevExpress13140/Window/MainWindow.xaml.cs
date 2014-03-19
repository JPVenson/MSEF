using DevExpress.Xpf.Ribbon;

namespace IEADPC.Shell.Commen.DevExpress13140.Window
{
    /// <summary>
    ///     Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MainWindow : DXRibbonWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnClose()
        {
            base.OnClose();
            Dispatcher.InvokeShutdown();
        }
    }
}