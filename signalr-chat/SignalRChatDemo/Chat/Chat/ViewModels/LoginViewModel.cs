using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        static readonly string TAG = "LoginViewModel";
        private static readonly Lazy<LoginViewModel> _lazy = new Lazy<LoginViewModel>(() => new LoginViewModel());

        public static LoginViewModel Instance
        {
            get => _lazy.Value;
        }

        private string _host;

        public string Host
        {
            get => _host;
            set {  _host = value; RaisePropertyChanged("Host"); }
        }
        public RelayCommand LoginCommand { get; private set; }


        public LoginViewModel()
        {
            Host = default;
            LoginCommand = new RelayCommand(Login);
        }


        public void Login()
        {
            if (Host == default)
            {
                Log.Write(TAG, "LOGIN NOTEXT", Log.State.Error);
                Models.Managers.ViewManager.Instance.DialogService.DisplayAlert ("WARING", "NOTEXT", "OK");
                return;
            }

            Log.Write(TAG, $"LOGIN {Host}");
            Models.Managers.ChatManager.Instance.Login(Host);
        }

        public void Clear()
        {
            Host = "";
        }
    }
}
