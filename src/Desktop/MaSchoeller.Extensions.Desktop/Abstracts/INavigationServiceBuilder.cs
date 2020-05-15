using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public interface INavigationServiceBuilder
    {
        void AddRoute<TPage, TViewModel>(string route, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TPage : Page
            where TViewModel : IRoutable;

        void AddDepedenciesToServiceCollection(IServiceCollection services);
        INavigationService Build(IServiceProvider provider);
    }
}
