﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MaSchoeller.Extensions.Universal.Abstracts
{
    public interface INavigationService
    { 
        bool Navigate(string route);

        IRoutable CurrentRoute { get; }

    }
}
