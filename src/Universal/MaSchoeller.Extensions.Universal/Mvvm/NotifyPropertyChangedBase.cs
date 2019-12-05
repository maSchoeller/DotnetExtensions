using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Universal.Mvvm
{
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Dictionary<string, object> _props;

        public NotifyPropertyChangedBase()
        {
            _props = new Dictionary<string, object>();
        }

        protected void SetProperty<TValue>(TValue value, [CallerMemberName]string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name can't be null, empty or whitespace.", nameof(name));
            }

            if (!_props.ContainsKey(name) || !Equals(_props[name], value))
            {
                _props[name] = value;
                RaisePropertyChanged(name);
            }
        }

        protected TValue GetProperty<TValue>([CallerMemberName]string name = null)
        {
            if (_props.ContainsKey(name))
            {
                return (TValue)_props[name];
            }
            else
            {
                return default;
            }
        }

        private void RaisePropertyChanged([CallerMemberName]string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
