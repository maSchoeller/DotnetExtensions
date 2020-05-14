using MaSchoeller.Extensions.Desktop.Mvvm;
using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Sample1.ViewModels;
using MaSchoeller.Extensions.Desktop.Sample1.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using MaSchoeller.Extensions.Desktop.Sample1.Controllers;

namespace MaSchoeller.Extensions.Desktop.Sample1
{
    class Program
    {

        static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                    .UseAutoFac()
                    .UseMVVMC()
                    .ConfigureSplashscreen<SplashscreenWindow>()
                    .ConfigureDesktopDefaults<ShellWindow>(b =>
                    {
                        b.UseStartup<Startup>();
                        b.ConfigureContainer(builder =>
                        {
                            builder.RegisterType<Page1ViewModel>();
                            builder.RegisterType<Page2ViewModel>();

                        });
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

