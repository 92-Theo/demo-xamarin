using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Chat.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private static readonly Lazy<ChatViewModel> _lasy = new Lazy<ChatViewModel>(()=>new ChatViewModel());

        public static ChatViewModel Instance
        {
            get => _lasy.Value;
        }

        #region Variables

        private string _title;
        private string _message;
        private ObservableCollection<string> _messageBox;

        public string Title
        {
            get => _title;
            set { _title = value; RaisePropertyChanged("Title"); }
        }
        public string Message
        {
            get => _message;
            set { _message = value; RaisePropertyChanged("Message"); }
        }
        public ObservableCollection<string> MessageBox
        {
            get => _messageBox;
            set { _messageBox = value; RaisePropertyChanged("MessageBox"); }
        }

        public RelayCommand ChatCommand { get; private set; }
        #endregion


        #region Constructor
        public ChatViewModel()
        {
            Title = "Chat Room";
            Message = default;
            MessageBox = new ObservableCollection<string>();

            ChatCommand = new RelayCommand(Chat);
            ChatCommand = new RelayCommand(Chat);
        }
        #endregion

        /// <summary>
        /// Clear Data
        /// </summary>
        public void Clear()
        {
            Message = default;
            MessageBox.Clear();
            Title = "Chat Room";
        }

        private void Chat()
        {
            if (Message == default)
                return;

            Models.Managers.ChatManager.Instance.Chat(Message);

            MessageBox.Add($"[SEMD]{Message}");
        }
  
    }
}
