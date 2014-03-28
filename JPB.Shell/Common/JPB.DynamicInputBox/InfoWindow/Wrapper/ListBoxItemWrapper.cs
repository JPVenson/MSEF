using JPB.WPFBase.MVVM.ViewModel;

namespace JPB.DynamicInputBox.InfoWindow.Wrapper
{
    public interface IListBoxItemWrapper
    {
        int Index { get; set; }
        string Text { get; set; }
        bool IsChecked { get; set; }
    }

    public class ListBoxItemWrapper : ViewModelBase, IListBoxItemWrapper
    {
        #region Index property

        private int _index = default(int);

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                SendPropertyChanged(() => Index);
            }
        }

        #endregion

        #region Text property

        private string _text = default(string);

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                SendPropertyChanged(() => Text);
            }
        }

        #endregion

        #region IsChecked property

        private bool _isChecked = default(bool);

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                SendPropertyChanged(() => IsChecked);
            }
        }

        #endregion
    }
}