using MaSchoeller.Extensions.Desktop.Mvvm;
using MaSchoeller.Extensions.Desktop.Sample1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Desktop.Sample1.Controllers
{
    public class Page2Controller : ControllerBase
    {
        public readonly Page2ViewModel _viewModel;
        public Page2Controller(Page2ViewModel viewModel)
        {
            _viewModel = viewModel;
        }


        public override object Initialize()
        {
            _viewModel.Test = "some other string";
            return _viewModel;
        }
    }
}
