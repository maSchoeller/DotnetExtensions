using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts
{
    public class ConnectedEventArgs : EventArgs
    {
        public ConnectedEventArgs(ConnectedReason reason)
        {
            Reason = reason;
        }

        public ConnectedReason Reason { get; }
    }

    public enum ConnectedReason
    {
        Requested,
        Reconnect,
        Unknown
    }
}
