using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public interface IDesktopBuilder
    {
        void ConfigureServices(Action<IServiceCollection> services);
        void ConfigureApplication(Action<Application> application);
        void UseStartup<TStartup>();

        void Build();
    }
}
