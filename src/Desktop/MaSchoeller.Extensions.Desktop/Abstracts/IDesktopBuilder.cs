using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public interface IDesktopBuilder
    {
        void ConfigureServices(Action<IServiceCollection> configure);
        void ConfigureApplication(Action<Application> configure);
        void ConfigureNavigation(Action<INavigationServiceBuilder> configure);
        void UseStartup<TStartup>();
        void Build(bool enableNavigation = true);
    }
}
