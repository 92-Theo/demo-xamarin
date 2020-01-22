using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Models.Events
{
    
    public class ShowDialogEventArgs
    {
        public enum DialogType
        {
            Warning, Error, Login
        }
        public DialogType Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Cancel { get; set; }
    }
}
