using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Internals
{
    public struct TinkerforgeDeviceInfo
    {

        public TinkerforgeDeviceInfo(
            string uid,
            string connectedUid,
            char position,
            short[] hardwareVersion,
            short[] firmwareVersion,
            int type) : this(
                uid, 
                connectedUid, 
                ConvertPosition(position),
                hardwareVersion,
                firmwareVersion,
                (DeviceType)type)
        { }

        public TinkerforgeDeviceInfo(
            string uid,
            string connectedUid,
            Position position,
            short[] hardwareVersion,
            short[] firmwareVersion,
            DeviceType type)
        {
            Uid = uid;
            ConnectedUid = connectedUid;
            Position = position;
            HardwareVersion = hardwareVersion;
            FirmwareVersion = firmwareVersion;
            Type = type;
        }

        public string Uid { get; }
        public string ConnectedUid { get; }
        public Position Position { get; }
        public short[] HardwareVersion { get; }
        public short[] FirmwareVersion { get; }
        public DeviceType Type { get; }

        public static Position ConvertPosition(char pos)
        {
            Position retPos;
            switch (pos)
            {
                case 'a':
                    retPos = Position.A;
                    break;
                case 'b':
                    retPos = Position.B;
                    break;
                case 'c':
                    retPos = Position.C;
                    break;
                case 'd':
                    retPos = Position.D;
                    break;
                case 'e':
                    retPos = Position.E;
                    break;
                case 'f':
                    retPos = Position.F;
                    break;
                case 'g':
                    retPos = Position.G;
                    break;
                case 'h':
                    retPos = Position.H;
                    break;
                case 'i':
                    retPos = Position.RaspberryHat;
                    break;
                case 'z':
                    retPos = Position.Isolator;
                    break;
                case '0':
                    retPos = Position.Stack0;
                    break;
                case '1':
                    retPos = Position.Stack1;
                    break;
                case '2':
                    retPos = Position.Stack2;
                    break;
                case '3':
                    retPos = Position.Stack3;
                    break;
                case '4':
                    retPos = Position.Stack4;
                    break;
                case '5':
                    retPos = Position.Stack5;
                    break;
                case '6':
                    retPos = Position.Stack6;
                    break;
                case '7':
                    retPos = Position.Stack7;
                    break;
                default:
                    retPos = Position.Unkown;
                    break;
            }
            return retPos;
        }
    }

    public enum Position
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        Stack0,
        Stack1,
        Stack2,
        Stack3,
        Stack4,
        Stack5,
        Stack6,
        Stack7,
        RaspberryHat,
        Isolator,
        Unkown,
    }
}
