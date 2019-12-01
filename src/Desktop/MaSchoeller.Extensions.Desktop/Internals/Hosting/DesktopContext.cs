using MaSchoeller.Extensions.Desktop.Abstracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace MaSchoeller.Extensions.Desktop.Internals.Hosting
{
    internal class DesktopContext : IDesktopContext
    {

        public DesktopContext(
            IHostApplicationLifetime lifetime
            /*, IOptionsMonitor<WpfLaunchOptions>? options*/)
        {
            Lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
            ShutdownMode = /*options?.CurrentValue.ShutdownMode ?? */ShutdownMode.OnMainWindowClose;
        }

        public ShutdownMode ShutdownMode { get; set; }

        public IHostApplicationLifetime Lifetime { get; }

        public bool IsRunning { get; set; }

        public Application? WpfApplication { get; set; }

        public Dispatcher? Dispatcher => WpfApplication?.Dispatcher;
    }
}
