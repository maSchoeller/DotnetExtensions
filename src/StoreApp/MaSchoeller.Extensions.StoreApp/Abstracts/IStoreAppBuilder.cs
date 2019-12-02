using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MaSchoeller.Extensions.StoreApp.Abstracts
{
    public interface IStoreAppBuilder
    {
        void ConfigureServices(Action<IServiceCollection> configure);
        void ConfigureApplication(Action<Application> configure);
        void ConfigureNavigation(Action<INavigationServiceBuilder> configure);

        void UseStartup<Startup>();

        void Build();
    }
}
