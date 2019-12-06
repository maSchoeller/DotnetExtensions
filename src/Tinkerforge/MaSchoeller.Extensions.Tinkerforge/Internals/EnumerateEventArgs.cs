using System;

namespace MaSchoeller.Extensions.Tinkerforge.Internals
{
    internal class EnumerateEventArgs : EventArgs
    {
        public EnumerateEventArgs(TinkerforgeDeviceInfo deviceInfo,int enumerationType)
            : this(deviceInfo,(EnumerationType)enumerationType)
        {
        }

        public EnumerateEventArgs(TinkerforgeDeviceInfo deviceInfo, EnumerationType enumerationType)
        {
            DeviceInfo = deviceInfo;
            EnumerationType = enumerationType;
        }

        public TinkerforgeDeviceInfo DeviceInfo { get; }
        public EnumerationType EnumerationType { get; }
    }

    internal enum EnumerationType
    {
        Available = 0,
        Connected = 1,
        Disconnected = 2
    }
}