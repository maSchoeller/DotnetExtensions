using MaSchoeller.Extensions.Universal.Abstracts;
using MaSchoeller.Extensions.Universal.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MaSchoeller.Extensions.Universal.Internals.Routing
{
    public class NavigationService : NotifyPropertyChangedBase, INavigationService
    {
        public readonly static string DefaultRoute = "Home";

        private readonly IServiceProvider _provider;
        private readonly NavigationFrame _frame;
        private readonly IDictionary<string, (Type ViewModel, Type Page)> _bindings;

        private IServiceScope _currentServiceScope;

        public NavigationService(
            IServiceProvider provider,
            NavigationFrame frame,
            IHostApplicationLifetime lifetime,
            IDictionary<string, (Type ViewModel, Type Page)> bindings)
        {
            if (lifetime is null)
            {
                throw new ArgumentNullException(nameof(lifetime));
            }
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
            _bindings = bindings ?? throw new ArgumentNullException(nameof(bindings));
            lifetime.ApplicationStarted.Register(() =>
            {
                if (_bindings.ContainsKey(DefaultRoute))
                {
                    Navigate(DefaultRoute);
                }
            });
        }
        public IRoutable CurrentRoute 
        { 
            get => GetProperty<IRoutable>(); 
            private set => SetProperty(value); 
        }

        public bool Navigate(string route)
        {
            if (route is null)
            {
                throw new ArgumentNullException(nameof(route));
            }
            route = route.ToUpper(CultureInfo.InvariantCulture);
            if (!_bindings.ContainsKey(route))
            {
                //Todo: Add Exception message
                throw new ArgumentException();
            }
            var (viewModelType, pageType) = _bindings[route];
            CurrentRoute?.OnLeave();
            _currentServiceScope?.Dispose();
            _currentServiceScope = _provider.CreateScope();
            CurrentRoute = _currentServiceScope
                                .ServiceProvider
                                .GetRequiredService(viewModelType) as IRoutable;
            if (CurrentRoute is null)
            {
                //Todo: Add Exception message
                throw new InvalidCastException();
            }
            var ret = _frame.Navigate(pageType, CurrentRoute);
            CurrentRoute.OnEnter();
            return ret;
        }
    }
}
