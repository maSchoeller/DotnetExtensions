using MaSchoeller.Extensions.Desktop.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Mvvm
{
    public static class ConfigurableCommandExtensions
    {

        public static IConfigurableCommand Observe<TReturn>(
            this IConfigurableCommand command,
            INotifyPropertyChanged caller,
            Expression<Func<TReturn>> expression)
        {
            command.AddObserver(new CommandPropertyObserver<TReturn>(caller, expression));
            return command;
        }

        public static IConfigurableCommand ObserveAll(
            this IConfigurableCommand command,
            INotifyPropertyChanged caller)
        {
            command.AddObserver(new CommandAllObserver(caller));
            return command;
        }
    }
}
