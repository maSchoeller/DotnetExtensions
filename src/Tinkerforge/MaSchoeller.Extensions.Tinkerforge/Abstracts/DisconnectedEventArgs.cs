using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts
{
    public class DisconnectedEventArgs : EventArgs
    {
        public DisconnectedEventArgs(DisconnectedReason reason)
        {
            Reason = reason;
        }

        public DisconnectedReason Reason { get; }
    }

    public enum DisconnectedReason
    {
        Timeout,
        RequestedFromEndpoint,
        RequestedFromSelf, //Rename find matchable name.
        Unknown,
    }
}
