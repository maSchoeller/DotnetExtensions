using MaSchoeller.Desktop.GenericHost.Extensions.WPF.Abstracts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF.Internals
{
    internal class SplashScreenHost : IHostedService
    {

        public SplashScreenHost(ISplashscreenWindow splashscreen, IHostApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                splashscreen.CloseWindow();
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
