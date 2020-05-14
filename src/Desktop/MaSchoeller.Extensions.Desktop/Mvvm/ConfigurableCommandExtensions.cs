using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Internals.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Mvvm
{
    public static class ConfigurableCommandExtensions
    {

        public static ICommandBuilder Observe<TObserveable,TReturn>(
            this ICommandBuilder command,
            TObserveable caller,
            Expression<Func<TReturn>> expression) where TObserveable : INotifyPropertyChanged
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
