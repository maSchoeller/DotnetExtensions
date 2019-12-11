using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public class NavigationFrame : Frame
    {
        public NavigationFrame()
        {
            NavigationUIVisibility = NavigationUIVisibility.Hidden;
        }
    }
}
