using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using MaSchoeller.Extensions.Desktop;
using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Helpers;
using MaSchoeller.Extensions.Desktop.Mvvm;

namespace MaSchoeller.Extensions.Desktop.Sample1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder()
                    .ConfigureSplashscreen<SplashscreenWindow>()
                    .ConfigureDesktopDefaults<ShellWindow>(configure: b =>
                    {
                        b.UseStartup<Startup>();
                    })
                    .Build()
                    .RunAsync();
        }
    }


    class Startup
    {
        public void ConfigureNavigation(INavigationServiceBuilder builder)
        {
        }
    }
}

