using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Sample.WebSocket.Models;
using Serilog;

namespace Sample.WebSocket
{
    public class WebSock_Demo
    {
        public WebSock Sock;
        public static bool IsRun = true;

        public static void Run()
        {
            WebSock_Demo demo = new WebSock_Demo();
            demo.Process();
            
        }

        public void Process()
        {
            Log.Debug("==================================");
            Log.Debug("Process");
            Log.Debug("==================================");
            Sock.Run();
            
            while (IsRun)
            {
                var msg = Console.ReadLine();
                if (msg != default)
                    Sock.Send(msg);
            }
            IsRun = false;
            Sock.Stop();
            Log.Debug("==================================");
            Log.Debug("Wait to Exit");
            Log.Debug("==================================");
            Thread.Sleep(1000);
            Log.Debug("==================================");
            Log.Debug("Exit");
            Log.Debug("==================================");
        }

        public WebSock_Demo()
        {
            Sock = new WebSock(new Uri(Program.URI));
            Sock.ReceivedMessage += ReceivedMessage_Sock;
            Sock.ChangedSockState += ChangedSockState_Sock;
        }


        public void ReceivedMessage_Sock(object sender, ReceivedMessageEventArgs e)
        {
            Log.Information($"RECV: {e.ToString()}");
        }

        public void ChangedSockState_Sock(object sender, ChangedSockStateEventArgs e)
        {
            Log.Information($"STATE: {e.ToString()}");
        }


    }
}
