using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts
{
    public interface IHardwareManager : IAsyncDisposable, IDisposable, IEnumerable<IHardware>
    {
        event EventHandler<DisconnectedEventArgs> Disconnected;
        event EventHandler<ConnectedEventArgs> Connected;

        bool IsConnected { get; }

        IHardware this[string id] { get; }
        THardware GetHardware<THardware>(string? id = null)
            where THardware : class, IHardware;

        IHardware GetHardware(string id);

        IEnumerable<THardware> GetHardwareList<THardware>()
            where THardware : class, IHardware;

        Task ConnectAsync(string host, int port, CancellationToken token);
        Task DisconnectAsync(CancellationToken token);
    }
}
