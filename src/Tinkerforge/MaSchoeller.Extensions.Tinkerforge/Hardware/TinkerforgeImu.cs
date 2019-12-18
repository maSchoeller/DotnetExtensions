using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using MaSchoeller.Extensions.Tinkerforge.Abstracts.Sensors;
using System;
using System.Collections.Generic;
using System.Text;
using Tinkerforge;

namespace MaSchoeller.Extensions.Tinkerforge.Hardware
{
    public class TinkerforgeImu : TinkerforgeHardware, IImuSensor
    {
        public event EventHandler<SensorUpdatedEventArgs<ImuData>>? InputUpdated;


        public TinkerforgeImu(string key) : base(key)
        {

        }

        public ImuData CurrentInput { get; private set; }

        internal override void UpdateUnderlyingDevice(Device device)
        {

        }
    }
}
