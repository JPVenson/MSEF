using JPB.Foo.Client.Module.View;
using JPB.Foo.Client.Module.ViewModel;
using JPB.Shell.CommonContracts.Attributes;
using JPB.Shell.Contracts.Interfaces;
using JPB.Shell.Contracts.Interfaces.Services.ModuleServices;

namespace JPB.Foo.Client.Module
{
    [RibbonMetadata("Shell.Foo", 0, 0, "Foo", typeof (IVisualService))]
    public class Module : IVisualService
    {
        public static IApplicationContext Context;

        public void OnStart(IApplicationContext application)
        {
            Context = application;
        }

        public object View { get; private set; }
        public object ViewModel { get; private set; }

        public bool OnEnter()
        {
            ViewModel = new ClientViewModel();
            View = new FooClientView {DataContext = ViewModel};
            return true;
        }

        public bool OnLeave()
        {
            return false;
        }
    }
}