﻿using MaSchoeller.Extensions.Desktop.Abstracts;
using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace MaSchoeller.Extensions.Desktop.Internals.Mvvm
{
    internal class CommandPropertyObserver<T> : ICommandObserver
    {
        public event Action<object, EventArgs>? Changed;

        public CommandPropertyObserver(INotifyPropertyChanged caller, Expression<Func<T>> target)
        {
            if (caller is null)
            {
                throw new ArgumentNullException(nameof(caller));
            }
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }
            var member = target.Body as MemberExpression;
            //Todo: Add exception Message
            var name = member?.Member.Name ?? throw new ArgumentException();
            caller.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == name)
                {
                    Changed?.Invoke(this, EventArgs.Empty);
                }
            };
        }
    }
}