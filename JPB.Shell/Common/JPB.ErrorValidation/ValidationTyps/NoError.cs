using System;

namespace JPB.ErrorValidation.ValidationTyps
{
    /// <summary>
    ///     Wird als Dummy benutzt um einen "kein" error zu simulieren
    /// </summary>
    public class NoError<T> : IValidation<T>
    {
        #region IValidation<T> Members

        public string ErrorIndicator { get; set; }

        public string ErrorText { get; set; }

        public Func<T, bool> Condition { get; set; }

        public string ErrorType
        {
            get { return "NoError"; }
        }

        #endregion
    }
}