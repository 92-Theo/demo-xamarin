using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace Chat.Models.Events
{
    public class ReceivedEventArgs : EventArgs
    {
        public string Host { get; set; }
        public byte[] Data { get; set; }
        public WebSocketMessageType Type { get; set; }

        public string GetString()
        {
            if (Type.HasFlag(WebSocketMessageType.Text))
            {
                return Encoding.UTF8.GetString(Data);
            }
            return "NOTEXT";
        }

        public override string ToString()
        {


            return $"Received [{GetString()}] from {Host}";
        }
    }
}
