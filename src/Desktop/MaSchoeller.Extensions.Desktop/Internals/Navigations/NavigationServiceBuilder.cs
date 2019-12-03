using MaSchoeller.Extensions.Desktop.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace MaSchoeller.Extensions.Desktop.Internals.Navigations
{
    public class NavigationServiceBuilder : INavigationServiceBuilder
    {
        private readonly Dictionary<string, (Type ViewModel, Type Page)> _bindings;
        private readonly List<(Type ViewModelType, ServiceLifetime Lifetime)> _dependencies;

        public NavigationServiceBuilder()
        {
            _bindings = new Dictionary<string, (Type ViewModel, Type Page)>();
            _dependencies = new List<(Type ViewModelType, ServiceLifetime Lifetime)>();
        }

        public void AddDepedenciesToServiceCollection(IServiceCollection services)
        {
            services.TryAddEnumerable(
                    _dependencies.Select(
                        d => new ServiceDescriptor(typeof(IRoutable), d.ViewModelType, d.Lifetime)));
        }

        public void AddRoute<TPage, TViewModel>(string route, ServiceLifetime lifetime = ServiceLifetime.Singleton)
            where TPage : Page
            where TViewModel : IRoutable
        {
            if (_bindings.ContainsKey(route))
            {
                //Todo: add exception message.
                throw new ArgumentException();
            }
            _bindings.Add(route, (typeof(IRoutable), typeof(TPage)));
            _dependencies.Add((typeof(TViewModel), lifetime));
        }

        public INavigationService Build(IServiceProvider provider)
        {
            return new NavigationService(_bindings);
        }
    }
}
