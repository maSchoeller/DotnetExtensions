using MaSchoeller.Extensions.Desktop.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Mvvm
{
    public class CommandAllObserver : ICommandObserver
    {
        public event Action<object, EventArgs>? Changed;
        public CommandAllObserver(INotifyPropertyChanged caller)
        {
            if (caller is null)
            {
                throw new ArgumentNullException(nameof(caller));
            }

            caller.PropertyChanged += (s, e) => Changed?.Invoke(this, EventArgs.Empty);
        }

    }
}
