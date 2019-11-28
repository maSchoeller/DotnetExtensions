using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF
{
    public interface IWpfContext
    {
        
        ShutdownMode ShutdownMode { get; }

        IHostApplicationLifetime Lifetime { get; }

        bool IsRunning { get; }
       
        Application? WpfApplication { get; }

        Dispatcher? Dispatcher { get; }
    }
}
