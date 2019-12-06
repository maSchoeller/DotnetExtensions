using MaSchoeller.Extensions.Universal.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Universal.Mvvm
{
    public class RoutableBase : NotifyPropertyChangedBase, IRoutable
    {
        public string Header 
        { 
            get => GetProperty<string>(); 
            protected set => SetProperty(value); 
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnLeave()
        {
        }
    }
}
