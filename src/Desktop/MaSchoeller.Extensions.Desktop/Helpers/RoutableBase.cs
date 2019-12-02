using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Helpers
{
    public class RoutableBase : NotifyPropertyChangedBase, IRoutable
    {
        public virtual bool CanEnter() => true;
        public virtual bool CanLeave() => true;

        public virtual bool TryEnter() => true;
        public virtual bool TryLeave() => true;

        public virtual void ReEnter()
        {
        }
    }
}
