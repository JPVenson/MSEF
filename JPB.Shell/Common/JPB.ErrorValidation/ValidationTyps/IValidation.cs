using System;

namespace JPB.ErrorValidation.ValidationTyps
{
    public interface IValidation<T>
    {
        string ErrorType { get; }
        string ErrorIndicator { get; set; }
        string ErrorText { get; set; }
        Func<T, bool> Condition { get; set; }
    }
}