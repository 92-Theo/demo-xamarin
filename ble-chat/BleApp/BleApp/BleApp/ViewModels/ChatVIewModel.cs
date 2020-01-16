using BleApp.Controls;
using BleApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BleApp.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        public readonly string TAG =  "ChatViewModel";
        public static ChatViewModel Instance { get; private set; }

        private string _writeMessage;
        private ObservableCollection<string> _message;// = new ObservableRangeCollection<string>();

        public string WriteMessage
        {
            get => _writeMessage;
            set => SetProperty(ref _writeMessage, value);
        }
        public ObservableCollection<string> Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public ICommand WriteCommand { get; private set; }

        static ChatViewModel()
        {
            Instance = new ChatViewModel();
        }
        private ChatViewModel()
        {
            WriteCommand = new Command(() => Write());
            _message  = new ObservableRangeCollection<string>();
            Message = _message;
        }

        public void Clear()
        {
            Message.Clear();
            WriteMessage = default;
        }

        public void AddMessage(string message)
        {
            Message.Add(message);
        }

        private async void Write()
        {
            string message = WriteMessage;
            bool bRet = await BleManager.Instance.Write(message);
            string symbol = default;
            if (!bRet)
                symbol = "★";
            else
                WriteMessage = default;
            Message.Add($"{symbol}[SEND]{message}");
        }

    }
}
