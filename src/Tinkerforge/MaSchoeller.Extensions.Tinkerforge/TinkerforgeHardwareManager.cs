using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using MaSchoeller.Extensions.Tinkerforge.Internals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Tinkerforge
{
    public class TinkerforgeHardwareManager : IHardwareManager
    {
        public event EventHandler<DisconnectedEventArgs>? Disconnected;
        public event EventHandler<ConnectedEventArgs>? Connected;

        private ConnectionHandler? _activeConnectionHandler;
        private readonly Dictionary<string, IHardware> _hardware;

        public TinkerforgeHardwareManager()
        {
            _hardware = new Dictionary<string, IHardware>();
        }

        public bool IsConnected => _activeConnectionHandler?.IsConnceted ?? false;
        public IHardware this[string id] => _hardware[id];

        public async Task ConnectAsync(string host, int port, CancellationToken token)
        {
            if (_activeConnectionHandler is null)
            {
                _activeConnectionHandler = new ConnectionHandler(host, port);
                _activeConnectionHandler.Enumerate += (s, e) =>
                {
                    var hardware = TinkerforgeFactory.Create(e.DeviceInfo.Type, e.DeviceInfo.Uid, _activeConnectionHandler.Connection);
                    //Todo: maybe get options for mapping uid to a meaningful key.
                    var success = _hardware.TryAdd(e.DeviceInfo.Uid, hardware);
                    if (!success)
                    {
                        //Todo: add exception message
                        throw new InvalidOperationException();
                    }
                };
                await _activeConnectionHandler.ConnectAsync(token)
                    .ConfigureAwait(false);
            }
            else
            {
                //Todo: add exception message
                throw new InvalidOperationException();
            }
        }

        public async Task DisconnectAsync(CancellationToken token)
        {
            if (!(_activeConnectionHandler is null))
            {
                await _activeConnectionHandler.DisconnectAsync(token)
                    .ConfigureAwait(false);
                await Task.Delay(1000).ConfigureAwait(false);//Wait a small time to get enough time for registering the hardware.
            }
            else
            {
                //Todo: add exception message
                throw new InvalidOperationException();
            }
        }


        public IEnumerator<IHardware> GetEnumerator()
            => _hardware.Values.GetEnumerator();

        public THardware GetHardware<THardware>(string? id = null)
            where THardware : class, IHardware
        {
            THardware? hardware;
            Type? excpectedType;
            if (!(id is null))
            {
                var h = GetHardware(id);
                excpectedType = h.GetType();
                hardware = h as THardware;
            }
            hardware = _hardware.Values.FirstOrDefault(h => h.GetType() == typeof(THardware)) as THardware;
            return hardware ?? throw new InvalidCastException(); // Todo: add exception message
        }

        public IHardware GetHardware(string id)
        {
            if (_hardware.ContainsKey(id))
            {
                return _hardware[id];
            }
            else
            {
                //Todo add Exception message
                throw new KeyNotFoundException(); 
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public async ValueTask DisposeAsync()
        {
            await (_activeConnectionHandler?.DisposeAsync() ?? new ValueTask());
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                _activeConnectionHandler?.Dispose();
                disposedValue = true;
            }
        }
        ~TinkerforgeHardwareManager()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
