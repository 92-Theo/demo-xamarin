using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace Chat
{
    public class Helper
    {
        public static string GetString(WebSocketState state)
        {
            string message = "NONE";
            switch (state)
            {
                case WebSocketState.Closed: message = "Closed"; break;
                case WebSocketState.Aborted: message = "Aborted"; break;
                case WebSocketState.CloseReceived: message = "CloseReceived"; break;
                case WebSocketState.CloseSent: message = "CloseSent"; break;
                case WebSocketState.Connecting: message = "Connecting"; break;
                case WebSocketState.Open: message = "Open"; break;
            }

            return message;
        }
    }
}
