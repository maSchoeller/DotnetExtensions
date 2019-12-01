using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Internals.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaSchoeller.Extensions.Desktop.Internals.Hosting
{
    internal class DesktopBuilder<TShellWindow> : DesktopBuilder
       where TShellWindow : Window, IDesktopShell
    {
        public DesktopBuilder(IHostBuilder builder) : base(builder)
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
        private Action<IServiceCollection>? _configureServices;
        private Action<Application>? _configureApplication;
        private Type? _startupType;

        public DesktopBuilder(IHostBuilder hostBuilder)
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
                    var startup = StartupClassResolver.CreateStartup(_startupType, context);
                    if (!(startup is null))
                    {
                        ConfigureServices(c => StartupClassResolver.InvokeConfigureServices(startup, c, context));
                        ConfigureApplication(a => StartupClassResolver.InvokeConfigureApplication(startup, a, context));
                    }
                }
                ConfigureServices(AddBasicServices);
                _configureServices?.Invoke(services);
                services.AddHostedService(p
                    => ActivatorUtilities.CreateInstance<DesktopInitializerHost>(p));
            });
        }

        protected virtual void AddBasicServices(IServiceCollection services)
        {
            services.AddSingleton<DesktopContext>();
            services.AddSingleton<IDesktopContext>(p => p.GetService<DesktopContext>());
            //services.AddOptions<WpfLaunchOptions>();
        }

    }
}
