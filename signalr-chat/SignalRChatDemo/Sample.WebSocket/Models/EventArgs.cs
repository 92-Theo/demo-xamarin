using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace Sample.WebSocket.Models
{
    public class ReceivedMessageEventArgs : System.EventArgs
    {
        public string IP { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"[{IP}]{Message}";
        }
    }

    public class ChangedSockStateEventArgs : System.EventArgs
    {
        public string IP { get; set; }
        public WebSocketState NewState { get; set; }
        public WebSocketState OldState { get; set; }

        public override string ToString()
        {
            return $"[{IP}] {OldState} >>> {NewState}";
        }
    }

    public class OccuredErrorEventArgs : EventArgs
    {
        public enum ErrorCode
        {
            Default, Exception
        }

        public Exception Exception { get; set; }

        public string Message { get; set; }

        public ErrorCode Code { get; set; } = ErrorCode.Default;
    }
}
