using Autofac;
using Caliburn.Micro;
using Caliburn.Micro.Autofac;
using NoPassword.General.Bluetooth;
using NoPassword.General.Navigation;
using NoPassword.ViewModels;

namespace NoPassword.General
{
    public class AppBootstrap : AutofacBootstrapper<MainViewModel>
    {
        public AppBootstrap()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<Navigation.Navigation>().As<INavigation>().SingleInstance();
            builder.RegisterType<EncryptedFileStorage>().As<IStorage>().SingleInstance();
            builder.RegisterType<AesEncryptionWithRsaKeyEncryption>().As<IEncryption>().SingleInstance();
            //builder.RegisterType<BluetoothDeviceProvider>().As<IBluetoothDeviceProvider>().SingleInstance();
            builder.RegisterType<FakeBluetoothDeviceProvider>().As<IBluetoothDeviceProvider>().SingleInstance();
            builder.RegisterType<BluetoothDeviceWatcher>().As<IBluetoothDeviceWatcher>().SingleInstance();
            builder.RegisterType<Workstation>().As<IWorkstation>().SingleInstance();
        }
    }

    
}