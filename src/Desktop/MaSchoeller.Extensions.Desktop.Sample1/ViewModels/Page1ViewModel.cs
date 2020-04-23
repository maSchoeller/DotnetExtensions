using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Helpers;
using MaSchoeller.Extensions.Desktop.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MaSchoeller.Extensions.Desktop.Sample1.ViewModels
{
    public class Page1ViewModel : RoutableBase
    {
        public ICommand MyCommand{ get; }
        public Page1ViewModel()
        {
            MyCommand = ConfigurableCommand.Create(o => { })
                .ObserveAll(this)
                .Build();

        }

        public int Property { get; set; }

        protected override void Enter()
        {

        }

        protected override void Leave()
        {

        }
    }
}
