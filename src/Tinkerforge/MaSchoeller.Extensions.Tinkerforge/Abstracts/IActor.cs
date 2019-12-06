using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts
{
    public interface IActor<TOutput> where TOutput : struct
    {
        TOutput Output { get; set; }
    }
}
