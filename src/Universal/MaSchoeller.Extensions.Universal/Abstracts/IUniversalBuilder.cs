using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace MaSchoeller.Extensions.Universal.Abstracts
{
    public interface IUniversalBuilder
    {
        void ConfigureServices(Action<IServiceCollection> configure);
        void ConfigureNavigation(Action<INavigationServiceBuilder> configure);

        void UseStartup<TStartup>();

        void Build();
    }
}
