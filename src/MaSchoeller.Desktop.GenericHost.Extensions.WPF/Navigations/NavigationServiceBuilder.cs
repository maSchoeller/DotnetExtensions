using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace MaSchoeller.Desktop.GenericHost.Extensions.WPF.Navigations
{
    public class NavigationServiceBuilder
    {
        private readonly Dictionary<Type, Type> _bindings 
            = new Dictionary<Type, Type>();

        public bool BindView<TView,TViewModel>() 
            where TView : Page
        {
            var viewType = typeof(TView);
            var viewModelType = typeof(TViewModel);
            if (_bindings.ContainsKey(viewModelType))
            {
                return false;
            }
            else
            {
                _bindings.Add(viewModelType,viewType);
                return true;
            }
        }

    }
}
