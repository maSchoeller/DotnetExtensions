using MaSchoeller.Extensions.Universal.Abstracts;
using MaSchoeller.Extensions.Universal.Internals.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Universal.Sample1
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void ConfigureNavigation(INavigationServiceBuilder navigation)
        {
            navigation.ConfigureRoute<BlankPage1, BlankPage1ViewModel>(NavigationService.DefaultRoute);
        }
    }
}
