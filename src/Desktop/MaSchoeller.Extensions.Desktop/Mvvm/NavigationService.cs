using MaSchoeller.Extensions.Desktop.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MaSchoeller.Extensions.Desktop.Mvvm
{
    public class NavigationService : NotifyPropertyChangedBase, INavigationService
    {
        public event EventHandler<NavigationEventArgs>? Navigated;
        public event EventHandler<NavigationEventArgs>? NavigationFailed;
        public static readonly string DefaultRoute = "home";

        private readonly IDictionary<string, (Type ViewModel, Type View)> _routes;
        private readonly IServiceProvider _provider;
        private readonly NavigationFrame _frame;

        private IServiceScope _currentServiceScope;

        public NavigationService(
            IServiceProvider provider,
            NavigationFrame frame,
            IHostApplicationLifetime lifetime,
            IDesktopContext context,
            IDictionary<string, (Type ViewModel, Type View)> routes)
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
                    ((INavigationService)this).NavigateTo(DefaultRoute);
                });
            });
        }

        public IRoutable CurrentRoute { get => GetProperty<IRoutable>(); private set => SetProperty(value); }


        public async Task NavigateToAsync(string route)
        {
            if (route is null)
            {
                throw new ArgumentNullException(nameof(route));
            }
            route = route.ToUpperInvariant();
            if (!_routes.ContainsKey(route))
            {
                //Todo: add exception message.
                throw new ArgumentException();
            }

            _currentServiceScope?.Dispose();
            _currentServiceScope = _provider.CreateScope();
            var vm = _currentServiceScope
                        .ServiceProvider
                        .GetService(_routes[route].ViewModel) as IRoutable;
            if (vm is null)
            {
                //Todo: add Exception message
                throw new InvalidCastException();
            }
            bool succeeded = false;
            if (!(ActivatorUtilities.CreateInstance(_currentServiceScope.ServiceProvider, _routes[route].View) is Page view))
            {
                //Todo: add Exception message;
                throw new InvalidCastException();
            }
            view.DataContext = vm;
            succeeded = _frame.Navigate(view);
            if (succeeded)
            {
                await (CurrentRoute?.LeaveAsync() ?? Task.CompletedTask);
                CurrentRoute = vm;
                await CurrentRoute.EnterAsync();
                Navigated?.Invoke(this, new NavigationEventArgs(route));
            }
            else
            {
                NavigationFailed?.Invoke(this, new NavigationEventArgs(route));
            }
        }
    }
}
