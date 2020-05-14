using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Mvvm
{
    public abstract class ControllerBase : RoutableBase, IController
    {
        public abstract object Initialize();
    }
}
