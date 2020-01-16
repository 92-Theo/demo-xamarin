using BleApp.Controls;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BleApp.ViewModels
{
    public class ScanBleViewModel : ViewModelBase
    {
        public readonly string TAG = "ScanBleViewModel";
        public static ScanBleViewModel Instance { get; private set; }

        private ObservableRangeCollection<Models.Ble> _bles = new ObservableRangeCollection<Models.Ble>();
        
        private Models.Ble _selectedBle = default;

        public ICommand RefreshCommand { get; private set; }
        public ICommand ChatCommand { get; private set; }

        public ObservableRangeCollection<Models.Ble> Bles
        {
            get => _bles;
            set => SetProperty(ref _bles, value);
        }

        public Models.Ble SelectedBle
        {
            get => _selectedBle;
            set => SetProperty(ref _selectedBle, value);
        }

        static ScanBleViewModel()
        {
            Instance = new ScanBleViewModel();
        }


        private ScanBleViewModel()
        {
            RefreshCommand = new Command(() => Refresh());
            ChatCommand = new Command(() => Chat());
        }

        public void Clear()
        {
            Log.Write(TAG, "Clear");
            Bles.Clear();
            //
            //
            // 연결된 BLE은 Scan에서 볼 수 없으므로 추가함
            Bles.AddRange(Models.BleManager.Instance.ConnectedBles);
            SelectedBle = default;
        }


        public void Refresh()
        {
            Log.Write(TAG, "Refresh");
            Clear();
            Models.BleManager.Instance.StartScan();
        }

        public async void Chat()
        {
            Models.BleManager.Instance.StopScan();
            //
            //
            // Connect
            Log.Write(TAG, "Chat");
            if (SelectedBle == null || SelectedBle.Device == null)
            {
                Log.Write(LogState.Error, TAG, "ConnectBle NoSelectedBle");
                return;
            }
            if (!await Models.BleManager.Instance.Connect(SelectedBle.Device))
            {
                return;
            }
            //
            //
            // Clear ChatScreen
            ChatViewModel.Instance.Clear();
            //
            //
            // Start Chat
            bool bRet = await Models.BleManager.Instance.StartChat(SelectedBle);
            if (bRet == false)
            {
                Models.BleManager.Instance.ClearChat();
                return;
            }
            await Application.Current.MainPage.Navigation.PushAsync(new Views.ChatPage());
        }
    }
}
