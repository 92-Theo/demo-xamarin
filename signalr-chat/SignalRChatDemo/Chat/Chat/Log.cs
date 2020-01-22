using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Chat
{
    public class Log
    {
        public enum State
        {
            Default, Warninng, Error
        }
        public static void Write (string tag, string message, State state = State.Default)
        {
            string symbol = default;
            switch (state)
            {
                case State.Warninng: symbol = "☆";  break;
                case State.Error: symbol = "★"; break;
                default:
                    symbol = "●";
                    break;
            }
            Debug.WriteLine($"{symbol}[{tag}]{message}");
        }
    }
}
