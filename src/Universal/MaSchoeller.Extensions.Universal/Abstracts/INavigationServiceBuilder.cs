using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MaSchoeller.Extensions.Universal.Abstracts
{
    public interface INavigationServiceBuilder
    {
        void ConfigureRoute<TPage, TViewModel>(string route, ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TPage : Page
            where TViewModel : IRoutable;

        void AddDependenciesToServiceCollection(IServiceCollection services);
        INavigationService Build(IServiceProvider provider);
    }
}
