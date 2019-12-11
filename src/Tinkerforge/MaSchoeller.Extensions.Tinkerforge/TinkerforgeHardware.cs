using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using Tinkerforge;

namespace MaSchoeller.Extensions.Tinkerforge
{
    public abstract class TinkerforgeHardware : IHardware
    {

        public TinkerforgeHardware(string id)
        {
            Id = id;
        }


        public string Id { get; }


        internal abstract void UpdateUnderlyingDevice(Device device);

    }
}
