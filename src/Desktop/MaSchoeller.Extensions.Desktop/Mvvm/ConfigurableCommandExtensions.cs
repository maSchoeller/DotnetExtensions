using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Internals.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Mvvm
{
    public static class ConfigurableCommandExtensions
    {

        public static ICommandBuilder Observe<TReturn>(
            this ICommandBuilder command,
            INotifyPropertyChanged caller,
            Expression<Func<TReturn>> expression)
        {
            command.AddObserver(new CommandPropertyObserver<TReturn>(caller, expression));
            return command;
        }

        public static ICommandBuilder ObserveAll(
            this ICommandBuilder command,
            INotifyPropertyChanged caller)
        {
            command.AddObserver(new CommandAllObserver(caller));
            return command;
        }
    }
}
