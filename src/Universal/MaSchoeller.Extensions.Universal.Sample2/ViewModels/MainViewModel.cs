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
        private readonly INavigationService _navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void NavViewLoaded(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate(NavigationService.DefaultRoute);
        }

        public void ChangeRoute(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var route = args.InvokedItemContainer.Tag.ToString();
            _navigationService.Navigate(route);
        }
    }
}
