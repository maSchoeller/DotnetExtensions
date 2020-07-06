using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public interface IRouter
    {
        event NavigationChangedEventHandler RouteChanged;
        event NavigationChangedEventHandler RouteChangedError;

        string ActualRoute { get; }

        //Todo:implementing routing queue
        Task<bool> TryNavigateToAsnyc(CancellationToken token = default);
    }

    public delegate void NavigationChangedEventHandler(object sender, NavigationChangedEventArgs args);

    public class NavigationChangedEventArgs : EventArgs
    {

    }
}
