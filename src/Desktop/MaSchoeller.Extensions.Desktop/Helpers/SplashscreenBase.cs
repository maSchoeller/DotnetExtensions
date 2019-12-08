using MaSchoeller.Extensions.Desktop.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MaSchoeller.Extensions.Desktop.Helpers
{
    public class SplashScreenBase : Window, ISplashscreenWindow, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly Dictionary<string, object> _properties;

        public SplashScreenBase()
        {
            _properties = new Dictionary<string, object>();
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            DataContext = this;
        }

        public string Header { get => GetProperty<string>(); set => SetProperty(value); }
        public string ReportMessage { get => GetProperty<string>(); set => SetProperty(value); }
        public bool IsBusy { get => GetProperty<bool>(); set => SetProperty(value); }
        public byte Progress { get => GetProperty<byte>(); set => SetProperty(value); }


        protected void SetProperty<T>(T value, [CallerMemberName]string? name = null)
        {
            if (name is null)
            {
                throw new ArgumentNullException(name);
            }
            Dispatcher.Invoke(() =>
            {
                if (_properties.ContainsKey(name))
                {

                    if (!Equals(_properties[name], value))
                    {
                        _properties[name] = value!;
                        OnPropertyChanged(name);
                    }
                }
                else
                {
                    _properties[name] = value!;
                    OnPropertyChanged(name);
                }
            });
        }

        protected T GetProperty<T>([CallerMemberName]string? name = null)
        {
            if (name is null)
            {
                throw new ArgumentNullException(name);
            }
            if (_properties.ContainsKey(name))
            {
                return (T)_properties[name];
            }
            else
            {
                return default;
            }
        }

        private void OnPropertyChanged([CallerMemberName]string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public void CloseWindow()
        {
            Dispatcher.Invoke(() =>
            {
                Close();
            });
        }
    }
}
