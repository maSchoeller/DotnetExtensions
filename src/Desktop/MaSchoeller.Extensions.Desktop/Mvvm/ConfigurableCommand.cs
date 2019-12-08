using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Internals.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MaSchoeller.Extensions.Desktop.Mvvm
{
    public class ConfigurableCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged;


        public ConfigurableCommand(
            IEnumerable<ICommandObserver> observers,
            Action<object> execute,
            Func<object, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
            foreach (var item in observers)
            {
                item.Changed += (s, e) =>
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object parameter)
            => _canExecute?.Invoke(parameter) ?? true;

        public void Execute(object parameter)
            => _execute(parameter);


        public static ICommandBuilder Create(Action<object> execute,
            Func<object, bool>? canExecute = null)
            => new CommandBuilder(execute, canExecute);
    }
}
