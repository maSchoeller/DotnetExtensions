using System.Threading.Tasks;

using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Mvvm;
using MaSchoeller.Extensions.Desktop.Sample1.Controllers;
using MaSchoeller.Extensions.Desktop.Sample1.ViewModels;
using MaSchoeller.Extensions.Desktop.Sample1.Views;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

                           services.AddSingleton<TestClass>();
                           services.AddSingleton<ShellViewModel>();
                           services.AddSingleton<Page1ViewModel>();
                           services.AddSingleton<Page2ViewModel>();
                       });
                        b.UseStartup<Startup>();

                        b.ConfigureNavigation(nav =>
                        {
                            nav.AddRoute<Page1, Page1Controller>(Navigation.DefaultRoute);
                            nav.AddRoute<Page2, Page2Controller>("other");
                        });
                    })
                    .Build()
                    .RunAsync();
        }
    }
}

