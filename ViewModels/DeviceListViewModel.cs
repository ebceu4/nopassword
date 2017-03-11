using System;
using System.Windows;
using Caliburn.Micro;
using NoPassword.General;
using NoPassword.General.Bluetooth;

namespace NoPassword.ViewModels
{
    public sealed class DeviceListViewModel : Screen
    {
        private readonly IBluetoothDeviceProvider _bluetoothDeviceProvider;
        private readonly IBluetoothDeviceWatcher _bluetoothDeviceWatcher;
        private readonly IWorkstation _workstation;

        private IDisposable _deviceStatusSubscription;
        private bool _isLoading;
        private BluetoothDevice[] _devices;
        private BluetoothDevice _selectedDevice;

        public DeviceListViewModel(IBluetoothDeviceProvider deviceProvider, IBluetoothDeviceWatcher bluetoothDeviceWatcher, IWorkstation workstation)
        {
            _workstation = workstation;
            _bluetoothDeviceWatcher = bluetoothDeviceWatcher;
            _bluetoothDeviceProvider = deviceProvider;
            DisplayName = "Device List";
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            DiscoverDevices();
        }

        public async void DiscoverDevices()
        {
            try
            {
                IsLoading = true;
                Devices = await _bluetoothDeviceProvider.DiscoverDevicesInRange();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while discovering nearby devices");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public bool CanDiscoverDevices => !IsLoading;

        public void StopWatchingDevice()
        {
            _deviceStatusSubscription?.Dispose();
            _deviceStatusSubscription = null;
            NotifyOfPropertyChange(nameof(CanStopWatchingDevice));
        }

        public bool CanStopWatchingDevice => _deviceStatusSubscription != null;

        public void WatchSelectedDevice()
        {
            if(SelectedDevice == null)
                return;

            _deviceStatusSubscription?.Dispose();

            _deviceStatusSubscription = _bluetoothDeviceWatcher.Watch(SelectedDevice).Subscribe(status =>
            {
                if (status == BluetoothDeviceStatus.None)
                {
                    _workstation.Lock();
                }
            });

            NotifyOfPropertyChange(nameof(CanStopWatchingDevice));
        }

        public bool CanWatchSelectedDevice => SelectedDevice != null;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(CanDiscoverDevices));
            }
        }

        public BluetoothDevice[] Devices
        {
            get { return _devices; }
            set
            {
                _devices = value;
                NotifyOfPropertyChange();
            }
        }

        public BluetoothDevice SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(CanWatchSelectedDevice));
            }
        }
    }
}