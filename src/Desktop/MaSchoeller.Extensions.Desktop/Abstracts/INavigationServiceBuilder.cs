using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public interface INavigationServiceBuilder
    {
        void AddRoute<TPage, TViewModel>(string route, ServiceLifetime lifetime = ServiceLifetime.Singleton);
        void AddDepedenciesToServiceCollection(IServiceCollection services);
        INavigationService Build(IServiceProvider provider);
    }
}
