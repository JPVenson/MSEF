using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JPB.ErrorValidation.ValidationTyps;

namespace JPB.ErrorValidation.ValidationRules
{
    public abstract class ValidationRuleBase<T> : IErrorProvider<T>
    {
        private readonly ICollection<IValidation<T>> _vallidationErrors = new ObservableCollection<IValidation<T>>();

        protected ValidationRuleBase()
        {
            Errors = new ObservableCollection<IValidation<T>>();
        }

        #region IErrorProvider<T> Members

        public virtual bool HasError
        {
            get
            {
                if (Errors.Any())
                    return Errors.Any(s => s is Error<T>);
                return false;
            }
        }

        public bool WarningAsFailure { get; set; }

        public ObservableCollection<IValidation<T>> Errors { get; set; }

        public NoError<T> DefaultNoError { get; set; }

        public int Count
        {
            get { return _vallidationErrors.Count; }
        }

        public bool IsReadOnly
        {
            get { return _vallidationErrors.IsReadOnly; }
        }

        public Type RetrunT()
        {
            return typeof (T);
        }

        public IValidation<T> RetrunError(string columnName)
        {
            return _vallidationErrors.FirstOrDefault(s => s.ErrorIndicator == columnName);
        }

        public void Add(IValidation<T> item)
        {
            _vallidationErrors.Add(item);
        }

        public void Clear()
        {
            _vallidationErrors.Clear();
        }

        public bool Contains(IValidation<T> item)
        {
            return _vallidationErrors.Contains(item);
        }

        public void CopyTo(IValidation<T>[] array, int arrayIndex)
        {
            _vallidationErrors.CopyTo(array, arrayIndex);
        }

        public bool Remove(IValidation<T> item)
        {
            if (_vallidationErrors.Contains(item))
                return _vallidationErrors.Remove(item);
            return false;
        }

        public IEnumerator<IValidation<T>> GetEnumerator()
        {
            return _vallidationErrors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _vallidationErrors.GetEnumerator();
        }

        #endregion
    }
}