using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public interface ICommandBuilder
    {
        void AddObserver(ICommandObserver observer);

        ICommand Build();
    }
}
