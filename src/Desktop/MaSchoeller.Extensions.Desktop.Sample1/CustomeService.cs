using MaSchoeller.Extensions.Desktop.Abstracts;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Desktop.Sample1
{
    public class CustomService : IHostedService
    {
        private readonly ISplashscreenWindow _splashscreen;

        public CustomService(
            ISplashscreenWindow splashscreen, 
            IHostApplicationLifetime lifetime)
        {
            _splashscreen = splashscreen;
            lifetime.ApplicationStarted.Register(() =>
            {
                _splashscreen.IsBusy = false;
            });
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _splashscreen.IsBusy = true;
            _splashscreen.Progress = 30;
            _splashscreen.ReportMessage = "Some Message";
            await Task.Delay(3000);
            _splashscreen.Progress = 40;
            _splashscreen.ReportMessage = "Other Message";
            await Task.Delay(1000);
        }

        public Task StopAsync(CancellationToken cancellationToken) 
            => Task.CompletedTask;
    }
}
