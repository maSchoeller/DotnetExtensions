using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Internals.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MaSchoeller.Extensions.Desktop
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureSplashscreen<TSplashscreen>(this IHostBuilder builder)
            where TSplashscreen : Window, ISplashscreenWindow, new()
        {
            var app = ApplicationBuilder
                .CreateIfNotExistsAsync(ShutdownMode.OnMainWindowClose)
                .Result;

            app.Dispatcher.InvokeAsync(() =>
            {
                var window = new TSplashscreen();
                window.Show();
                builder.ConfigureServices((context,services) =>
                {
                    services.AddSingleton<ISplashscreenWindow>(window);
                    services.AddHostedService<SplashscreenLifetimeHost>();
                });
            }).Wait();
            return builder;
        }

        public static IHostBuilder ConfigureDesktopDefaults<TShellWindow>(this IHostBuilder builder, Action<IDesktopBuilder>? action = null)
            where TShellWindow : Window, IDesktopShell
        {
            var wpfbuilder = new DesktopBuilder<TShellWindow>(builder);
            action?.Invoke(wpfbuilder);
            wpfbuilder.Build();
            return builder;
        }

        public static IHostBuilder ConfigureDesktopDefaults(this IHostBuilder builder, Action<IDesktopBuilder>? action = null)
        {
            var wpfbuilder = new DesktopBuilder(builder);
            action?.Invoke(wpfbuilder);
            wpfbuilder.Build();
            return builder;
        }
    }
}
