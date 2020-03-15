using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using MaSchoeller.Extensions.Tinkerforge.Hardware;
using System;
using System.Collections.Generic;
using System.Text;
using Tinkerforge;

namespace MaSchoeller.Extensions.Tinkerforge.Internals
{
    internal static class TinkerforgeFactory
    {

        public static Device CreateTinkerforgeDevice(DeviceType type, string uid, IPConnection connection)
        {
            return type switch
            {
                DeviceType.CANBrickletV2 => new BrickletCANV2(uid, connection),
                DeviceType.JoystickBricklet => new BrickletJoystick(uid, connection),
                DeviceType.IMUBrickV2 => new BrickIMUV2(uid, connection),
                _ => throw new System.NotSupportedException()
            };
        }

        public static TinkerforgeHardware CreateHardwareAbstraction(DeviceType type, string key)
        {
            return type switch
            {
                DeviceType.CANBrickletV2 => new TinkerforgeCan(key),
                DeviceType.IMUBrickV2 => new TinkerforgeImu(key),
                _ => throw new System.NotSupportedException()
            };
        }

        public static int GetDeviceVersion(Device device)
        {
            return 1;
        }


        public static bool DeviceHasCoPorzessor(Device device)
        {
            return false;
        }
    }
}
