using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using MaSchoeller.Extensions.Desktop;

namespace MaSchoeller.Extensions.Desktop.Sample1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder()
                    .UseSplashScreen<SplashscreenWindow>()
                    .ConfigureDesktopDefaults<ShellWindow>()
                    .Build()
                    .RunAsync();
        }
    }
}
