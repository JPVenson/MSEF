#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 13:09

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JPB.DynamicInputBox.InfoWindow.Controls;
using JPB.DynamicInputBox.InfoWindow.IQuestionModelImp;
using JPB.ErrorValidation;
using JPB.ErrorValidation.ValidationRules;
using JPB.WPFBase.MVVM.DelegateCommand;

namespace JPB.DynamicInputBox.InfoWindow
{
    public class UserInputViewModel : ErrorProviderBase<UserInputViewModel, UserInputViewModelValidation>, IEnumerator
    {
        private readonly Action _abort;

        public Dictionary<string, string> Localisation = new Dictionary<string, string>()
            {
                {"GoTo","Go to page:"},
                {"Close","OK"},
                {"Back","Back"},
                {"Quit","Quit"},
            };

        public UserInputViewModel(List<object> inputQuestions, Func<List<object>> returnlist, Action abort,
                                  IEnumerable<EingabeModus> inputmode)
        {
            Inputmode = inputmode;
            Returnlist = returnlist;
            InputQuestions = inputQuestions;
            _abort = abort;
            Init();
        }

        public IEnumerable<EingabeModus> Inputmode { get; set; }

        #region SelectedStep property

        private QuestionUserControl _selectedStep = default(QuestionUserControl);

        public QuestionUserControl SelectedStep
        {
            get { return _selectedStep; }
            set
            {
                if (value != null && !Equals(SelectedStep, value))
                {
                    var vm = value.DataContext as QuestionViewModel;
                    if (!vm.IsInit)
                        vm.Init();
                    vm.ForceRefresh();

                    _selectedStep = value;
                    SendPropertyChanged(() => SelectedStep);
                }
            }
        }

        #endregion

        #region ContinueText property

        private string _continueText = default(string);

        public string ContinueText
        {
            get { return _continueText; }
            set
            {
                _continueText = value;
                SendPropertyChanged(() => ContinueText);
            }
        }

        #endregion

