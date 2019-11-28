using MaSchoeller.Desktop.GenericHost.Extensions.WPF.Abstracts;
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

        public static IHostBuilder UseSplashScreen<TSplashScreen>(this IHostBuilder builder)
            where TSplashScreen : Window, ISplashscreenWindow, new()
        {
            if (Application.Current is null)
            {
                var thread = new Thread(() =>
                {
                    var app = new Application
                    {
                        ShutdownMode = ShutdownMode.OnMainWindowClose
                    };
                    app.Run();
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            Task.Delay(200).Wait(); //Wait small time to run the app.
            var app = Application.Current!;
            app.Dispatcher.InvokeAsync(() =>
            {
                var window = new TSplashScreen();
                window.Show();
                builder.ConfigureServices(c =>
                {
                    c.AddSingleton<ISplashscreenWindow>(window);
                    c.AddHostedService<SplashScreenHost>();
                });
            }).Wait();
            return builder;
        }

        public static IHostBuilder ConfigureWpfDefaults<TShellWindow>(this IHostBuilder builder, Action<IWpfBuilder> action)
            where TShellWindow : Window, IWpfShell
        {
            var wpfbuilder = new WpfBuilder<TShellWindow>(builder);
            action?.Invoke(wpfbuilder);
            wpfbuilder.Build();
            return builder;
        }

        public static IHostBuilder ConfigureWpfDefaults(this IHostBuilder builder, Action<IWpfBuilder> action)
        {
            var wpfbuilder = new WpfBuilder(builder);
            action?.Invoke(wpfbuilder);
            wpfbuilder.Build();
            return builder;
        }
    }
}
