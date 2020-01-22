using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Models.Managers
{
    public class LogManager
    {
        private static readonly Lazy<LogManager> _lazy = new Lazy<LogManager>(() => new LogManager());
        public static LogManager Instance { get => _lazy.Value; }
    }
}
