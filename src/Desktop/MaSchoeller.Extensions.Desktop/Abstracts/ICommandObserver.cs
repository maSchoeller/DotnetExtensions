using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{

    public interface ICommandObserver
    {
        public event Action<object, EventArgs> Changed;
    }

}
