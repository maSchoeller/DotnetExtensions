using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using Tinkerforge;

namespace MaSchoeller.Extensions.Tinkerforge.Internals
{
    public static class TinkerforgeFactory
    {

        public static IHardware Create(DeviceType type, string uid, IPConnection connection) 
        {
            return (type) switch
            {
                //DeviceType.ServoBrick => new BrickServo(uid, connection), 
                //DeviceType.IMUBrickV2 => new BrickIMUV2(uid, connection), 
                //DeviceType.RGBLEDButtonBricklet => new BrickletRGBLEDButton(uid, connection),
                _ => default
            };
        }

        
    }
}
