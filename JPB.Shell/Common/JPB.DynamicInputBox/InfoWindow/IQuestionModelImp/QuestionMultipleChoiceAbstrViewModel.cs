#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 11:00

#endregion

using System.Collections.ObjectModel;
using JPB.DynamicInputBox.InfoWindow.Wrapper;

namespace JPB.DynamicInputBox.InfoWindow.IQuestionModelImp
{
    public abstract class QuestionMultipleChoiceAbstrViewModel : QuestionViewModel
    {
        protected QuestionMultipleChoiceAbstrViewModel(object question, EingabeModus eingabeModus)
            : base(question, eingabeModus)
        {
            Output = new ObservableCollection<IListBoxItemWrapper>();
        }

        #region ListBoxItemWrappers property

        public ObservableCollection<IListBoxItemWrapper> Output
        {
            get { return Input as ObservableCollection<IListBoxItemWrapper>; }
            set
            {
                Input = value;
                SendPropertyChanged(() => Output);
            }
        }

        #endregion

        #region PossibleInput property

        private ObservableCollection<IListBoxItemWrapper> _possibleInput;

        public ObservableCollection<IListBoxItemWrapper> PossibleInput
        {
            get { return _possibleInput; }
            set
            {
                _possibleInput = value;
                SendPropertyChanged(() => PossibleInput);
            }
        }

        #endregion
    }
}