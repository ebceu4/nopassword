using System.Linq;
using System.Threading.Tasks;
using InTheHand.Net.Sockets;

namespace NoPassword.General.Bluetooth
{
    public class BluetoothDeviceProvider : IBluetoothDeviceProvider
    {
        private readonly object _lock = new object();
        private Task<BluetoothDevice[]> _discoverDevicesInRange;

        public Task<BluetoothDevice[]> DiscoverDevicesInRange()
        {
            lock (_lock)
            {
                if (_discoverDevicesInRange?.IsCompleted == true)
                {
                    _discoverDevicesInRange =
                        Task.Run(
                            () =>
                                new BluetoothClient().DiscoverDevicesInRange()
                                    .Select(d => new BluetoothDevice {DeviceName = d.DeviceName}).ToArray());
                }
            }

            return _discoverDevicesInRange;
        }
    }
}