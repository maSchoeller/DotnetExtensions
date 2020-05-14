using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Mvvm
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<TValue>(ref TValue storage, TValue value, [CallerMemberName] string? propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Can't be null, empty or whitespace.", nameof(propertyName));
            }
            if (!Equals(storage,value))
            {
                storage = value;
                RaisePropertyChanged(propertyName);
            }

        }

        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
