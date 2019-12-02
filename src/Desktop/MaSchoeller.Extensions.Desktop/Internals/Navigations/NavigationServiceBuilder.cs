using MaSchoeller.Extensions.Desktop.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Internals.Navigations
{
    public class NavigationServiceBuilder : INavigationServiceBuilder
    {
        public void AddDepedenciesToServiceCollection(IServiceCollection services)
        {
        }

        public void AddRoute<TPage, TViewModel>(string route, ServiceLifetime lifetime = ServiceLifetime.Singleton)
        {
        }

        public INavigationService Build(IServiceProvider provider)
        {
            return new NavigationService();
        }
    }
}
