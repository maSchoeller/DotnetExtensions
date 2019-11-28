using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF.Internals
{
    internal class WpfBuilder<TShellWindow> : IWpfBuilder
        where TShellWindow : Window, IWpfShell
    {
        private readonly IHostBuilder _hostBuilder;
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

        public void Build()
        {
            _configureServices += AddBasicServices;
            _configureApplication += a => { };
            _hostBuilder.ConfigureServices((c, s) =>
            {
                _configureServices?.Invoke(s);
                s.AddHostedService(p
                    => ActivatorUtilities.CreateInstance<WpfUiThreadBuilder>(p, _configureApplication));
            });
        }


        private void AddBasicServices(IServiceCollection services)
        {
            services.AddSingleton<WpfContext>();
            services.AddSingleton<IWpfContext>(p => p.GetService<WpfContext>());
            services.AddSingleton<IWpfShell, TShellWindow>();
            services.AddOptions<WpfLaunchOptions>();
        }

    }
}
