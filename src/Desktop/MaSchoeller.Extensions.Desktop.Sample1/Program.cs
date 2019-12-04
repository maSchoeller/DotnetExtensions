using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using MaSchoeller.Extensions.Desktop;
using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Helpers;
using MaSchoeller.Extensions.Desktop.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace MaSchoeller.Extensions.Desktop.Sample1
{
    class Program
    {
       
        static async Task Main(string[] args)
        {  
            await Host.CreateDefaultBuilder()
                    .ConfigureSplashscreen<SplashscreenWindow>()
                    .ConfigureDesktopDefaults<ShellWindow>(b =>
                    {
                        b.UseStartup<Startup>();
                    })
                    .Build()
                    .RunAsync();
        }
    }


    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void ConfigureNavigation(INavigationServiceBuilder builder)
        {

        }
    }
}

