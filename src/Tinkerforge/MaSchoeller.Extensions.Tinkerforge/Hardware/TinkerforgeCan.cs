using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using Tinkerforge;

namespace MaSchoeller.Extensions.Tinkerforge.Hardware
{
    public class TinkerforgeCan : TinkerforgeHardware, ICanConnector
    {
        public event EventHandler<SensorUpdatedEventArgs<CanPackage>> InputUpdated;

        private BrickletCANV2  _canBricklet;

        public TinkerforgeCan(string key) : base(key)
        {

        }

        public CanPackage CurrentInput { get; private set; }

        public bool WriteMessage(CanPackage package) 
            => _canBricklet.WriteFrame((byte)package.Type, package.Identifier, package.GetData());

        internal override void UpdateUnderlyingDevice(Device device)
        {
            if (device is BrickletCANV2 bricklet)
            {
                _canBricklet.FrameReadCallback -= CanBricklet_FrameReadCallback;
                _canBricklet = bricklet;
                _canBricklet.FrameReadCallback += CanBricklet_FrameReadCallback;
            }
            else
            {
                //Todo: Add Exception message
                throw new ArgumentException();
            }
            base.UpdateUnderlyingDevice(device);
        }

        private void CanBricklet_FrameReadCallback(BrickletCANV2 sender, byte frameType, long identifier, byte[] data)
        {
            var can = new CanPackage((CanFrameType)frameType, identifier, data);
            CurrentInput = can;
            InputUpdated?.Invoke(this, new SensorUpdatedEventArgs<CanPackage>(can));
        }

    }
}
