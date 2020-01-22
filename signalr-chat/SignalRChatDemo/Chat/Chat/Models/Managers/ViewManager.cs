using System;
using System.Collections.Generic;
using System.Text;
using Chat.Services;
using Xamarin.Forms;
using Chat.Views;

namespace Chat.Models.Managers
{
    public class ViewManager
    {
        static readonly string TAG = "ViewManager";
        private static readonly Lazy<ViewManager> _lazy = new Lazy<ViewManager>(() => new ViewManager());
        public static ViewManager Instance { get => _lazy.Value; }

        public DialogService DialogService { get; private set; }


        //
        //
        // Views
        public ChatPage ChatPage { get; private set; }


        public ViewManager()
        {
            DialogService = new DialogService();

            ChatPage = new ChatPage();
        }

        public void PushChatPage()
        {
            foreach (var page in Application.Current.MainPage.Navigation.NavigationStack)
            {
                if (page  == ChatPage)
                {
                    return;
                }
            }

            Device.BeginInvokeOnMainThread(() => Application.Current.MainPage.Navigation.PushAsync(ChatPage));
        }
    }
}
