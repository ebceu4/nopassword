using System;
using System.Linq;
using System.Threading.Tasks;

namespace NoPassword.General.Bluetooth
{
    public class FakeBluetoothDeviceProvider : IBluetoothDeviceProvider
    {
        private readonly Random _random;

        private readonly BluetoothDevice[] _devices =
        {
            new BluetoothDevice {DeviceName = "my phone"},
            new BluetoothDevice {DeviceName = "234_solaris"},
            new BluetoothDevice {DeviceName = "GF43LFF"},
            new BluetoothDevice {DeviceName = "Mom"},
            new BluetoothDevice {DeviceName = "TvSamsung"},
            new BluetoothDevice {DeviceName = "NokiaLumia"},
            new BluetoothDevice {DeviceName = "W34F"},
            new BluetoothDevice {DeviceName = "Lumia1520"},
        };

        public FakeBluetoothDeviceProvider()
        {
            _random = new Random();
        }

        public Task<BluetoothDevice[]> DiscoverDevicesInRange()
        {
            var randomCount = _random.Next(_devices.Length - 2, _devices.Length);

            var devices = _devices.ToList();

            while (devices.Count > randomCount)
                devices.RemoveAt(_random.Next(devices.Count));

            return Task.Delay(TimeSpan.FromSeconds(1)).ContinueWith(task => devices.ToArray());
        }
    }
}