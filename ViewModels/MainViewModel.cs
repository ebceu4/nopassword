using Autofac;
using Caliburn.Micro;
using NoPassword.General.Navigation;

namespace NoPassword.ViewModels
{
    public class MainViewModel : Conductor<IScreen>, IHandle<NavigateMessage<DeviceListViewModel>>
    {
        private readonly IComponentContext _componentContext;

        public MainViewModel(IComponentContext componentContext, IEventAggregator eventAggregator)
        {
            eventAggregator.Subscribe(this);
            _componentContext = componentContext;
            Navigate<LoginViewModel>();
        }

        public void Navigate<T>() where T : IScreen
        {
            var vm = _componentContext.Resolve<T>();
            ActivateItem(vm);
        }

        public void Handle(NavigateMessage<DeviceListViewModel> message)
        {
            Navigate<DeviceListViewModel>();
        }
    }
}