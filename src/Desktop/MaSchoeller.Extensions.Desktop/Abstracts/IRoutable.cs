using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Desktop.Abstracts
{
    public interface IRoutable
    {
        bool CanEnter();
        bool CanLeave();

        bool TryEnter();
        bool TryLeave();

        void ReEnter();
    }
}
