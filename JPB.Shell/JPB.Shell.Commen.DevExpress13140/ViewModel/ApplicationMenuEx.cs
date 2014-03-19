using System;
using System.Windows.Input;
using DevExpress.Xpf.Ribbon;

namespace IEADPC.Shell.Commen.DevExpress13140.ViewModel
{
    public class ApplicationMenuEx : ApplicationMenu
    {
        public event EventHandler<RibbonMenuCloseingEventArgs> Closing;

        public override void ClosePopup()
        {
            if (Closing != null)
            {
                CommandManager.InvalidateRequerySuggested();
                bool IsClosingCanceled = false;
                var closed = new RibbonMenuCloseingEventArgs(() => IsClosingCanceled = true);
                Closing(this, closed);
                if (IsClosingCanceled)
                    return;
            }
            base.ClosePopup();
        }
    }
}