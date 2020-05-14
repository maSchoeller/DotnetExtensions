using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public interface IController : IRoutable
    {
        public object Initialize();
    }
}
