using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using MaSchoeller.Extensions.Tinkerforge.Abstracts.Sensors;
using System;
using System.Collections.Generic;
using System.Text;
using Tinkerforge;

namespace MaSchoeller.Extensions.Tinkerforge.Hardware
{
    public class TinkerforgeGps : TinkerforgeHardware, IGpsSensor
    {
        public event EventHandler<SensorUpdatedEventArgs<GpsData>>? InputUpdated;

        public TinkerforgeGps(string key) : base(key)
        {

        }
        public GpsData CurrentInput { get; private set; }

        internal override void UpdateUnderlyingDevice(Device device)
        {
            throw new NotImplementedException();
        }
    }
}
