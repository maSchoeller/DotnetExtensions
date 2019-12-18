using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using MaSchoeller.Extensions.Tinkerforge.Abstracts.Sensors;
using System;
using System.Collections.Generic;
using System.Text;
using Tinkerforge;

namespace MaSchoeller.Extensions.Tinkerforge.Hardware
{
    public class TinkerforgeJoystick : TinkerforgeHardware, IJoyStickSensor, IButtonSensor
    {
        public event EventHandler<SensorUpdatedEventArgs<(double X, double Y)>>? InputUpdated;
        public event EventHandler<SensorUpdatedEventArgs<bool>>? ButtonStateChanged;
        event EventHandler<SensorUpdatedEventArgs<bool>> ISensor<bool>.InputUpdated
        {
            add => ButtonStateChanged += value;
            remove => ButtonStateChanged -= value;
        }
        public TinkerforgeJoystick(string key) : base(key)
        {

        }

        public (double X, double Y) CurrentInput { get; private set; }
        public bool AsToggleButton { get; set; }
        public bool ButtonPressed { get; private set; }
        bool ISensor<bool>.CurrentInput => ButtonPressed;

        internal override void UpdateUnderlyingDevice(Device device)
        {
            throw new NotImplementedException();
        }
    }
}
