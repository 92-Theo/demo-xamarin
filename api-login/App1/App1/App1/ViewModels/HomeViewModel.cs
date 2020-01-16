using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private bool isControlEnable = true;

        public ICommand LogoutCommand { get; private set; }

        public HomeViewModel()
        {
            LogoutCommand = new Command(() => Logout(), () => IsControlEnable);
        }

        private void Logout()
        {
            IsBusy = true;
            IsControlEnable = false;
            (LogoutCommand as Command).ChangeCanExecute();

            Console.WriteLine($"Logout Process");
            var nav = Application.Current.MainPage.Navigation;
            Console.WriteLine($"Nav Count : {nav.NavigationStack.Count}");
            if (nav.NavigationStack.Count > 0)
            {
                Page lastPage = nav.NavigationStack[(nav.NavigationStack.Count - 1)];
                nav.InsertPageBefore(new Views.LoginView(), lastPage);
                _ = nav.PopAsync();
            }
            else
            {
                _ = nav.PushAsync(new Views.LoginView());
            }

            IsControlEnable = true;
            (LogoutCommand as Command).ChangeCanExecute();
            IsBusy = false;
        }

        public bool IsControlEnable
        {
            get => isControlEnable;
            set => SetProperty(ref this.isControlEnable, value);
        }
    }
}
