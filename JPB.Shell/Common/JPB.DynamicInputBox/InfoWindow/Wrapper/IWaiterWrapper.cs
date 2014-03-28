using System;

namespace JPB.DynamicInputBox.InfoWindow.Wrapper
{
    public interface IWaiterWrapper
    {
        bool IsAsnc { get; set; }
        Func<object> WorkerFunction { get; set; }
        string WaiterText { get; set; }
        int MaxProgress { get; set; }
        int CurrentProgress { get; set; }
    }
}