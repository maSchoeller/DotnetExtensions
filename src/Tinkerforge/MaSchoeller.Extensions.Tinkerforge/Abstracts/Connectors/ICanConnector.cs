using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts
{
    public interface ICanConnector : ISensor<CanPackage>
    {
        bool WriteMessage(CanPackage package);

    }

    public class CanEventArgs : EventArgs
    {
        public CanEventArgs(CanPackage package)
        {
            Package = package;
        }

        public CanPackage Package { get; }
    }

    public readonly struct CanPackage : IEquatable<CanPackage>
    {
        public CanFrameType Type { get; }
        public long Identifier { get; }
        public byte[] Data { get; }

        public CanPackage(CanFrameType type, long identifier, byte[] data)
        {
            Type = type;
            Identifier = identifier;
            Data = data;
        }

        public static bool operator ==(CanPackage left, CanPackage right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CanPackage left, CanPackage right)
        {
            return !(left == right);
        }

        public static bool Equals(CanPackage left, CanPackage right)
        {
            return left == right;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public bool Equals(CanPackage other)
        {
            return Equals((object)other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this);
        }

    }

    public enum CanFrameType
    {
        StandartData = 0,
        StandartRemote = 1,
        ExtendedData = 2,
        ExtendedRemote = 3
    }
}
