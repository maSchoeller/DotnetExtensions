using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public interface IDesktopContext
    {
        ShutdownMode ShutdownMode { get; }

        IHostApplicationLifetime Lifetime { get; }

        bool IsRunning { get; }

        Application? WpfApplication { get; }

        Dispatcher? Dispatcher { get; }
    }
}
