using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MaSchoeller.Extensions.Universal.Mvvm
{
    public class PageBase : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!(e.Parameter is null))
            {
                var propinfo = GetType().GetProperty("ViewModel");
                var paramType = e.Parameter.GetType();
                if (propinfo.PropertyType.IsAssignableFrom(paramType))
                {
                    propinfo.SetValue(this, e.Parameter);
                }
                else
                {
                    //Todo: add exception message
                    throw new ArgumentException();
                }
            }

        }
    }
}
