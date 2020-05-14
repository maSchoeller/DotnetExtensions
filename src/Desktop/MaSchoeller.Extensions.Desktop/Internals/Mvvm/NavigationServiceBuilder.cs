using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Internals.Mvvm;
using MaSchoeller.Extensions.Desktop.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MaSchoeller.Extensions.Desktop.Internals.Mvvm
{
    internal class NavigationServiceBuilder : INavigationServiceBuilder
    {
        private readonly IDictionary<string, (Type ViewModel, Type Page)> _bindings;
        private Action<IServiceCollection>? _dependencies;

        public NavigationServiceBuilder()
        {
            _bindings = new Dictionary<string, (Type ViewModel, Type Page)>();
            //_dependencies = new List<(Type ViewModelType, ServiceLifetime Lifetime)>();
        }

        public void AddDepedenciesToServiceCollection(IServiceCollection services)
        {
            _dependencies?.Invoke(services);
        }

        public void AddRoute<TPage, TViewModel>(string route, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TPage : Page
            where TViewModel : IRoutable
        {
            if (route is null)
            {
                throw new ArgumentNullException(nameof(route));
            }
            route = route.ToUpperInvariant();
            if (_bindings.ContainsKey(route))
            {
                throw new ArgumentException($"There is already a route defined '{route}' can't register this route again.");
            }
            _bindings.Add(route, (typeof(TViewModel), typeof(TPage)));
            _dependencies += services => services.TryAdd(ServiceDescriptor.Describe(typeof(TViewModel), typeof(TViewModel), lifetime));
        }

        public INavigationService Build(IServiceProvider provider)
            => ActivatorUtilities.CreateInstance<NavigationService>(provider, _bindings);
    }
}
