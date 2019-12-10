using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Desktop.Helpers
{
    public class RoutableBase : NotifyPropertyChangedBase, IRoutable
    {

        public virtual Task EnterAsync()
        {
            Enter();
            return Task.CompletedTask;
        }

        protected virtual void Enter() { }

        public virtual Task LeaveAsync()
        {
            Leave();
            return Task.CompletedTask;
        }

        protected virtual void Leave() { }
    }
}
