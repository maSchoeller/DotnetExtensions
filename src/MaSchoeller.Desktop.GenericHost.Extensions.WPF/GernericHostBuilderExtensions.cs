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
                    var window = new TSplashScreen();
                    builder.ConfigureServices(c =>
                    {
                        c.AddSingleton<ISplashscreenWindow>(window);
                    });
                    app.Run(window);
                });
                thread.SetApartmentState(ApartmentState.STA);
                //thread.Priority = ThreadPriority.Highest;
                thread.Start();
                builder.ConfigureServices((c, s) => s.AddHostedService<SplashScreenHost>());
            }
            else
            {
                var app = Application.Current;
                app.Dispatcher.Invoke(() =>
                {
                    var window = new TSplashScreen();
                    window.Show();
                    builder.ConfigureServices(c =>
                    {
                        c.AddSingleton<ISplashscreenWindow>(window);
                        builder.ConfigureServices((c, s) => s.AddHostedService<SplashScreenHost>());
                    });
                });
            }
            Task.Delay(200).Wait(); //Wait small time to register the splashscreen
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


        public static async Task RunWpfAsync(this IHostBuilder builder)
        {
            var host = builder
                 .Build();
            await host.RunAsync();
        }
    }
}
