using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace Chat.Models.Events
{
    public class ChangedSockcetStateEventArgs : EventArgs
    {
        public string Host { get; set; }
        public WebSocketState NewState { get; set; }
        public WebSocketState OldState { get; set; }
    }
}
