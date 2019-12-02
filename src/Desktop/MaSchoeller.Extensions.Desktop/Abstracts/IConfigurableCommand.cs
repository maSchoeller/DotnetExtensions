using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public interface IConfigurableCommand : ICommand
    {
        void AddObserver(ICommandObserver observer);
    }
}
