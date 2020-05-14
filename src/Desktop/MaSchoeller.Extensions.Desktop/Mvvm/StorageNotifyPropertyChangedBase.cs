using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Mvvm
{
    public class StorageNotifyPropertyChangedBase : NotifyPropertyChangedBase
    {
        
        private readonly IDictionary<string, object?> _propertyStore;

        public StorageNotifyPropertyChangedBase()
        {
            _propertyStore = new Dictionary<string,object?>();
        }


        protected void SetProperty<TValue>(TValue value, [CallerMemberName]string? propertyName = null)
        {

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Can't be null, empty or whitespace.",nameof(propertyName));
            }

            if (!_propertyStore.ContainsKey(propertyName!) || !Equals(_propertyStore[propertyName!], value))
            {
                _propertyStore[propertyName!] = value;
                RaisePropertyChanged(propertyName);
            }

        }

        protected TValue GetProperty<TValue>([CallerMemberName]string? propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Can't be null, empty or whitespace.", nameof(propertyName));
            }
            if (!_propertyStore.ContainsKey(propertyName!))
            {
                return default!;
            }

            return (TValue)(_propertyStore[propertyName!] ?? default)!;
        }

       
    }
}
