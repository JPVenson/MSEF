namespace JPB.Shell.Contracts.Interfaces.Services.ModuleServices
{
    public interface IVisualService : IService
    {
        object View { get; }
        object ViewModel { get; }
        bool OnEnter();
        bool OnLeave();
    }
}