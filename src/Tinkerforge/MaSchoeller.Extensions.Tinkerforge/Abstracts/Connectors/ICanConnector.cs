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

    public struct CanPackage
    {

    }
}
