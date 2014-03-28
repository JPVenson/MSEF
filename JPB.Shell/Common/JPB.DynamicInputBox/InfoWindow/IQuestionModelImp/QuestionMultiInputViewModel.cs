#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 11:15

#endregion

using JPB.DynamicInputBox.InfoWindow.Wrapper;
using JPB.WPFBase.MVVM.DelegateCommand;

namespace JPB.DynamicInputBox.InfoWindow.IQuestionModelImp
{
    public class QuestionMultiInputViewModel : QuestionMultipleChoiceAbstrViewModel
    {
        public QuestionMultiInputViewModel(object question, EingabeModus eingabeModus)
            : base(question, eingabeModus)
        {
            AddInputCommand = new DelegateCommand(AddInput, CanAddInput);
            RemoveSelectedInputCommand = new DelegateCommand(RemoveSelectedInput, CanRemoveSelectedInput);
        }

        #region SelectedListBoxItemWrapper property

        private IListBoxItemWrapper _selectedListBoxItemWrapper = default(IListBoxItemWrapper);

        public IListBoxItemWrapper SelectedListBoxItemWrapper
        {
            get { return _selectedListBoxItemWrapper; }
            set
            {
                _selectedListBoxItemWrapper = value;
                SendPropertyChanged(() => SelectedListBoxItemWrapper);
            }
        }

        #endregion

        #region AddInput DelegateCommand

        public DelegateCommand AddInputCommand { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        private void AddInput(object sender)
        {
            SelectedListBoxItemWrapper = Output.AddAndSetSelectetItem(() => Output.Add(new ListBoxItemWrapper()));
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        /// <returns>True if you can use it otherwise false</returns>
        private bool CanAddInput(object sender)
        {
            return true;
        }

        #endregion

        #region RemoveSelectedInput DelegateCommand

        public DelegateCommand RemoveSelectedInputCommand { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        private void RemoveSelectedInput(object sender)
        {
            SelectedListBoxItemWrapper = Output.RemoveAndSetSelectetItem(SelectedListBoxItemWrapper, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        /// <returns>True if you can use it otherwise false</returns>
        private bool CanRemoveSelectedInput(object sender)
        {
            return SelectedListBoxItemWrapper != null && Output.Contains(SelectedListBoxItemWrapper);
        }

        #endregion
    }
}