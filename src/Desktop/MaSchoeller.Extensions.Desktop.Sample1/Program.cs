using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using MaSchoeller.Extensions.Desktop.Abstracts;
using Microsoft.Extensions.DependencyInjection;

namespace MaSchoeller.Extensions.Desktop.Sample1
{
    class Program
    {
       
        static async Task Main(string[] args)
        {  
            await Host.CreateDefaultBuilder(args)
                    .ConfigureDesktopDefaults<ShellWindow>()
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

