using System;
using System.Collections.ObjectModel;
using PB.Controls.Wrapper;
using PB.ENUMs;
using PB.Error;

namespace PB.Controls.ViewModel.IQuestionModelImp
{
    public interface IQuestionViewModel<T> : IErrorProviderBase<T>
        where T : class
    {
        bool IsInit { get; set; }
        Object Question { get; set; }
        Object Input { get; set; }
        EingabeModus SelectedEingabeModus { get; set; }
        void Init();
    }

    public interface IMultiQuestionViewModel<T> : IQuestionViewModel<T> where T : class
    {
        string ParsexQuestionText(object question);
        ObservableCollection<IListBoxItemWrapper> PossibleInput { get; set; }
    }
}