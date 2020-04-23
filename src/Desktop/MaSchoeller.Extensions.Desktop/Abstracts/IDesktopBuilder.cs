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
        void ConfigureApplication(Action<Application, IHostEnvironment, IConfiguration> configure);
        void ConfigureNavigation(Action<INavigationServiceBuilder, IHostEnvironment, IConfiguration> configure);

        void UseStartup<TStartup>();
        void Build(bool enableNavigation = true);
    }
}
