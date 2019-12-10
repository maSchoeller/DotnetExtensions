using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public interface IDesktopBuilder
    {
        void ConfigureServices(Action<IServiceCollection, IHostEnvironment, IConfiguration> configure);

        void ConfigureServices(Action<IServiceCollection, IHostEnvironment> configure)
            => ConfigureServices((s, h, _) => configure(s, h));
        void ConfigureServices(Action<IServiceCollection, IConfiguration> configure)
            => ConfigureServices((s, _, c) => configure(s, c));
        void ConfigureServices(Action<IServiceCollection> configure)
            => ConfigureServices((s, __, _) => configure(s));

        void ConfigureApplication(Action<Application, IHostEnvironment, IConfiguration> configure);

        void ConfigureApplication(Action<Application, IHostEnvironment> configure)
            => ConfigureApplication((a, h, _) => configure(a, h));
        void ConfigureApplication(Action<Application, IConfiguration> configure)
            => ConfigureApplication((a, _, c) => configure(a, c));
        void ConfigureApplication(Action<Application> configure)
            => ConfigureApplication((a, __, _) => configure(a));

        void ConfigureNavigation(Action<INavigationServiceBuilder, IHostEnvironment, IConfiguration> configure);

        void ConfigureNavigation(Action<INavigationServiceBuilder, IConfiguration> configure)
            => ConfigureNavigation((n, _, c) => configure(n, c));
        void ConfigureNavigation(Action<INavigationServiceBuilder, IHostEnvironment> configure)
            => ConfigureNavigation((n, h, _) => configure(n, h));
        void ConfigureNavigation(Action<INavigationServiceBuilder> configure)
            => ConfigureNavigation((n, __, _) => configure(n));

        void UseStartup<TStartup>();
        void Build(bool enableNavigation = true);
    }
}
