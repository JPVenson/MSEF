#region Jean-Pierre Bachmann

// Erstellt von Jean-Pierre Bachmann am 15:28

#endregion

using System;
using JPB.ErrorValidation;

namespace JPB.DynamicInputBox.InfoWindow.Interfaces
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
}