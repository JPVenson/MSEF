using System;

namespace JPB.ErrorValidation.ValidationTyps
{
    /// <summary>
    ///     Endhält ein gültigen Error
    /// </summary>
    public class Error<T> : IValidation<T>
    {
        public Error(string ErrorText, string ErrorIndicator, Func<T, bool> Condition)
        {
            this.Condition = Condition;
            this.ErrorIndicator = ErrorIndicator;
            this.ErrorText = ErrorText;
        }

        #region IValidation<T> Members

        public string ErrorIndicator { get; set; }

        public string ErrorText { get; set; }

        public Func<T, bool> Condition { get; set; }

        public string ErrorType
        {
            get { return "Need"; }
        }

        #endregion
    }
}