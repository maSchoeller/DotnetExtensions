using MaSchoeller.Extensions.Desktop.Abstracts;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Desktop.Internals.Hosting
{
    internal class SplashscreenLifetimeHost : IHostedService
    {
        public SplashscreenLifetimeHost(
            ISplashscreenWindow splashscreen, 
            IHostApplicationLifetime lifetime)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                splashscreen.CloseWindow();
            });
        }

        public Task StartAsync(CancellationToken cancellationToken) 
            => Task.CompletedTask;

        public Task StopAsync(CancellationToken cancellationToken) 
            => Task.CompletedTask;
    }
}
