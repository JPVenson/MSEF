using System;

namespace IEADPC.Shell.Commen.DevExpress13140.ViewModel
{
    public class RibbonMenuCloseingEventArgs : EventArgs
    {
        private readonly Action _cancelClosing;
        private bool _cancelClose;

        public RibbonMenuCloseingEventArgs(Action cancelClosing)
        {
            _cancelClosing = cancelClosing;
        }

        public bool CancelClose
        {
            get { return _cancelClose; }
            set
            {
                _cancelClose = value;
                if (value)
                    _cancelClosing();
            }
        }
    }
}