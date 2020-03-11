using MaSchoeller.Extensions.Desktop.Abstracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MaSchoeller.Extensions.Desktop.Internals.Hosting
{
    internal class DesktopInitializerHost : IHostedService
    {
        //private readonly WpfLaunchOptions _options;
        private readonly DesktopContext _context;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IServiceProvider _provider;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;
        private readonly Action<Application, IHostEnvironment, IConfiguration>? _configureApp;
        private Application? _application;

        public DesktopInitializerHost(
            DesktopContext context,
            IHostApplicationLifetime lifetime,
            IServiceProvider provider,
            IConfiguration configuration,
            IHostEnvironment environment,
            Action<Application, IHostEnvironment, IConfiguration>? configureApp = null)
        {
            //_options = options?.CurrentValue ?? throw new ArgumentNullException(nameof(options));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));


            configureApp += (a, _,__) =>
            {
                a.Exit += (s, e) =>
                {
                    _context.IsRunning = false;
                    if (!lifetime.ApplicationStopping.IsCancellationRequested)
                    {
                        lifetime.StopApplication();
                    }
                };
            };
            _configureApp = configureApp;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _application = await ApplicationBuilder
                .CreateIfNotExistsAsync(_context.ShutdownMode);
            _context.WpfApplication = _application;
            await _application.Dispatcher
                .InvokeAsync(() => _configureApp?.Invoke(_application,_environment,_configuration));
            _context.IsRunning = true;
            await _application.Dispatcher.InvokeAsync(() =>
            {
                if (_provider.GetService<IDesktopShell>() is Window windowShell)
                {
                    _application.MainWindow = windowShell;
                }
                _application.ShutdownMode = _context.ShutdownMode;
            });
            _lifetime.ApplicationStarted.Register(() =>
            {
                _application.Dispatcher.Invoke(() => _application.MainWindow?.Show());
            });
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (!(_application is null) && _context.IsRunning)
            {
                await _application.Dispatcher.InvokeAsync(() =>
                {
                    _application.Shutdown();
                });
            }

        }
    }
}
