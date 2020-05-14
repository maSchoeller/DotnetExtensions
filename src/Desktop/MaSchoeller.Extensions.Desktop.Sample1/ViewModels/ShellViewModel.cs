using MaSchoeller.Extensions.Desktop.Abstracts;
using MaSchoeller.Extensions.Desktop.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MaSchoeller.Extensions.Desktop.Sample1.ViewModels
{
    public class ShellViewModel : NotifyPropertyChangedBase
    {
        public ShellViewModel(INavigationService navigationService, TestClass @class)
        {
            NavigationCommand = ConfigurableCommand.Create(
            o =>
            {
                var name = o as string;
                navigationService.NavigateTo(name);

            }).Build();
        }

        public ICommand NavigationCommand { get; }
    }
}
