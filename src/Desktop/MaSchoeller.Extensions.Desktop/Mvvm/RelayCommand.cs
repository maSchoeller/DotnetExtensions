using MaSchoeller.Extensions.Desktop.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MaSchoeller.Extensions.Desktop.Mvvm
{
    public class RelayCommand : IConfigurableCommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;


        public RelayCommand(Action<object> execute, Func<object,bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) 
            => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter) 
            => _execute(parameter);

        public void AddObserver(ICommandObserver observer)
        {
            if (observer is null)
            {
                throw new ArgumentNullException(nameof(observer));
            }
            observer.Changed += (s, e) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
