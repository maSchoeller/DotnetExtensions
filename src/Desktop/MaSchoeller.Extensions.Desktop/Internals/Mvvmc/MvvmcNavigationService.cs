using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MaSchoeller.Extensions.Desktop.Internals.Mvvmc
{
    internal class MvvmcNavigationService : NotifyPropertyChangedBase, INavigationService
    {
        public event EventHandler<NavigationEventArgs>? Navigated;
        public event EventHandler<NavigationEventArgs>? NavigationFailed;

        private readonly IDictionary<string, (Type Controller, Type View)> _routes;
        private readonly IServiceProvider _provider;
        private readonly NavigationFrame _frame;

        private IServiceScope? _currentServiceScope;

        public MvvmcNavigationService(
            IServiceProvider provider,
            NavigationFrame frame,
            IHostApplicationLifetime lifetime,
            IDesktopContext context,
            IDictionary<string, (Type Controller, Type View)> routes)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _routes = routes ?? throw new ArgumentNullException(nameof(routes));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
            lifetime.ApplicationStarted.Register(() =>
            {
                context.Dispatcher!.Invoke(() =>
                {
                    this.NavigateTo(Navigation.DefaultRoute);
                });
            });
        }

        private string? _currentRouteName;
        private IRoutable _currentRoute = null!;
        public IRoutable CurrentRoute { get => _currentRoute; private set => SetProperty(ref _currentRoute, value); }


        public async Task<bool> TryNavigateToAsync(string route)
        {
            if (route is null)
            {
                throw new ArgumentNullException(nameof(route));
            }
            route = route.ToUpperInvariant();
            if (!_routes.ContainsKey(route))
            {
                throw new ArgumentException($"There is no Route register they contain the key: '{route}'.");
            }
            if (!(_currentRouteName is null) && _currentRouteName == route)
            {
                NavigationFailed?.Invoke(this, new NavigationEventArgs(route));
                return false;
            }
            _currentServiceScope?.Dispose();
            _currentServiceScope = _provider.CreateScope();
            IController? controller = _currentServiceScope
                        .ServiceProvider
                        .GetService(_routes[route].Controller) as IController;

            if (controller is null)
            {
                throw new InvalidCastException($"The Controller '{_routes[route].Controller.GetType().FullName}' does not implement '{typeof(IController).FullName}', can't able to navigate to this object, change from MVVMC to MVVM if you want only a ViewModel");
            }
            if (!(controller is IRoutable routable))
            {
                throw new InvalidCastException($"The Controller '{_routes[route].Controller.GetType().FullName}' does not implement '{typeof(IRoutable).FullName}', can't able to navigate to this object");
            }
            if (!(ActivatorUtilities.CreateInstance(_currentServiceScope.ServiceProvider, _routes[route].View) is Page view))
            {
                throw new InvalidCastException($"The related view '{_routes[route].View.GetType().FullName}' does not inheritance from '{typeof(Page).FullName}', can't able to navigate to this page..");
            }
            var canEnter = await routable.CanEnterRouteAsync();
            if (!canEnter)
            {
                return false;
            }
            view.DataContext = controller.Initialize();
            bool succeeded = _frame.Navigate(view);
            if (succeeded)
            {
                await (CurrentRoute?.LeaveAsync() ?? Task.CompletedTask);
                CurrentRoute = routable;
                _currentRouteName = route;
                await CurrentRoute.EnterAsync();
                Navigated?.Invoke(this, new NavigationEventArgs(route));
                return true;
            }
            else
            {
                NavigationFailed?.Invoke(this, new NavigationEventArgs(route));
                return false;
            }
        }
    }
}
