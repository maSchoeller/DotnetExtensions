using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Tinkerforge
{
    public class TinkerforgeHardwareManager : IHardwareManager
    { 
        public event EventHandler<DisconnectedEventArgs> Disconnected;
        public event EventHandler<ConnectedEventArgs> Connected;

        public TinkerforgeHardwareManager()
        {

        }

        public bool IsConnected => throw new NotImplementedException();
        public IHardware this[string id] => throw new NotImplementedException();

        public Task ConnectAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public Task DisconnectAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IHardware> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public THardware GetHardware<THardware>(string id = null) where THardware : IHardware
        {
            throw new NotImplementedException();
        }

        public IHardware GetHardware(string id)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
