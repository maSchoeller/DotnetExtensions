using MaSchoeller.Extensions.Universal.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Universal.Sample1
{
    public class BlankPage1ViewModel : RoutableBase
    {
        public BlankPage1ViewModel()
        {

        }

        public override void OnEnter()
        {
            Test = "Hallo welt";
        }

        public override void OnLeave()
        {
        }

        public string Test { get => GetProperty<string>(); set => SetProperty(value); }
    }
}
