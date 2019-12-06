using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts
{
    public interface IMultiActor<TOutput, out TActor> : IHardware , IReadOnlyList<TActor>
        where TOutput : struct
        where TActor : IActor<TOutput>
    {

        TActor GetActor(string name);
    }
}
