using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using JPB.ErrorValidation.ValidationTyps;
using JPB.WPFBase.MVVM.ViewModel;

namespace JPB.ErrorValidation
{
    public abstract class ErrorProviderBase<T, TE> : AsyncViewModelBase, IErrorProviderBase<T>
        where T : class
        where TE : class, IErrorProvider<T>, new()
    {
        private string _error;

        protected ErrorProviderBase()
        {
            if (ErrorObserver<T>.Instance.GetProviderViaType() == null)
                ErrorObserver<T>.Instance.RegisterErrorProvider(new TE());

            AddTypeToText = true;
            DefaultNoError = new NoError<T> {ErrorText = "OK"};
        }

        #region IErrorProviderBase<T> Members

        [XmlIgnore]
        public IErrorProvider<T> ErrorProviderSimpleAccessAdapter
        {
            get { return ErrorObserver<T>.Instance.GetProviderViaType(); }
        }

        public bool HasError
        {
            get { return ErrorObserver<T>.Instance.GetProviderViaType().HasError; }
        }

        public bool AddTypeToText { get; set; }

        public string Error
        {
            get { return _error; }
        }

        [XmlIgnore]
        public NoError<T> DefaultNoError { get; set; }

        string IDataErrorInfo.Error
        {
            get { return _error; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get { return GetError(columnName, this as T); }
        }

        public void ForceRefresh()
        {
            foreach (var error in ErrorObserver<T>.Instance.GetProviderViaType())
            {
                ObManage(error.ErrorIndicator, this as T);
            }
        }

        public string GetError(string columnName, T obj)
        {
            return ObManage(columnName, obj);
        }

        #endregion

        private string ObManage(string errorIndicator, T obj)
        {
            var refference = ErrorObserver<T>.Instance.GetProviderViaType().Errors.ToList();

            if (refference.Any(s => s is NoError<T>))
            {
                IValidation<T> validation = refference.First(s => s is NoError<T>);
                refference.Remove(validation);
            }

            var listOfErrors =
                ErrorObserver<T>.Instance.GetProviderViaType().Where(s => s.ErrorIndicator == errorIndicator || s.ErrorIndicator == string.Empty);

            string errortext =
                listOfErrors.Select(vaildValidation => ManageError(obj, vaildValidation))
                            .Aggregate("", (current, error) => string.IsNullOrEmpty(error) ? current : error);

            if (!refference.Any(s => s is Error<T> || s is Warning<T>))
            {
                refference.Add(DefaultNoError);
            }

            _error = errortext;
            return errortext;
        }

        private string ManageError(T obj, IValidation<T> item)
        {
            string errortext = string.Empty;
            IErrorProvider<T> refference = ErrorObserver<T>.Instance.GetProviderViaType();
            bool conditionresult;
            try
            {
                conditionresult = item.Condition(obj);
            }
            catch (Exception)
            {
                conditionresult = true;
            }

            // Bedingung ist wahr und error ist nicht in der liste der angezeigten errors
            if (conditionresult && !refference.Errors.Contains(item))
            {
                if (AddTypeToText)
                {
                    int errtyptet = item.ErrorType.Count();
                    if (item.ErrorText.Substring(0, errtyptet) != item.ErrorType)
                        item.ErrorText = string.Format("{0} : {1}", item.ErrorType, item.ErrorText);
                }
                refference.Errors.Add(item);
                errortext = item.ErrorText;
            }

            // Bedingung ist flasch und error ist in der liste der angezeigten errors
            if (!conditionresult && refference.Errors.Contains(item))
            {
                refference.Errors.Remove(item);
            }

            else if (refference.Errors.Contains(item))
            {
                errortext = item.ErrorText;
            }

            return errortext;
        }
    }
}