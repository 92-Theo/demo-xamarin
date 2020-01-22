using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Sample.WebSocket.Models;
using Serilog;

namespace Sample.WebSocket
{
    public class WebSock
    {
        static readonly string TAG = "WebSock";

        #region Events
        public event EventHandler<OccuredErrorEventArgs> OccuredError;
        public event EventHandler<ChangedSockStateEventArgs> ChangedSockState;
        public event EventHandler<ReceivedMessageEventArgs> ReceivedMessage;
        #endregion


        #region Variables
        public ClientWebSocket Base { get; private set; }
        public int BufferSize { get; private set; }
        private byte[] Buffer { get; set; }
        private List<byte> Message { get; set; }
        public WebSocketState State { get => Base.State; }
        public WebSocketState NewState { get; set; }
        public WebSocketState OldState { get; set; }
        public Uri Uri { get; private set; }
        public string IP { get => Uri.ToString(); }
        public bool IsOpened { get => State == WebSocketState.Open; }
        public Queue<string> MessageQue { get; private set; }
        private Thread ProcThread { get; set; }
        public bool IsRun { get; set; } = false;
        #endregion

        #region Constructor
        public WebSock(Uri uri, int bufferSize = 4096)
        {
            this.Uri = uri;
            BufferSize = bufferSize;

            Buffer = new byte[BufferSize];
            Message = new List<byte>();
            MessageQue = new Queue<string>();


            Base = new ClientWebSocket();
            NewState = State;
            OldState = State;

            ProcThread = new Thread(Process);
        }
        #endregion

        #region Private Func
        private async Task Open()
        {
            if (IsOpened)
                return;
            try
            {
                await Base.ConnectAsync(Uri, CancellationToken.None);
            }
            catch (Exception e)
            {
                Log.Error(e, "Open");
            }
        }
        private async Task Close()
        {
            if (IsOpened)
                await Base.CloseAsync(WebSocketCloseStatus.Empty, default, CancellationToken.None);
        }
        private async Task Send()
        {
            if (!IsOpened)
                return;

            if (MessageQue.Count == 0)
                return;

            string message = MessageQue.Peek();

            try
            {
                await Base.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(message)),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }catch(Exception e)
            {
                Log.Error(e, "Send");
                return;
            }

            MessageQue.Dequeue();
        }
        private async Task Read()
        {
            if (!IsOpened)
                return;

            try
            {
                var result = await Base.ReceiveAsync(new ArraySegment<byte>(Buffer), CancellationToken.None);
                if (result.Count > 0 && result.MessageType == WebSocketMessageType.Text)
                {
                    Message.AddRange(Buffer);
                    Buffer.Initialize();
                    if (result.EndOfMessage)
                    {
                        var message = Encoding.UTF8.GetString(Message.ToArray());
                        Message.Clear();
                        ReceivedMessage?.Invoke(this, new ReceivedMessageEventArgs()
                        {
                            IP = this.IP,
                            Message = message
                        });
                        // Log.Information($"Received :{msg}");
                    }
                }

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    // Log.Warning("Received Close Message");
                }
            }catch(Exception e)
            {
                Log.Error(e, "Read");
            }
        }
        private async void Process()
        {
            Log.Debug("WebSock Process");
            while (IsRun)
            {
                OldState = NewState;
                NewState = State;
                if (OldState != NewState)
                {
                    ChangedSockState?.Invoke(this, new ChangedSockStateEventArgs()
                    {
                        IP = this.IP,
                        NewState = this.NewState,
                        OldState = this.OldState
                    });
                }
                await Open();
                await Send();
                await Read();
            }
            await Close();
            IsRun = false;
            Log.Debug("WebSock Process Exit");
        }
        #endregion

        public void Send(string message)
        {
            if (message == default)
                return;

            MessageQue.Enqueue(message);
        }
        public void Run()
        {
            Log.Debug("WebSock Run");
            IsRun = true;
            ProcThread.Start();
        }
        public void Stop()
        {
            IsRun = false;
        }
    }
}
