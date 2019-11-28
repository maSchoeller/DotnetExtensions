using MaSchoeller.Desktop.GenericHost.Extensions.WPF.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF.Internals
{
    internal class WpfUiBuilder : IHostedService
    {
        private readonly WpfLaunchOptions _options;
        private readonly WpfContext _context;
        private readonly IServiceProvider _provider;
        private readonly Action<Application>? _configureApp;
        private Application? _application;

        public WpfUiBuilder(
            WpfContext context,
            IOptionsMonitor<WpfLaunchOptions> options,
            IHostApplicationLifetime lifetime,
            IServiceProvider provider,
            Action<Application>? configureApp = null)
        {
            _options = options?.CurrentValue ?? throw new ArgumentNullException(nameof(options));
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
            if (Application.Current is null)
            {
                var uiThread = new Thread(() =>
                {
                    SynchronizationContext.SetSynchronizationContext(
                        new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));
                    new Application
                    {
                        ShutdownMode = _options.ShutdownMode
                    };
                    Application.Current!.Run(); //Thread is Blocking
                });
                uiThread.IsBackground = true;
                uiThread.SetApartmentState(ApartmentState.STA);
                uiThread.Start();
            }

            await Task.Delay(200);
            _application = Application.Current!;
            _context.WpfApplication = _application;
            await _application.Dispatcher.InvokeAsync(() =>
            {
                _configureApp?.Invoke(_application);

            });
            _context.IsRunning = true;
            _application.Dispatcher.Invoke(() =>
            {
                if (_provider.GetRequiredService<IWpfShell>() is Window windowShell)
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