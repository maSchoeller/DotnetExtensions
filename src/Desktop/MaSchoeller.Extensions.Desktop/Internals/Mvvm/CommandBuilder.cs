using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MaSchoeller.Extensions.Desktop.Internals.Mvvm
{
    internal class CommandBuilder : ICommandBuilder
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool>? _canExecute;
        private readonly List<ICommandObserver> _observers;

        public CommandBuilder(
            Action<object> execute, 
            Func<object,bool>? canExecute = null)
        {
            _observers = new List<ICommandObserver>();
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public void AddObserver(ICommandObserver observer)
        {
            _observers.Add(observer);
        }

        public ICommand Build()
        {
            return new ConfigurableCommand(_observers,_execute,_canExecute);
        }
    }
}
