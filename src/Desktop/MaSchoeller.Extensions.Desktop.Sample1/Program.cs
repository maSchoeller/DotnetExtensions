using MaSchoeller.Extensions.Desktop.Mvvm;
using MaSchoeller.Extensions.Desktop.Sample1.ViewModels;
using MaSchoeller.Extensions.Desktop.Sample1.Views;
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
                        b.ConfigureServices(services =>
                        {
                            services.AddSingleton<ShellViewModel>();
                            services.AddHostedService<CustomeService>();
                        });
                        b.ConfigureNavigation(nav =>
                        {
                            nav.AddRoute<Page1, Page1ViewModel>(NavigationService.DefaultRoute);
                            nav.AddRoute<Page2, Page2ViewModel>("other");
                        });
                    })
                    .Build()
                    .RunAsync();
        }
    }
}

