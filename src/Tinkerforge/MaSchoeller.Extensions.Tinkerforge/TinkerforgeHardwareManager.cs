using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using MaSchoeller.Extensions.Tinkerforge.Internals;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tinkerforge;

namespace MaSchoeller.Extensions.Tinkerforge
{
    public class TinkerforgeHardwareManager : IHardwareManager
    {
        public event EventHandler<DisconnectedEventArgs>? Disconnected;
        public event EventHandler<ConnectedEventArgs>? Connected;

        private ConnectionHandler? _activeConnectionHandler;
        private readonly Dictionary<string, TinkerforgeHardware> _hardware;
        private readonly TinkerforgeHardwareManagerOptions _options;
        private readonly ILogger<TinkerforgeHardwareManager>? _logger;

        public TinkerforgeHardwareManager(IOptions<TinkerforgeHardwareManagerOptions> options,
            ILogger<TinkerforgeHardwareManager>? logger = null)
            : this(options?.Value ?? throw new ArgumentNullException(nameof(options)), logger)
        {
        }

        public TinkerforgeHardwareManager(TinkerforgeHardwareManagerOptions options,
            ILogger<TinkerforgeHardwareManager>? logger = null)
        {
            _hardware = new Dictionary<string, TinkerforgeHardware>();
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger;
        }

        public bool IsConnected => _activeConnectionHandler?.IsConnceted ?? false;

        public IEnumerable<string> Keys => _hardware.Keys;

        public IEnumerable<IHardware> Values => _hardware.Values;

        public int Count => _hardware.Count;

        public IHardware this[string id] => _hardware[id];

        public async Task ConnectAsync(CancellationToken token)
        {
            if (_activeConnectionHandler is null)
            {
                _activeConnectionHandler = new ConnectionHandler(_options.Host, _options.Port);
                _activeConnectionHandler.Enumerate += (s, e) =>
                {
                    Device tinker;
                    TinkerforgeHardware? hardware;
                    string key = e.DeviceInfo.Uid; //Todo: maybe get options for mapping uid to a meaningful key.
                    string uid = e.DeviceInfo.Uid;
                    switch (e.EnumerationType)
                    {
                        case EnumerationType.Available:
                            hardware = TinkerforgeFactory.CreateHardwareAbstraction(e.DeviceInfo.Type, key);
                            tinker = TinkerforgeFactory.CreateTinkerforgeDevice(e.DeviceInfo.Type, uid, _activeConnectionHandler.Connection);
                            hardware.UpdateUnderlyingDevice(tinker);
                            if (_hardware.ContainsKey(key))
                            {
                                //Todo: add exception message
                                throw new InvalidOperationException();
                            }
                            _hardware.Add(key, hardware);
                            break;
                        case EnumerationType.Connected:
                            tinker = TinkerforgeFactory.CreateTinkerforgeDevice(e.DeviceInfo.Type, uid, _activeConnectionHandler.Connection);
                            if (_hardware.TryGetValue(key, out hardware))
                            {
                                hardware.UpdateUnderlyingDevice(tinker);
                            }
                            else
                            {
                                //Todo: add exception message
                                throw new InvalidOperationException();
                            }
                            break;
                    }

                };
                await _activeConnectionHandler.ConnectAsync(token)
                    .ConfigureAwait(false);
                //await Task.Delay(_options.StartupDelay).ConfigureAwait(false);//Wait a small time to get enough time for registering the hardware.
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
            }
            else
            {
                //Todo: add exception message
                throw new InvalidOperationException();
            }
        }

        public IEnumerable<THardware> GetHardwareList<THardware>()
            where THardware : class, IHardware
        {
           
            return _hardware.Values
                .Select(h => h as THardware)
                .Where(h => !(h is null))
                .Select(h => h!); // Warning is not smart enough to recognize the where.
        }

        public THardware GetHardware<THardware>(string? id = null)
            where THardware : class, IHardware
        {
            THardware? hardware;
            if (!(id is null))
            {
                var h = GetHardware(id);
                hardware = h as THardware;
            }
            else
            {
                hardware = _hardware.Values.FirstOrDefault(h => typeof(THardware).IsAssignableFrom(h.GetType())) as THardware;
            }
            // Todo: add exception message
            return hardware ?? throw new InvalidCastException();
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

        public bool ContainsKey(string key) => _hardware.ContainsKey(key);

        public bool TryGetValue(string key, out IHardware value)
        {
            var ret = _hardware.TryGetValue(key,out var untypedValue);
            value = untypedValue;
            return ret;
        }

        public IEnumerator<KeyValuePair<string, IHardware>> GetEnumerator()
        {
            //Todo: it can be run in InvalidOperationException, if the collection was modified.
            //Know the problem but no solution, maybe some lock technic, but looks tricky.
            return _hardware
                .Select(kw => new KeyValuePair<string, IHardware>(kw.Key,kw.Value))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public ValueTask DisposeAsync()
        {
            return _activeConnectionHandler?.DisposeAsync() ?? new ValueTask();
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
                DisposeAsync()
                    .GetAwaiter()
                    .GetResult();
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
