using MaSchoeller.Desktop.GenericHost.Extensions.WPF.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF.Internals
{
    internal class WpfBuilder<TShellWindow> : WpfBuilder
        where TShellWindow : Window, IWpfShell
    {
        public WpfBuilder(IHostBuilder builder) : base(builder)
        {
        }

        protected override void AddBasicServices(IServiceCollection services)
        {
            base.AddBasicServices(services);
            services.AddSingleton<IWpfShell, TShellWindow>();
        }
    }

    internal class WpfBuilder : IWpfBuilder
    {
        protected readonly IHostBuilder _hostBuilder;
        private Action<IServiceCollection>? _configureServices;
        private Action<Application>? _configureApplication;
        private Type? _startupType;

        public WpfBuilder(IHostBuilder hostBuilder)
        {
            _hostBuilder = hostBuilder;
        }

        public void UseStartup<TStartup>()
            => _startupType = typeof(TStartup);

        public void ConfigureServices(Action<IServiceCollection> callback)
            => _configureServices += callback;

        public void ConfigureApplication(Action<Application> callback)
            => _configureApplication += callback;

        public virtual void Build()
        {
            _hostBuilder.ConfigureServices((context, services) =>
            {
                if (!(_startupType is null))
                {
                    var startup = StartupResolver.CreateStartup(_startupType, context);
                    if (!(startup is null))
                    {
                        ConfigureServices(c => StartupResolver.InvokeConfigureServices(startup, c, context));
                        ConfigureApplication(a => StartupResolver.InvokeConfigureApplication(startup, a, context));
                    }
                }
                ConfigureServices(AddBasicServices);
                _configureServices?.Invoke(services);
                services.AddHostedService(p
                    => ActivatorUtilities.CreateInstance<WpfUiBuilder>(p, _configureApplication));
            });
        }

        protected virtual void AddBasicServices(IServiceCollection services)
        {
            services.AddSingleton<WpfContext>();
            services.AddSingleton<IWpfContext>(p => p.GetService<WpfContext>());
            services.AddOptions<WpfLaunchOptions>();
        }

    }
}
