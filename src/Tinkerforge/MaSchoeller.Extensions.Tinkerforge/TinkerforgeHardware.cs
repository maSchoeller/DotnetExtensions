using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using MaSchoeller.Extensions.Tinkerforge.Internals;
using System;
using System.Collections.Generic;
using System.Text;
using Tinkerforge;

namespace MaSchoeller.Extensions.Tinkerforge
{
    public abstract class TinkerforgeHardware : IHardware
    {

        private Device? _device;

        public TinkerforgeHardware(string id)
        {
            Id = id;
        }

        public string Id { get; }

        internal virtual void UpdateUnderlyingDevice(Device device)
        {
            _device = device;
        }
        public int? GetUnderlyingDeviceVersion()
            => TinkerforgeFactory.GetDeviceVersion(_device);
        public Device? GetUnderlyingDevice()
            => _device;

        public bool? HasCoProzessor()
            => TinkerforgeFactory.DeviceHasCoPorzessor(_device);
    }
}
