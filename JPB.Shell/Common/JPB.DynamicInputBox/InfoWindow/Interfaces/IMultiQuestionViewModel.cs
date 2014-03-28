using System.Collections.ObjectModel;
using JPB.DynamicInputBox.InfoWindow.Wrapper;

namespace JPB.DynamicInputBox.InfoWindow.Interfaces
{
    public interface IMultiQuestionViewModel<T> : IQuestionViewModel<T> where T : class
    {
        ObservableCollection<IListBoxItemWrapper> PossibleInput { get; set; }
        string ParsexQuestionText(object question);
    }
}