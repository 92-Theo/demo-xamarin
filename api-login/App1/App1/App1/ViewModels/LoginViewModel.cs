using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private bool isControlEnable = true;
        private string _id = string.Empty;
        private string _pw = string.Empty;

        public ICommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(() => Login(), () => IsControlEnable);
        }

        private void Login()
        {
            bool bRet = false;
            IsBusy = true;
            IsControlEnable = false;
            (LoginCommand as Command).ChangeCanExecute();
            Console.WriteLine($"Login Process : {ID}, {PW}");

            try
            {
                bRet = Services.ApiService.Instance.Login(ID, PW);
            }catch (Exception e)
            {
                Console.WriteLine($"Login Exception : {e.StackTrace}" );
            }

            if (bRet)
            {
                var nav = Application.Current.MainPage.Navigation;
                Console.WriteLine($"Nav Count : {nav.NavigationStack.Count}");
                if (nav.NavigationStack.Count > 0)
                {
                    Page lastPage = nav.NavigationStack[(nav.NavigationStack.Count - 1)];
                    nav.InsertPageBefore(new Views.HomeView(), lastPage);
                    _ = nav.PopAsync();
                }
                else
                {
                    _ = nav.PushAsync(new Views.HomeView());
                }
            }
            else
            {
                // Toast Message
                // DependencyService.Get<IToastService>().Show("Failed");
            }

            IsControlEnable = true;
            (LoginCommand as Command).ChangeCanExecute();
            IsBusy = false;
        }

        public bool IsControlEnable
        {
            get => isControlEnable;
            set => SetProperty(ref this.isControlEnable, value);
        }

        public string ID { get => _id; set => SetProperty(ref _id, value); }
        public string PW { get => _pw; set => SetProperty(ref _pw, value); }
    }
}
