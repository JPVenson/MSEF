#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 10:49

#endregion

using JPB.DynamicInputBox.InfoWindow.Interfaces;
using JPB.ErrorValidation;
using JPB.ErrorValidation.ValidationRules;
using JPB.ErrorValidation.ValidationTyps;

namespace JPB.DynamicInputBox.InfoWindow.IQuestionModelImp
{
    public abstract class QuestionAbstrViewModelBase<T, TE> : ErrorProviderBase<T, TE>, IQuestionViewModel<T>
        where T : class
        where TE : class, IErrorProvider<T>, new()
    {
        private object _input;
        private bool _isInit;
        private object _question;
        private EingabeModus _selectedEingabeModus;

        protected QuestionAbstrViewModelBase(object question, EingabeModus eingabeModus)
        {
            SelectedEingabeModus = eingabeModus;
            Question = question;
        }

        #region Implementation of IQuestionViewModel

        public bool IsInit
        {
            get { return _isInit; }
            set
            {
                _isInit = value;
                SendPropertyChanged(() => IsInit);
            }
        }

        public object Question
        {
            get { return _question; }
            set
            {
                _question = value;
                SendPropertyChanged(() => Question);
            }
        }

        public object Input
        {
            get { return _input; }
            set
            {
                _input = value;
                SendPropertyChanged(() => Input);
            }
        }

        public EingabeModus SelectedEingabeModus
        {
            get { return _selectedEingabeModus; }
            set
            {
                _selectedEingabeModus = value;
                SendPropertyChanged(() => SelectedEingabeModus);
            }
        }

        public virtual void Init()
        {
            IsInit = true;
        }

        #endregion
    }

    public class QuestionAbstrViewModelBaseValidation<T, TE> : ValidationRuleBase<QuestionAbstrViewModelBase<T, TE>>
        where T : class
        where TE : class, IErrorProvider<T>, new()
    {
        public QuestionAbstrViewModelBaseValidation()
        {
            Add(new Error<QuestionAbstrViewModelBase<T, TE>>("Bitte gebe etwas in das Eingabefeld ein", "Input",
                                                             s => s.Input == null));
        }
    }
}