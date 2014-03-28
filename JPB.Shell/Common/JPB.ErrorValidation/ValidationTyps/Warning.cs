using System;

namespace JPB.ErrorValidation.ValidationTyps
{
    public class Warning<T> : IValidation<T>
    {
        public Warning(string ErrorText, string ErrorIndicator, Func<T, bool> Condition)
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
            get { return "Optimal"; }
        }

        #endregion
    }
}