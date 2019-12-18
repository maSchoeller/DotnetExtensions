using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using Tinkerforge;

namespace MaSchoeller.Extensions.Tinkerforge.Internals
{
    public static class TinkerforgeFactory
    {

        public static Device CreateTinkerforge(DeviceType type, string uid, IPConnection connection) 
        {
            throw new NotImplementedException();
        }

        public static TinkerforgeHardware CreateHardware(DeviceType type)
        {
            throw new NotImplementedException();
        }

        public static int? GetDeviceVersion(Device? device)
        {
            return 1;
        }
        

        public static bool? DeviceHasCoPorzessor(Device? device)
        {
            return false;
        }
    }
}
