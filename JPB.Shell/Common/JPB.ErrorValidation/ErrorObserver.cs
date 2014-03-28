namespace JPB.ErrorValidation
{
    public class ErrorObserver<T>
    {
        #region SINGELTON for ever

        private static ErrorObserver<T> _instance;

        private ErrorObserver()
        {
        }

        public static ErrorObserver<T> Instance
        {
            get { return _instance ?? (_instance = new ErrorObserver<T>()); }
            set { _instance = value; }
        }

        #endregion

        private IErrorProvider<T> Provider { get; set; }

        public void RegisterErrorProvider(IErrorProvider<T> provider)
        {
            Provider = (provider);
        }

        public void UnregistErrorProvider(IErrorProvider<T> provider)
        {
            Provider = null;
        }

        public IErrorProvider<T> GetProviderViaType()
        {
            return Provider;
        }
    }
}