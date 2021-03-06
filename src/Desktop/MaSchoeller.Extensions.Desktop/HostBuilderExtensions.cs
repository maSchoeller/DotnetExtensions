﻿using System;
using System.Windows;

using Autofac.Extensions.DependencyInjection;

using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Internals.Hosting;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MaSchoeller.Extensions.Desktop
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder ConfigureSplashscreen<TSplashscreen>(this IHostBuilder builder)
            where TSplashscreen : Window, ISplashscreenWindow, new()
        {
            var app = ApplicationBuilder
                .CreateIfNotExistsAsync(ShutdownMode.OnExplicitShutdown)
                .GetAwaiter().GetResult();

            app.Dispatcher.InvokeAsync(() =>
            {
                var window = new TSplashscreen();
                window.Show();
                builder.ConfigureServices((context, services) =>
                {
                    services.AddSingleton<ISplashscreenWindow>(window);
                    services.AddHostedService<SplashscreenLifetimeHost>();
                });
            }).Task.GetAwaiter().GetResult();
            return builder;
        }

        public static IHostBuilder ConfigureDesktopDefaults<TShellWindow>(this IHostBuilder builder, Action<IDesktopBuilder>? configure = null, bool enableNavigation = true)
            where TShellWindow : Window, IDesktopShell
        {
            var wpfbuilder = new DesktopBuilder<TShellWindow>(builder);
            configure?.Invoke(wpfbuilder);
            wpfbuilder.Build(enableNavigation);
            return builder;
        }

        public static IHostBuilder ConfigureDesktopDefaults(this IHostBuilder builder, Action<IDesktopBuilder>? configure = null, bool enableNaviagtion = true)
        {
            var wpfbuilder = new DesktopBuilder(builder);
            configure?.Invoke(wpfbuilder);
            wpfbuilder.Build(enableNaviagtion);
            return builder;
        }

        public static IHostBuilder UseAutoFac(this IHostBuilder builder)
        {
            builder.Properties[DesktopBuilder.UseAutoFacPropertyName] = true;
            builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            return builder;
        }
        public static IHostBuilder UseMVVMC(this IHostBuilder builder)
        {
            builder.Properties[DesktopBuilder.UseMVVMCpropertyName] = true;
            return builder;
        }
    }
}
