using Caliburn.Micro;
using NoPassword.ViewModels;

namespace NoPassword.General.Navigation
{
    public class Navigation : INavigation
    {
        private readonly IEventAggregator _eventAggregator;

        public Navigation(IEventAggregator aggregator)
        {
            _eventAggregator = aggregator;
            _eventAggregator.Subscribe(this);
        }

        public void NavigateToDeviceListScreen()
        {
            _eventAggregator.PublishOnUIThread(new NavigateMessage<DeviceListViewModel>());
        }
    }
}