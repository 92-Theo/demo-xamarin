using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Chat.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            InitializeComponent();

            this.BindingContext = ViewModels.ChatViewModel.Instance;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // ViewModels.LoginViewModel.Instance.Clear();
        }
    }
}