using MaSchoeller.Desktop.GenericHost.Extensions.WPF.Internals;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF
{
    public static class GernericHostBuilderExtensions
    {
        public static IHostBuilder ConfigureWpfDefaults<TShellWindow>(this IHostBuilder builder, Action<IWpfBuilder> action)
            where TShellWindow : Window, IWpfShell
        {
            var wpfbuilder = new WpfBuilder<TShellWindow>(builder);
            action?.Invoke(wpfbuilder);
            wpfbuilder.Build();
            return builder;
        }

        public static async Task RunWpfAsync(this IHostBuilder builder)
        {
            var host = builder
                 .Build();

            await host.RunAsync();
        }
    }
}
