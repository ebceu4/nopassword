using System.Threading.Tasks;

namespace NoPassword.General.Bluetooth
{
    public interface IBluetoothDeviceProvider
    {
        Task<BluetoothDevice[]> DiscoverDevicesInRange();
    }
}