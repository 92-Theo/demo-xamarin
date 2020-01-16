using System.Diagnostics;

namespace BleApp
{
    public enum LogState
    {
        Default, Warning, Error
    }
    public class Log
    {
        public static void Write(string tag, string message)
        {
            Write(LogState.Default, tag, message);
        }

        public static void Write(LogState state, string tag, string message)
        {
            string symbol = "○";
            if (state == LogState.Warning)
                symbol = "☆";
            else if (state == LogState.Error)
                symbol = "★";

            Debug.WriteLine($"{symbol}[{tag}]{message}");
        }
    }
}
