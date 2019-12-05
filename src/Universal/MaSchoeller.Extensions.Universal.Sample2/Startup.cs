using MaSchoeller.Extensions.Universal.Abstracts;
using MaSchoeller.Extensions.Universal.Internals.Routing;
using MaSchoeller.Extensions.Universal.Sample2.ViewModels;
using MaSchoeller.Extensions.Universal.Sample2.Views;

namespace MaSchoeller.Extensions.Universal.Sample2
{
    public class Startup
    {
        public void ConfigureNavigation(INavigationServiceBuilder builder)
        {
            builder.ConfigureRoute<Page1, Page1ViewModel>(NavigationService.DefaultRoute);
            builder.ConfigureRoute<Page2, Page2ViewModel>("page2");
        }
    }
}