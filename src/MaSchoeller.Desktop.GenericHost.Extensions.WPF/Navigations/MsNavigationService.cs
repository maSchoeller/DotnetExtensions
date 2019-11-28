using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF.Navigations
{
    public class MsNavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDictionary<Type, Type> _bindings;
        private IServiceScope? _actualScope;

        public MsNavigationService(IServiceProvider serviceProvider, IDictionary<Type, Type> bindings)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _bindings = bindings ?? throw new ArgumentNullException(nameof(bindings));
        }


        public void Navigate<TViewModel>()
        {
            Navigate(typeof(TViewModel));
        }

        public void Navigate(Type viewModel)
        {
            if (!_bindings.ContainsKey(viewModel))
            {
                throw new ArgumentException($"");
            }
            var viewType = _bindings[viewModel];
            _actualScope?.Dispose();
            _actualScope = _serviceProvider.CreateScope();

            if (ActivatorUtilities.CreateInstance(_actualScope.ServiceProvider, viewType) is Page view)
            {
                view.DataContext = _actualScope
                                        .ServiceProvider
                                        .GetService(viewModel);
                CurrentView.Navigate(view);
            }
            else
            {
                //Todo: add exception message
                throw new InvalidCastException();
            }

        }

        public Frame CurrentView { get; private set; }
    }
}
