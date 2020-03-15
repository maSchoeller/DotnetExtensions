using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
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
        private readonly byte[] _data;

        public byte this[int index] { get => _data[index]; }

        public CanPackage(CanFrameType type, long identifier, byte[] data)
        {
            Type = type;
            Identifier = identifier;
            _data = data;
        }

        internal byte[] GetData() => _data;

        public static bool operator ==(CanPackage left, CanPackage right) => left.Equals(right);
        public static bool operator !=(CanPackage left, CanPackage right) => !(left == right);
        public static bool Equals(CanPackage left, CanPackage right) => left == right;
        public override bool Equals(object obj) => base.Equals(obj);
        public bool Equals(CanPackage other) => Equals((object)other);
        public override int GetHashCode() => base.GetHashCode();

    }

    public enum CanFrameType
    {
        StandartData = 0,
        StandartRemote = 1,
        ExtendedData = 2,
        ExtendedRemote = 3
    }
}
