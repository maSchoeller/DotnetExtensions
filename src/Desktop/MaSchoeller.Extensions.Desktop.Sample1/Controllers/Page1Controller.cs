using MaSchoeller.Extensions.Desktop.Mvvm;
using MaSchoeller.Extensions.Desktop.Sample1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Desktop.Sample1.Controllers
{
    public class Page1Controller : ControllerBase
    {
        public readonly Page1ViewModel _viewModel;
        public Page1Controller(Page1ViewModel viewModel)
        {
            _viewModel = viewModel;
        }


        public override object Initialize()
        {
            _viewModel.Property = 30;
            return _viewModel;
        }
    }
}
