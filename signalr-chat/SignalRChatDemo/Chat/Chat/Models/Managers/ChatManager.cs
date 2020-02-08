using Chat.Models.Events;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Websocket.Client;
using Xamarin.Forms;

namespace Chat.Models.Managers
{
    public class ChatManager
    {
        //
        // TAG
        static readonly string TAG = "ChatManager";

        private static readonly Lazy<ChatManager> _lazy = new Lazy<ChatManager>(() => new ChatManager());
        public static ChatManager Instance { get => _lazy.Value; }


        private WebsocketClient Sock { get; set; }


        public ChatManager()
        {
        }
        
        public async void Login (string host)
        {
            Log.Write(TAG, $"Login {host}");

            if (host == default)
            {
                Log.Write(TAG, $"Open NOTEXT");
                await ViewManager.Instance.DialogService.DisplayAlert("WARNING", "Insert Host", "OK");
                return;
            }

            await Open(host);
        }

        public void Chat (string message)
        {
            Log.Write(TAG, $"Chat {message}");
            if (Sock.IsRunning)
                Sock.Send(message);
            else
                Log.Write(TAG, $"Chat NORUNNING");

        }
       

        private async Task Open (string host)
        {
            if (Sock != default)
            {
                Log.Write(TAG, "Open NODEFAULT");
                if (Sock.IsStarted)
                {
                    Log.Write(TAG, "Open IsStarted");
                    if (Sock.Url.ToString() == host)
                    {
                        Log.Write(TAG, "Open SAME HOST");
                        if (Sock.IsRunning)
                        {
                            Log.Write(TAG, "Open RUNNING");
                            ViewManager.Instance.PushChatPage();
                        }
                        return;
                    }
                    Log.Write(TAG, "Open REQUEST STOP");
                    await Sock.Stop(WebSocketCloseStatus.Empty, "");
                }
            }
            //
            //
            // Creat Socket
            Log.Write(TAG, $"Open BEGIN CREATE {host}");
            var factory = new Func<ClientWebSocket>(() =>
            {
                var client = new ClientWebSocket
                {
                    Options =
                    {
                        KeepAliveInterval = TimeSpan.FromSeconds(5),
                        // Proxy = ...
                        // ClientCertificates = ...
                    }
                };
                //client.Options.SetRequestHeader("Origin", "xxx");
                return client;
            });
            
            Sock = new WebsocketClient(new Uri(host), factory);
            Sock.Name = "Theo";
            Sock.ReconnectTimeout = TimeSpan.FromSeconds(30);
            Sock.ErrorReconnectTimeout = TimeSpan.FromSeconds(30);
            Sock.ReconnectionHappened.Subscribe(type =>
            {
                Log.Write(TAG, $"Reconnection happened, type: {type}, url: {Sock.Url}");
                ViewModels.ChatViewModel.Instance.Title = Sock.Url.ToString();
                
                ViewManager.Instance.PushChatPage();
            });
            Sock.DisconnectionHappened.Subscribe(info =>
                Log.Write(TAG, $"Disconnection happened, type: {info.Type}"));

            Sock.MessageReceived.Subscribe(msg =>
            {
                Log.Write(TAG, $"Message received: {msg}");
                // BeginInvokeOnMainThread를 쓰지 않을 경우
                // 처음받아온 메시지만 업데이트 하고
                // 서버로부터 메시지를 받을 수 없음
                Device.BeginInvokeOnMainThread( () => ViewModels.ChatViewModel.Instance.MessageBox.Add(msg.Text));
            });

            Log.Write(TAG, $"Open Wait");
            await Sock.Start();
            Log.Write(TAG, $"Open  Exit");
        }
    }
}
