using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public static class DesktopBuilderExtensions
    {
        public static void ConfigureApplication(this IDesktopBuilder builder, Action<Application, IHostEnvironment> configure)
            => builder.ConfigureApplication((a, h, _) => configure(a, h));
        public static void ConfigureApplication(this IDesktopBuilder builder, Action<Application, IConfiguration> configure)
            => builder.ConfigureApplication((a, _, c) => configure(a, c));
        public static void ConfigureApplication(this IDesktopBuilder builder, Action<Application> configure)
            => builder.ConfigureApplication((a, __, _) => configure(a));


        public static void ConfigureServices(this IDesktopBuilder builder, Action<IServiceCollection, IHostEnvironment> configure)
           => builder.ConfigureServices((s, h, _) => configure(s, h));
        public static void ConfigureServices(this IDesktopBuilder builder, Action<IServiceCollection, IConfiguration> configure)
            => builder.ConfigureServices((s, _, c) => configure(s, c));
        public static void ConfigureServices(this IDesktopBuilder builder, Action<IServiceCollection> configure)
            => builder.ConfigureServices((s, __, _) => configure(s));


        public static void ConfigureNavigation(this IDesktopBuilder builder, Action<INavigationServiceBuilder, IConfiguration> configure)
           => builder.ConfigureNavigation((s, _, c) => configure(s, c));
        public static void ConfigureNavigation(this IDesktopBuilder builder, Action<INavigationServiceBuilder, IHostEnvironment> configure)
            => builder.ConfigureNavigation((s, h, _) => configure(s, h));
        public static void ConfigureNavigation(this IDesktopBuilder builder, Action<INavigationServiceBuilder> configure)
            => builder.ConfigureNavigation((s, __, _) => configure(s));

    }
}
