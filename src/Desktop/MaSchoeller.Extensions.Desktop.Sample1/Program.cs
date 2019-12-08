using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Desktop.Sample1
{
    class Program
    {
       
        static async Task Main(string[] args)
        {  
            await Host.CreateDefaultBuilder(args)
                    .ConfigureSplashscreen<SplashscreenWindow>()
                    .ConfigureDesktopDefaults<ShellWindow>(b =>
                    {
                        b.ConfigureServices(services => services.AddHostedService<CustomeService>());
                    })
                    .Build()
                    .RunAsync();
        }
    }
}

