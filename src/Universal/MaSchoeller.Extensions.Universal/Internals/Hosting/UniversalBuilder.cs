using MaSchoeller.Extensions.Universal.Abstracts;
using MaSchoeller.Extensions.Universal.Helpers;
using MaSchoeller.Extensions.Universal.Internals.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MaSchoeller.Extensions.Universal.Internals.Hosting
{
    internal class UniversalBuilder<TShell, TViewModel> : UniversalBuilder
        where TShell : Page
    {
        public UniversalBuilder(IHostBuilder builder, bool enableNavigation)
            : base(builder, enableNavigation, typeof(TShell), typeof(TViewModel))
        {
        }
    }


    internal class UniversalBuilder : IUniversalBuilder
    {
        private readonly IHostBuilder _builder;
        private readonly bool _enableNavigation;
        private readonly Type _mainpage;
        private readonly Type _mainPageViewModelType;
        private Action<IServiceCollection> _configureServices;
        private Action<INavigationServiceBuilder> _configureNavigation;
        private Type _startupType;

        public UniversalBuilder(IHostBuilder builder, bool enableNavigation, Type mainpage, Type mainPageViewModel)
        {
            _builder = builder;
            _enableNavigation = enableNavigation;
            _mainpage = mainpage;
            _mainPageViewModelType = mainPageViewModel;
        }

        public void ConfigureNavigation(Action<INavigationServiceBuilder> configure)
            => _configureNavigation += configure;

        public void ConfigureServices(Action<IServiceCollection> configure)
            => _configureServices += configure;

        public void UseStartup<Startup>()
        {
            _startupType = typeof(Startup);
        }

        public virtual void Build()
        {
            _builder.ConfigureServices((context, services) =>
            {
                if (!(_startupType is null))
                {
                    object startup = StartupClassResolver.CreateStartup(_startupType, context);
                    ConfigureServices(s => StartupClassResolver.InvokeConfigureServices(startup, s, context));
                    ConfigureNavigation(b => StartupClassResolver.InvokeConfigureNavigation(startup, b, context));
                }
                ConfigureServices(AddBasicServices);
                _configureServices?.Invoke(services);
            });
        }

        private void AddBasicServices(IServiceCollection services)
        {
            services.AddHostedService(
                p => ActivatorUtilities.CreateInstance<UniversalInitializer>(p, _mainpage));
            services.TryAddSingleton(_mainPageViewModelType);
            services.TryAddSingleton(p => new MainPageContext()
            {
                Frame = p.GetService<NavigationFrame>(),
                ViewModel = ActivatorUtilities.CreateInstance(p, _mainPageViewModelType)
            });
            if (_enableNavigation)
            {
                services.TryAddSingleton<NavigationFrame>();
                var navigationbuilder = new NavigationServiceBuilder();
                _configureNavigation?.Invoke(navigationbuilder);
                navigationbuilder.AddDependenciesToServiceCollection(services);
                services.TryAddSingleton(p => navigationbuilder.Build(p));
            }
        }
    }
}
