using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.StoreApp.Abstracts
{
    public interface INavigationServiceBuilder
    {
        void ConfigureRoute<TPage, TViewModel>(string route, ServiceLifetime lifetime = ServiceLifetime.Singleton);
        void AddDependenciesToServiceCollection(IServiceCollection services);
        INavigationService Build(IServiceProvider provider);
    }
}
