using System;
using System.ComponentModel;
using System.Linq.Expressions;
using JPB.ErrorValidation.ValidationTyps;

namespace JPB.ErrorValidation
{
    public interface IErrorProviderBase<T> : IDataErrorInfo, INotifyPropertyChanged where T : class
    {
        bool HasError { get; }
        bool AddTypeToText { get; set; }
        string Error { get; }
        NoError<T> DefaultNoError { get; set; }
        IErrorProvider<T> ErrorProviderSimpleAccessAdapter { get; }

        /// <summary>
        ///     Raised when a property on this object has a new value
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;

        void SendPropertyChanged<TProperty>(Expression<Func<TProperty>> property);
        void ForceRefresh();
        string GetError(string columnName, T obj);
    }
}