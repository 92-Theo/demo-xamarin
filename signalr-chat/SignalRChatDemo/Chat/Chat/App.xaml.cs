using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chat
{
    public partial class App : Application
    {
        public bool IsRunTimer { get; set; } = true;
        public App()
        {
            InitializeComponent();

            ViewModels.LoginViewModel.Instance.Host = "ws://192.168.10.132:51000/ws";
            MainPage = new NavigationPage(new Views.LoginPage());
        }

        protected override void OnStart()
        {
            IsRunTimer = true;
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                //
                //
                // Process for socket
                return IsRunTimer;
            });
        }

        protected override void OnSleep()
        {

            IsRunTimer = false;
        }

        protected override void OnResume()
        {
            IsRunTimer = true;
            Device.StartTimer(TimeSpan.FromSeconds(1), () => {
                //
                //
                // Process for socket
                return IsRunTimer;
            });
        }
    }
}
