using MaSchoeller.Extensions.Desktop.Abstracts;
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
        private readonly IServiceProvider _provider;
        private readonly Action<Application>? _configureApp;
        private Application? _application;

        public DesktopInitializerHost(
            DesktopContext context,
            IHostApplicationLifetime lifetime,
            IServiceProvider provider,
            Action<Application>? configureApp = null)
        {
            //_options = options?.CurrentValue ?? throw new ArgumentNullException(nameof(options));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            configureApp += (Application a) =>
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
                .InvokeAsync(() =>_configureApp?.Invoke(_application));
            _context.IsRunning = true;
            _application.Dispatcher.Invoke(() =>
            {
                if (_provider.GetRequiredService<IDesktopShell>() is Window windowShell)
                {
                    _application.MainWindow = windowShell;
                    windowShell.Show();
                }
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
