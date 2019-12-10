using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Internals.Helpers;
using MaSchoeller.Extensions.Desktop.Internals.Navigations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;

namespace MaSchoeller.Extensions.Desktop.Internals.Hosting
{
    internal class DesktopBuilder<TShellWindow> : DesktopBuilder
       where TShellWindow : Window, IDesktopShell
    {
        public DesktopBuilder(IHostBuilder builder)
            : base(builder)
        {
        }

        protected override void AddBasicServices(IServiceCollection services)
        {
            base.AddBasicServices(services);
            services.AddSingleton<IDesktopShell, TShellWindow>();
        }
    }

    internal class DesktopBuilder : IDesktopBuilder
    {
        protected readonly IHostBuilder _hostBuilder;

        private Action<IServiceCollection, IHostEnvironment, IConfiguration>? _configureServices;
        private Action<Application, IHostEnvironment, IConfiguration>? _configureApplication;
        private Action<INavigationServiceBuilder, IHostEnvironment, IConfiguration>? _configureNavigation;
        private Type? _startupType;

        public DesktopBuilder(IHostBuilder hostBuilder, bool enableNavigation = true)
        {
            _hostBuilder = hostBuilder;
        }

        public void UseStartup<TStartup>()
            => _startupType = typeof(TStartup);

        public void ConfigureServices(Action<IServiceCollection, IHostEnvironment, IConfiguration> callback)
            => _configureServices += callback;

        public void ConfigureApplication(Action<Application, IHostEnvironment, IConfiguration> callback)
            => _configureApplication += callback;
        public void ConfigureNavigation(Action<INavigationServiceBuilder, IHostEnvironment, IConfiguration> configure)
            => _configureNavigation += configure;

        public virtual void Build(bool enableNavigation = true)
        {
            _hostBuilder.ConfigureServices((context, services) =>
            {
                if (!(_startupType is null))
                {
                    var startup = StartupClassResolver.CreateStartup(_startupType, context);
                    if (!(startup is null))
                    {
                        ((IDesktopBuilder)this).ConfigureServices(c => StartupClassResolver.InvokeConfigureServices(startup, c, context));
                        ((IDesktopBuilder)this).ConfigureApplication(a => StartupClassResolver.InvokeConfigureApplication(startup, a, context));
                        if (enableNavigation)
                        {
                            ((IDesktopBuilder)this).ConfigureNavigation(nb => StartupClassResolver.InvokeConfigureNavigation(startup, nb, context));
                        }
                    }
                }
                if (enableNavigation)
                {
                    var navigationBuilder = new NavigationServiceBuilder();
                    _configureNavigation?.Invoke(navigationBuilder, context.HostingEnvironment, context.Configuration);
                    ((IDesktopBuilder)this).ConfigureServices(services => 
                    {
                        navigationBuilder.AddDepedenciesToServiceCollection(services);
                        services.TryAddSingleton(p => navigationBuilder.Build(p));
                        services.TryAddSingleton<NavigationFrame>();
                    });
                }
                ((IDesktopBuilder)this).ConfigureServices(AddBasicServices);
                _configureServices?.Invoke(services, context.HostingEnvironment, context.Configuration);

                //Add a dummy callback, if the application callback was null.
                //It cause a Exception while creating the DesktopInitializerHost.
                _configureApplication += (_,__,___) => { };
                services.AddHostedService(p
                    => ActivatorUtilities.CreateInstance<DesktopInitializerHost>(p, _configureApplication));
            });
        }

        protected virtual void AddBasicServices(IServiceCollection services)
        {
            services.AddSingleton<DesktopContext>();
            services.AddSingleton<IDesktopContext>(p => p.GetService<DesktopContext>());
        }

    }
}
