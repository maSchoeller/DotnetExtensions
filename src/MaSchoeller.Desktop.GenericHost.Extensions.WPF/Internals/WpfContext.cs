using MaSchoeller.Desktop.GenericHost.Extensions.WPF.Abstracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF.Internals
{
    internal class WpfContext : IWpfContext
    {

        public WpfContext(IHostApplicationLifetime lifetime, IOptionsMonitor<WpfLaunchOptions>? options)
        {
            Lifetime = lifetime ?? throw new System.ArgumentNullException(nameof(lifetime));
            ShutdownMode = options?.CurrentValue.ShutdownMode ?? ShutdownMode.OnMainWindowClose;
        }

        public ShutdownMode ShutdownMode { get; set; }

        public IHostApplicationLifetime Lifetime { get; }

        public bool IsRunning { get; set; }

        public Application? WpfApplication { get; set; }

        public Dispatcher? Dispatcher => WpfApplication?.Dispatcher;
    }
}