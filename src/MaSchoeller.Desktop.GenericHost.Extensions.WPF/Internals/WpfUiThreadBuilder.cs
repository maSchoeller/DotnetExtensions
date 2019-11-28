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
    internal class WpfUiThreadBuilder : IHostedService
    {
        private readonly WpfLaunchOptions _options;
        private readonly WpfContext _context;
        private readonly IServiceProvider _provider;
        private readonly Action<Application>? _configureApp;
        private Application? _application;

        public WpfUiThreadBuilder(
            WpfContext context,
            IOptionsMonitor<WpfLaunchOptions> options,
            IHostApplicationLifetime lifetime,
            IServiceProvider provider,
            Action<Application>? configureApp = null)
        {
            _options = options?.CurrentValue ?? throw new ArgumentNullException(nameof(options));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _configureApp = configureApp;
            _configureApp += (Application a) =>
            {
                a.Exit += (s, e) =>
                {
                    _context.IsRunning = false;
                    lifetime.StopApplication();
                };
            };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var finished = false;
            var uiThread = new Thread(CreateWpfApplication)
            {
                IsBackground = true
            };
            uiThread.SetApartmentState(ApartmentState.STA);


            uiThread.Start(new Action(Callback));
            void Callback()
            {
                _application = new Application
                {
                    ShutdownMode = _options.ShutdownMode
                };
                _configureApp?.Invoke(_application);

                _context.WpfApplication = _application;
                _context.IsRunning = true;
                finished = true; // Set finished before call app.run otherwise startup will not finish.
                if (_provider.GetRequiredService<IWpfShell>() is Window windowShell)
                {
                    _application.MainWindow = windowShell;
                    _application.Run(windowShell); //Thread is Blocking
                }
                else
                {
                    _application.Run(); //Thread is Blocking
                }
            };
            //Blocking until the callback is finished.
            while (!finished && !cancellationToken.IsCancellationRequested) await Task.Delay(10);
            await Task.Delay(50);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (!(_application is null))
            {
                if (_context.IsRunning)
                {
                    await _application.Dispatcher.InvokeAsync(() =>
                    {
                        _application.Shutdown();
                    });
                }
            }

        }

        private static void CreateWpfApplication(object? obj)
        {
            if (obj is Action callback)
            {
                SynchronizationContext.SetSynchronizationContext(
                    new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));
                callback();
            }
            else
            {
                throw new ArgumentException($"{nameof(obj)} must be type of{typeof(Action<Application>).Name}.", nameof(obj));
            }

        }
    }
}