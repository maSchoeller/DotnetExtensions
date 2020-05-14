using Autofac;
using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Internals.Helpers;
using MaSchoeller.Extensions.Desktop.Internals.Mvvm;
using MaSchoeller.Extensions.Desktop.Internals.Mvvmc;
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
        public static string UseMVVMCpropertyName = "UseMVVMC";

        protected readonly IHostBuilder _hostBuilder;

        private Action<IServiceCollection, IHostEnvironment, IConfiguration>? _configureServices;
        private Action<Application, IHostEnvironment, IConfiguration>? _configureApplication;
        private Action<INavigationServiceBuilder, IHostEnvironment, IConfiguration>? _configureNavigation;
        private Action<ContainerBuilder, IHostEnvironment, IConfiguration>? _configureContainer;
        private Type? _startupType;

        public DesktopBuilder(IHostBuilder hostBuilder)
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
        public void ConfigureContainer(Action<ContainerBuilder, IHostEnvironment, IConfiguration> configure)
            => _configureContainer += configure;

        public virtual void Build(bool enableNavigation = true)
        {
            object? startup = null;
            if (!(_startupType is null))
            {
                startup = StartupClassResolver.CreateStartup(_startupType);
            }

            _hostBuilder.ConfigureContainer<ContainerBuilder>((context, builder) =>
            {
                if (!(startup is null))
                {
                    ConfigureContainer((c, e, conf) =>
                    {
                        StartupClassResolver.InvokeConfigureContainer(startup, c, context);
                    });
                }
                _configureContainer?.Invoke(builder, context.HostingEnvironment, context.Configuration);
            });

            _hostBuilder.ConfigureServices((context, services) =>
            {
                if (!(_startupType is null))
                {
                    if (!(startup is null))
                    {
                        this.ConfigureServices(c => StartupClassResolver.InvokeConfigureServices(startup, c, context));
                        this.ConfigureApplication(a => StartupClassResolver.InvokeConfigureApplication(startup, a, context));
                        if (enableNavigation)
                        {
                            this.ConfigureNavigation(nb => StartupClassResolver.InvokeConfigureNavigation(startup, nb, context));
                        }
                    }
                }
                if (enableNavigation)
                {
                    if (_hostBuilder.Properties.ContainsKey(UseMVVMCpropertyName) && (bool)_hostBuilder.Properties[UseMVVMCpropertyName])
                    {
                        var navigationBuilder = new MvvmcNavigationServiceBuilder();
                        _configureNavigation?.Invoke(navigationBuilder, context.HostingEnvironment, context.Configuration);
                        ((IDesktopBuilder)this).ConfigureServices(services =>
                        {
                            navigationBuilder.AddDepedenciesToServiceCollection(services);
                            services.TryAddSingleton(p => navigationBuilder.Build(p));
                        });
                    }
                    else
                    {
                        var navigationBuilder = new NavigationServiceBuilder();
                        _configureNavigation?.Invoke(navigationBuilder, context.HostingEnvironment, context.Configuration);
                        ((IDesktopBuilder)this).ConfigureServices(services =>
                        {
                            navigationBuilder.AddDepedenciesToServiceCollection(services);
                            services.TryAddSingleton(p => navigationBuilder.Build(p));
                        });
                    }
                    services.TryAddSingleton<NavigationFrame>();
                }
                ((IDesktopBuilder)this).ConfigureServices(AddBasicServices);
                _configureServices?.Invoke(services, context.HostingEnvironment, context.Configuration);

                //Add a dummy callback, if the application callback was null.
                //It cause a Exception while creating the DesktopInitializerHost.
                _configureApplication += (_, __, ___) => { };
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
