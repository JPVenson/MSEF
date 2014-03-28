using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JPB.ErrorValidation.ValidationTyps;

namespace JPB.ErrorValidation
{
    public interface IErrorProvider<T> : ICollection<IValidation<T>>
    {
        bool HasError { get; }
        bool WarningAsFailure { get; set; }
        //ICollection<IValidation<T>> VallidationErrors { get; }
        ObservableCollection<IValidation<T>> Errors { get; set; }
        NoError<T> DefaultNoError { get; set; }

        Type RetrunT();
        IValidation<T> RetrunError(string columnName);
    }
}