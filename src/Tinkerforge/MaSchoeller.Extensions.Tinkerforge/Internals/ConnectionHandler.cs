using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinkerforge;

namespace MaSchoeller.Extensions.Tinkerforge.Internals
{
    internal class ConnectionHandler : IDisposable, IAsyncDisposable
    {
        public event EventHandler<ConnectedEventArgs>? Connected;
        public event EventHandler<DisconnectedEventArgs>? Disconnected;
        public event EventHandler<EnumerateEventArgs>? Enumerate;

        public ConnectionHandler(string host, int port)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
            Port = port;
            Connection = new IPConnection();
            Connection.SetAutoReconnect(true);
            Connection.Connected += (s, e) =>
            {
                IsConnceted = true;
                Connection.Enumerate();
                var reason = e switch
                {
                    IPConnection.CONNECT_REASON_REQUEST => ConnectedReason.Requested,
                    IPConnection.CONNECT_REASON_AUTO_RECONNECT => ConnectedReason.Reconnect,
                    _ => ConnectedReason.Unknown,
                };
                Connected?.Invoke(this, new ConnectedEventArgs(reason));
            };
            Connection.Disconnected += (s, e) =>
            {
                IsConnceted = false;
                var reason = e switch
                {
                    IPConnection.DISCONNECT_REASON_REQUEST => DisconnectedReason.RequestedFromSelf,
                    IPConnection.DISCONNECT_REASON_SHUTDOWN => DisconnectedReason.RequestedFromEndpoint,
                    IPConnection.DISCONNECT_REASON_ERROR => DisconnectedReason.Timeout,
                    _ => DisconnectedReason.Unknown,
                };
                Disconnected?.Invoke(this, new DisconnectedEventArgs(reason));
            };
            Connection.EnumerateCallback += (s, uid, cu, p, hV, fV, dI, eT) =>
            {
                Enumerate?.Invoke(this, new EnumerateEventArgs(
                                new TinkerforgeDeviceInfo(uid, cu, p, hV, fV, dI),
                                eT));
            };
        }

        public bool IsConnceted { get; private set; }
        public string Host { get; }
        public int Port { get; }
        public IPConnection Connection { get; }

        public async Task ConnectAsync(CancellationToken token = default)
        {
            if (IsConnceted)
            {
                //Todo Add Exception Message
                throw new ArgumentException();
            }
            await Task.Run(() =>
            {
                Connection.Connect(Host, Port);
                IsConnceted = true;
                Connected?.Invoke(this, new ConnectedEventArgs(ConnectedReason.Requested));
            }, token).ConfigureAwait(false);
        }

        public async Task DisconnectAsync(CancellationToken token = default)
        {
            if (!IsConnceted)
            {
                //Todo Add Exception Message
                throw new ArgumentException();
            }
            await Task.Run(() =>
            {
                Connection.Connect(Host, Port);
            }, token).ConfigureAwait(false);
        }

        public void Dispose() 
            => DisposeAsync().GetAwaiter().GetResult();

        public ValueTask DisposeAsync()
        {
            //Todo: implement disposepattern
            Connection.Disconnect();
            return new ValueTask();
        }
    }
}
