using System;

namespace NoPassword.General.Bluetooth
{
    public interface IBluetoothDeviceWatcher
    {
        IObservable<BluetoothDeviceStatus> Watch(BluetoothDevice device);
    }
}