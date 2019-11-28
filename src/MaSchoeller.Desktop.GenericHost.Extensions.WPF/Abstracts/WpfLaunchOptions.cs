using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF.Abstracts
{
    public class WpfLaunchOptions
    {
        public ShutdownMode ShutdownMode { get; set; } = ShutdownMode.OnMainWindowClose;

    }
}
