using MaSchoeller.Extensions.Universal.Abstracts;
using MaSchoeller.Extensions.Universal.Internals.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MaSchoeller.Extensions.Universal.Sample2.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public INavigationService NavigationService { get; }
        public void NavViewLoaded(object sender, RoutedEventArgs e)
            => NavigationService.Navigate(Internals.Routing.NavigationService.DefaultRoute);

        public void ChangeRoute(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (!args.IsSettingsInvoked)
            {
                var route = args.InvokedItemContainer.Tag.ToString();
                NavigationService.Navigate(route);
            }
        }
    }
}
