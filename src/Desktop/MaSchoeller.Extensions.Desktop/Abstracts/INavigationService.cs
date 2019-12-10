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

        Task NavigateToAsync(string route);

        void NavigateTo(string route) 
            => NavigateToAsync(route).GetAwaiter().GetResult();

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
}
