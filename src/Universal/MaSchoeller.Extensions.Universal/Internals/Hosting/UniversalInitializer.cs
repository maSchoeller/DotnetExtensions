using MaSchoeller.Extensions.Universal.Abstracts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MaSchoeller.Extensions.Universal.Internals.Hosting
{
    internal class UniversalInitializer : IHostedService
    {
        private readonly Type _mainPageType;

        public UniversalInitializer(
            IHostApplicationLifetime lifetime,
            Type mainPageType,
            MainPageContext context)
        {
            if (lifetime is null)
            {
                throw new ArgumentNullException(nameof(lifetime));
            }

            _mainPageType = mainPageType ?? throw new ArgumentNullException(nameof(mainPageType));
            lifetime.ApplicationStarted.Register(() =>
            {
                if (!(Window.Current.Content is Frame frame))
                {
                    frame = new Frame();
                    Window.Current.Content = frame;
                }
                frame.Navigate(_mainPageType, context);
                Window.Current.Activate();
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
