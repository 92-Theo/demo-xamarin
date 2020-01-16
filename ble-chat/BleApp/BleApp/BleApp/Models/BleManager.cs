using BleApp.Controls;
using Plugin.BLE;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Abstractions.Exceptions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BleApp.Models
{
    public class BleManager
    {
        private readonly string TAG = "BleManager";
        public static BleManager Instance { get; private set; }

        #region Default Variable
        private int ScanTimeout = 10000; 
        public ObservableRangeCollection<Models.Ble> ConnectedBles { get; private set; } = new ObservableRangeCollection<Models.Ble>();
        public Models.Ble ChattingBle { get; set; } = default;

        private IBluetoothLE BLE { get { return CrossBluetoothLE.Current; } }
        private IAdapter Adapter { get { return CrossBluetoothLE.Current.Adapter; } }

        #endregion

        #region Chat
        private ICharacteristic ReadCharacteristic = default;
        private ICharacteristic WriteCharacteristic = default;
        private UTF8Encoding Encoder = new UTF8Encoding();
        #endregion

        #region Constructor
        static BleManager()
        {
            Instance = new BleManager();
        }

        private BleManager()
        {
            BLE.StateChanged += BLE_StateChanged;

            Adapter.DeviceDisconnected += Adapter_DeviceDisconnected;
            Adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
            Adapter.DeviceConnected += Adapter_DeviceConnected;

            Adapter.ScanTimeout = ScanTimeout;
        }
        #endregion

        #region Events
        private void Adapter_DeviceConnected(object sender, DeviceEventArgs e)
        {
            Ble ble = new Ble(e.Device);
            Log.Write(TAG, $"Adapter_DeviceConnected {ble}");
            ConnectedBles.Add(ble);
        }

        private void Adapter_DeviceDisconnected(object sender, DeviceEventArgs e)
        {
            Ble removeItem = default;
            foreach (Ble item in ConnectedBles)
            {
                if (item.Device.Id == e.Device.Id)
                {
                    removeItem = item;
                    break;
                }
            }
            if (removeItem != default)
            {
                if (ConnectedBles.Remove(removeItem))
                    Log.Write(TAG, $"Adapter_DeviceDisconnected {removeItem}");
            }
        }

        private void Adapter_DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            Ble ble = new Ble(e.Device);
            Log.Write(TAG, $"discoverd {ble} \n {e.Device.State}");
            ViewModels.ScanBleViewModel.Instance.Bles.Add(ble);
        }
        

        private void BLE_StateChanged(object sender, BluetoothStateChangedArgs e)
        {
            Log.Write(TAG, $"The bluetooth state changed to {e.NewState}");
            if (e.NewState != BluetoothState.On)
            {
                // Chat 초기화 작업
            }
        }
        #endregion

        #region Default Func
        public async void StartScan()
        {
            if (!await CheckPermission())
            {
                Log.Write(LogState.Error, TAG, "NO Permission");
                return;
            }

            if (Adapter.IsScanning)
                await Adapter.StopScanningForDevicesAsync();

            await Adapter.StartScanningForDevicesAsync();
        }

        public async void StopScan()
        {
            if (!Adapter.IsScanning)
                return;

            await Adapter.StopScanningForDevicesAsync();
        }

        public async Task<bool> CheckPermission()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status != PermissionStatus.Granted)
            {
                status = await Models.Utils.CheckPermissions(Permission.Location);
            }

            return (status == PermissionStatus.Granted);
        }


        public async Task<bool> Connect(IDevice dev)
        {
            try
            {
                await Adapter.ConnectToDeviceAsync(dev);
            }
            catch (DeviceConnectionException e)
            {
                Log.Write(LogState.Error, TAG, $"Connect Exception: {e.StackTrace}");
                return false;
            }

            return true;
        }

        public async Task<bool> Connect(Models.Ble ble)
        {
            if (ConnectedBles.Contains(ble))
            {
                return true;
            }
            return await Connect(ble.Device);
        }
        #endregion

        #region Chat Func
        public async Task<bool> StartChat(Models.Ble ble)
        {
            if (ble == null)
                return false;
            ChattingBle = ble;
            
            var services = await ChattingBle.Device.GetServicesAsync();
            foreach (var service in services)
            {
                var characteristics = await service.GetCharacteristicsAsync();
                foreach (var characteristic in characteristics)
                {
                    if (characteristic.Properties.HasFlag(CharacteristicPropertyType.Write | CharacteristicPropertyType.WriteWithoutResponse))
                    {
                        Log.Write(TAG, $"Add WriteCharacteristic {characteristic.Properties}");
                        WriteCharacteristic = characteristic;
                    }

                    if (characteristic.Properties.HasFlag(CharacteristicPropertyType.Notify))
                    {
                        ReadCharacteristic = characteristic;
                        characteristic.ValueUpdated += Read;
                        await characteristic.StartUpdatesAsync();
                    }
                }
            }

            if (ReadCharacteristic == default || WriteCharacteristic == default)
            { 
                ClearChat();
                return false;
            }
            return true;
        }

     

        private   void Read(object Sender, CharacteristicUpdatedEventArgs e)
        {
        //   await Task.Delay(1000);

            if (e.Characteristic != ReadCharacteristic)
            {
                e.Characteristic.ValueUpdated -= Read;
                return;
            }

            var message = $"[RECV]{Encoder.GetString(e.Characteristic.Value)}";
            Log.Write(TAG, $"Raed: {message}");
            // 주스레드에 접근하기 위해 사용
            Device.BeginInvokeOnMainThread(() => ViewModels.ChatViewModel.Instance.AddMessage(message));
            //ViewModels.ChatViewModel.Instance.Message.Add(message);
        }
       
        public void ClearChat()
        {
            ChattingBle = default;
            WriteCharacteristic = default;
            if (ReadCharacteristic != default)
                ReadCharacteristic.ValueUpdated -= Read;
            ReadCharacteristic = default;
        }
        public async Task<bool> Write (string message)
        {
            if (WriteCharacteristic == default)
                return false;

            return await WriteCharacteristic.WriteAsync(Encoder.GetBytes(message));
        }
        #endregion
    }
}
