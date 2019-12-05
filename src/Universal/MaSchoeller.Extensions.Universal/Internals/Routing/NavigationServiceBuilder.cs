using MaSchoeller.Extensions.Universal.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MaSchoeller.Extensions.Universal.Internals.Routing
{
    public class NavigationServiceBuilder : INavigationServiceBuilder
    {
        private Action<IServiceCollection> _dependecies;
        private readonly Dictionary<string, (Type ViewModelType, Type PageType)> _bindings;

        public NavigationServiceBuilder()
        {
            _bindings = new Dictionary<string, (Type,Type)>();
        }

        public void ConfigureRoute<TPage, TViewModel>(string route, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TPage : Page
            where TViewModel : IRoutable
        {
            if (_bindings.ContainsKey(route))
            {
                //Todo: add Exception message
                throw new ArgumentException();
            }
            _bindings.Add(route, (typeof(TViewModel), typeof(TPage)));
            _dependecies += services =>
            {
                services.TryAdd(new ServiceDescriptor(typeof(TViewModel), typeof(TViewModel), lifetime));
            };
        }

        public void AddDependenciesToServiceCollection(IServiceCollection services)
            => _dependecies?.Invoke(services);

        public INavigationService Build(IServiceProvider provider)
            => ActivatorUtilities.CreateInstance<NavigationService>(provider, _bindings);

    }
}
