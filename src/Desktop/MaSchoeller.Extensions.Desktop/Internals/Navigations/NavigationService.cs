using MaSchoeller.Extensions.Desktop.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Internals.Navigations
{
    public class NavigationService : INavigationService
    {
        private readonly IDictionary<string, (Type ViewModel, Type View)> _routes;

        public NavigationService(IDictionary<string, (Type ViewModel, Type View)> routes)
        {
            _routes = routes ?? throw new ArgumentNullException(nameof(routes));
        }

        public void Navigate(string route)
        {
            throw new NotImplementedException();
        }
    }
}
