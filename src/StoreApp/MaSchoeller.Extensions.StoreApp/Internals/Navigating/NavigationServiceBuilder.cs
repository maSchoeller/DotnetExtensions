using MaSchoeller.Extensions.StoreApp.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.StoreApp.Internals.Navigating
{
    public class NavigationServiceBuilder : INavigationServiceBuilder
    {
        private Action<IServiceCollection> _dependecies;
        private Dictionary<string, (Type PageType, Type ViewModelType)> _bindings;

        public NavigationServiceBuilder()
        {
            _bindings = new Dictionary<string, (Type PageType, Type ViewModelType)>();
        }

        public void ConfigureRoute<TPage, TViewModel>(string route, ServiceLifetime lifetime  = ServiceLifetime.Singleton)
        {
            if (_bindings.ContainsKey(route))
            {
                //Todo: add Exception message
                throw new ArgumentException();
            }
            _bindings.Add(route, (typeof(TPage), typeof(TViewModel)));
            _dependecies += services =>
            {
                services.TryAdd(new ServiceDescriptor(typeof(TViewModel), typeof(TViewModel), lifetime));
            };
        }

        public void AddDependenciesToServiceCollection(IServiceCollection services) 
            => _dependecies?.Invoke(services);

        public INavigationService Build(IServiceProvider provider)
        {
            return new NavigationService();
        }

    }
}
