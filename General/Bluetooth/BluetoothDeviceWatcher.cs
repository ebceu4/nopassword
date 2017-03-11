using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace NoPassword.General.Bluetooth
{
    public class BluetoothDeviceWatcher : IBluetoothDeviceWatcher
    {
        private const int UpdateIntervalInSeconds = 3;

        private readonly IBluetoothDeviceProvider _bluetoothDeviceProvider;

        public BluetoothDeviceWatcher(IBluetoothDeviceProvider provider)
        {
            _bluetoothDeviceProvider = provider;
        }

        public IObservable<BluetoothDeviceStatus> Watch(BluetoothDevice device)
        {
            return Observable.Create<BluetoothDeviceStatus>(observer =>
            {
                BluetoothDeviceStatus? lastStatus = null;

                return Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(UpdateIntervalInSeconds), TaskPoolScheduler.Default).Subscribe(_ =>
                {
                    try
                    {
                        var discoverDevicesInRange = _bluetoothDeviceProvider.DiscoverDevicesInRange();
                        discoverDevicesInRange.Wait();
                        var devices = discoverDevicesInRange.Result;

                        var newStatus = BluetoothDeviceStatus.None;

                        if (devices.Any(d => d.DeviceName == device.DeviceName))
                        {
                            newStatus = BluetoothDeviceStatus.Available;
                        }

                        if (lastStatus == null || lastStatus.Value != newStatus)
                        {
                            lastStatus = newStatus;
                            observer.OnNext(newStatus);
                        }
                    }
                    catch (Exception e)
                    {
                    }
                });
            });
        }
    }
}