        #region Index property

        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                if (Index + 1 < InputQuestions.Count)
                {
                    ContinueText = Localisation["GoTo"] + (Index + 1);
                }
                else
                {
                    ContinueText = Localisation["Quit"];
                }
                if ((Index - 1) >= 0)
                {
                    PreviousText = Localisation["GoTo"] + (Index - 1);
                }
                else
                {
                    PreviousText = Localisation["Back"];
                }
                SendPropertyChanged(() => Index);
            }
        }

        #endregion

        #region InputQuestions property

        private List<object> _inputQuestions;

        public List<object> InputQuestions
        {
            get { return _inputQuestions; }
            set
            {
                _inputQuestions = value;
                SendPropertyChanged(() => InputQuestions);
            }
        }

        #endregion

        #region Returnlist property

        private Func<List<object>> _returnlist;

        public Func<List<object>> Returnlist
        {
            get { return _returnlist; }
            set
            {
                _returnlist = value;
                SendPropertyChanged(() => Returnlist);
            }
        }

        #endregion

        #region Steps property

        private ObservableCollection<QuestionUserControl> _steps = new ObservableCollection<QuestionUserControl>();

        public ObservableCollection<QuestionUserControl> Steps
        {
            get { return _steps; }
            set
            {
                _steps = value;
                SendPropertyChanged(() => Steps);
            }
        }

        #endregion

        #region NextStep DelegateCommand

        public DelegateCommand NextStepCommand { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        private void NextStep(object sender)
        {
            var vm = SelectedStep.DataContext as QuestionViewModel;

            if (Returnlist.Invoke().Count > Index)
            {
                Returnlist.Invoke()[Index] = (vm.Input);
            }
            else
            {
                Returnlist.Invoke().Add(vm.Input);
            }

            if (Index + 1 >= InputQuestions.Count)
            {
                AbortCommand.Execute(null);
            }
            else
            {
                MoveNext();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        /// <returns>True if you can use it otherwise false</returns>
        private bool CanNextStep(object sender)
        {
            //return !string.IsNullOrEmpty((Steps.DataContext as QuestionViewModel).Input);
            return !(SelectedStep.DataContext as QuestionViewModel).HasError;
        }

        #endregion

        #region PreviousStep DelegateCommand

        public DelegateCommand PreviousStepCommand { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        private void PreviousStep(object sender)
        {
            var vm = ((QuestionViewModel)SelectedStep.DataContext);
            if (Returnlist.Invoke().Count > Index)
            {
                Returnlist.Invoke()[Index] = (vm.Input);
            }
            else
            {
                Returnlist.Invoke().Add(vm.Input);
            }
            MoveBack();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        /// <returns>True if you can use it otherwise false</returns>
        private bool CanPreviousStep(object sender)
        {
            return Index != 0;
        }

        #endregion

        #region PreviousText property

        private string _previousText = default(string);

        public string PreviousText
        {
            get { return _previousText; }
            set
            {
                _previousText = value;
                SendPropertyChanged(() => PreviousText);
            }
        }

        #endregion

        #region IsClosing property

        private bool _isClosing = default(bool);

        public bool IsClosing
        {
            get { return _isClosing; }
            set
            {
                _isClosing = value;
                SendPropertyChanged(() => IsClosing);
            }
        }

        #endregion

        #region Abort DelegateCommand

        public DelegateCommand AbortCommand { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        private void Abort(object sender)
        {
            if (!IsClosing)
                _abort.Invoke();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">The transferparameter</param>
        /// <returns>True if you can use it otherwise false</returns>
        private bool CanAbort(object sender)
        {
            return true;
        }

        #endregion

        private EingabeModus GetInput(int i)
        {
            return Inputmode != null && Inputmode.Count() > i
                       ? Inputmode.ElementAt(i)
                       : EingabeModus.Text;
        }

        private QuestionViewModel SwitchTypes(EingabeModus eingabemodi, object question)
        {
            QuestionViewModel vm = null;

            switch (eingabemodi)
            {
                case EingabeModus.Text:
                    vm = new QuestionViewModel(question, eingabemodi);
                    break;
                case EingabeModus.RichText:
                    vm = new QuestionViewModel(question, eingabemodi);
                    break;
                case EingabeModus.Zahl:
                    vm = new QuestionViewModel(question, eingabemodi);
                    break;
                case EingabeModus.Date:
                    vm = new QuestionViewModel(question, eingabemodi);
                    break;
                case EingabeModus.ShowProgress:
                    vm = new QuestionActionViewModel(question, eingabemodi);
                    break;
                case EingabeModus.RadioBox:
                    vm = new QuestionMutliOrSingelInputViewModel(question, eingabemodi);
                    break;
                case EingabeModus.CheckBox:
                    vm = new QuestionMutliOrSingelInputViewModel(question, eingabemodi);
                    break;
                case EingabeModus.MultiInput:
                    vm = new QuestionMultiInputViewModel(question, eingabemodi);
                    break;
                case EingabeModus.ListView:
                    vm = new QuestionSimpleList(question, eingabemodi);
                    break;
                default:
                    vm = new QuestionViewModel(question, eingabemodi);
                    break;
            }
            return vm;
        }

        private void Init()
        {
            Index = 0;
            AbortCommand = new DelegateCommand(Abort, CanAbort);
            NextStepCommand = new DelegateCommand(NextStep, CanNextStep);
            PreviousStepCommand = new DelegateCommand(PreviousStep, CanPreviousStep);

            for (int i = 0; i < InputQuestions.Count(); i++)
            {
                object question = InputQuestions.ElementAt(i);
                EingabeModus eingabemodi = GetInput(i);
                QuestionViewModel vm = SwitchTypes(eingabemodi, question);
                Steps.Add(new QuestionUserControl { DataContext = vm });
            }

            SelectedStep = Steps.ElementAt(0);
            PreviousText = Localisation["Back"];
        }

        public bool MoveBack()
        {
            Index--;
            SelectedStep = Steps.ElementAt(Index);
            return Index < Steps.Count;
        }

        #region Implementation of IEnumerator

        public bool MoveNext()
        {
            Index++;
            SelectedStep = Steps.ElementAt(Index);
            return Index < Steps.Count;
        }

        public void Reset()
        {
            Index = 0;
        }

        public object Current
        {
            get { return SelectedStep; }
        }

        #endregion
    }

    public class UserInputViewModelValidation : ValidationRuleBase<UserInputViewModel>
    {
    }
}

//public class InputModeToViewModelToViewSelector<T>
//{
//    public InputModeToViewModelToViewSelector(EingabeModus inputModi, T questionViewModels, UIElement views)
//    {
//        InputModi = inputModi;
//        QuestionViewModels = questionViewModels;
//        Views = views;
//    }

//    #region InputModi property

//    public EingabeModus InputModi { get; set; }

//    #endregion

//    #region QuestionViewModels property

//    public T QuestionViewModels { get; set; }

//    #endregion

//    #region QuestionViewModels property

//    public UIElement Views { get; set; }

//    #endregion
//}

//public class InputModeToViewModelToView
//{
//    public InputModeToViewModelToView()
//    {
//        InputModeToViewModelToViewSelectors = new List<InputModeToViewModelToViewSelector<QuestionViewModel>>();
//    }


//    #region InputModeToViewModelToViewSelectors property

//    public List<InputModeToViewModelToViewSelector<QuestionViewModel>> InputModeToViewModelToViewSelectors { get; set; }

//    #endregion

//    public void Add(QuestionViewModel viewmodel, EingabeModus input, UIElement view)
//    {
//        InputModeToViewModelToViewSelectors.Add(new InputModeToViewModelToViewSelector<QuestionViewModel>(input, viewmodel, view));
//    }

//    public InputModeToViewModelToViewSelector<QuestionViewModel> Get<T>(T viewmodel) where T: QuestionViewModel
//    {
//        return InputModeToViewModelToViewSelectors.FirstOrDefault(s => s.QuestionViewModels == viewmodel);
//    }
//}


//if (Steps.ElementAt(i) != null)
//{
//    var vm = Steps.ElementAt(i).DataContext as IQuestionViewModel;
//    if (!vm.IsInit)
//    {
//        var question = InputQuestions[Index];
//        vm.Init(question, GetInput(i));
//    }
//}

//var vm = Steps[Index].DataContext as QuestionViewModel;

//if (!vm.IsInit)
//{
//    var question = InputQuestions[Index];
//    vm.Init(question, eingabemodi);
//}

//if (Returnlist.Invoke().Count > Index)
//{
//    if (eingabemodi != EingabeModus.ShowProgress)
//        vm.Input = Returnlist.Invoke()[Index];
//}

//SelectedStep = Steps[Index];

//vm.ForceRefresh();

//Steps.Add(element);

//private void Setinputmode(int index)
//{
//    var eingabemodi = Inputmode != null && Inputmode.Count() > index
//                                   ? Inputmode.ElementAt(index)
//                                   : EingabeModus.Text;

//    var vm = Steps[Index].DataContext as QuestionViewModel;

//    if (!vm.IsInit)
//    {
//        var question = InputQuestions[Index];
//        vm.Init(question, eingabemodi);
//    }

//    if (Returnlist.Invoke().Count > Index)
//    {
//        if (eingabemodi != EingabeModus.ShowProgress)
//            vm.Input = Returnlist.Invoke()[Index];
//    }

//    SelectedStep = Steps[Index];

//    vm.ForceRefresh();
//}