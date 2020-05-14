using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public interface INavigationService : INotifyPropertyChanged
    {
        event EventHandler<NavigationEventArgs> Navigated;
        event EventHandler<NavigationEventArgs> NavigationFailed;

        Task<bool> TryNavigateToAsync(string route);
        IRoutable CurrentRoute { get; }
    }

    public class NavigationEventArgs: EventArgs
    {
        public NavigationEventArgs(string route)
        {
            Route = route;
        }

        public string Route { get; }
    }

    public static class NavigationServiceExtensions
    {
        public static void NavigateTo(this INavigationService navigation, string route)
           => navigation.TryNavigateToAsync(route).GetAwaiter().GetResult();
    }
}
