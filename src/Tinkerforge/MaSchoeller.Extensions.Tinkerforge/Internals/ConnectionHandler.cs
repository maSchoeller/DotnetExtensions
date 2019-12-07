using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinkerforge;

namespace MaSchoeller.Extensions.Tinkerforge.Internals
{
    internal class ConnectionHandler
    {
        public event EventHandler<ConnectedEventArgs> Connected;
        public event EventHandler<DisconnectedEventArgs> Disconnected;
        public event EventHandler<EnumerateEventArgs> Enumerate;

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
                ConnectedReason reason;
                switch (e)
                {
                    case IPConnection.CONNECT_REASON_REQUEST:
                        reason = ConnectedReason.Requested;
                        break;
                    case IPConnection.CONNECT_REASON_AUTO_RECONNECT:
                        reason = ConnectedReason.Reconnect;
                        break;
                    default:
                        reason = ConnectedReason.Unknown;
                        break;
                }
                Connected?.Invoke(this, new ConnectedEventArgs(reason));
            };
            Connection.Disconnected += (s, e) =>
            {
                IsConnceted = false;
                DisconnectedReason reason;
                switch (e)
                {
                    case IPConnection.DISCONNECT_REASON_REQUEST:
                        reason = DisconnectedReason.RequestedFromSelf;
                        break;
                    case IPConnection.DISCONNECT_REASON_SHUTDOWN:
                        reason = DisconnectedReason.RequestedFromEndpoint;
                        break;
                    case IPConnection.DISCONNECT_REASON_ERROR:
                        reason = DisconnectedReason.Timeout;
                        break;
                    default:
                        reason = DisconnectedReason.Unknown;
                        break;
                }
                Disconnected?.Invoke(this, new DisconnectedEventArgs(reason));
            };
            Connection.EnumerateCallback += (s,uid,cu,p,hV,fV,dI,eT) =>
            {
                Enumerate?.Invoke(this, new EnumerateEventArgs(
                                new TinkerforgeDeviceInfo(uid, cu, p, hV, fV, dI),
                                eT));
            };
        }

        public bool IsConnceted { get; private set; }
        public string Host { get; }
        public int Port { get; }
        private IPConnection Connection { get; }

        public async Task ConnectAsync(CancellationToken token = default)
        {
            if (IsConnceted)
            {
                //Todo Add Exception Message
                throw new ArgumentException();
            }
            await Task.Run(() =>
            {
                Connection.Connect(Host,Port);
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
    }
}
