using MaSchoeller.Extensions.Desktop.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Controls;

namespace MaSchoeller.Extensions.Desktop.Internals.Mvvmc
{
    internal class MvvmcNavigationServiceBuilder : INavigationServiceBuilder
    {
        private readonly IDictionary<string, (Type ViewModel, Type Page)> _bindings;
        private Action<IServiceCollection>? _dependencies;

        public MvvmcNavigationServiceBuilder()
        {
            _bindings = new Dictionary<string, (Type ViewModel, Type Page)>();
        }

        public void AddDepedenciesToServiceCollection(IServiceCollection services)
        {
            _dependencies?.Invoke(services);
        }

        public void AddRoute<TPage, TController>(string route, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TPage : Page
            where TController : IRoutable
        {
            if (route is null)
            {
                throw new ArgumentNullException(nameof(route));
            }
            if (!typeof(IController).IsAssignableFrom(typeof(TController)))
            {
                throw new InvalidCastException($"Type '{nameof(TController)}' must implement '{typeof(IController).FullName}' for the use of the MVVMC Pattern.");
            }
            route = route.ToUpperInvariant();
            if (_bindings.ContainsKey(route))
            {
                throw new ArgumentException($"There is already a route defined '{route}' can't register this route again.");
            }
            _bindings.Add(route, (typeof(TController), typeof(TPage)));
            _dependencies += services => services.TryAdd(ServiceDescriptor.Describe(typeof(TController), typeof(TController), lifetime));
        }

        public INavigationService Build(IServiceProvider provider)
            => ActivatorUtilities.CreateInstance<MvvmcNavigationService>(provider, _bindings);
    }
}
