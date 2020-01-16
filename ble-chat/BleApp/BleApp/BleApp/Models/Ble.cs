using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BleApp.Models
{
    public class Ble
    {
        public IDevice Device { get; set; }
        public Guid Id { get { return Device.Id; } }
        public string Name { get { return (Device.Name == default ? "Default" : Device.Name); } }

        public Ble(IDevice device) 
        {
            Device = device;
        }

        public override string ToString()
        {
            return $"{(Name == default ? "NONAME" : Name)}({Id})";
        }
    }
}
