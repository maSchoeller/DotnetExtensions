using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts
{
    public interface IHardwareManager : IDisposable, IEnumerable<IHardware>
    {
        event EventHandler<DisconnectedEventArgs> Disconnected;
        event EventHandler<ConnectedEventArgs> Connected;

        bool IsConnected { get; }

        IHardware this[string id] { get; }
        THardware GetHardware<THardware>(string id = null)
            where THardware : IHardware;
        IHardware GetHardware(string id);

        Task ConnectAsync(CancellationToken token);
        Task DisconnectAsync(CancellationToken token);
    }
}
