using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts
{
    public interface IMultiSensor<TInput, out TSensor> : IHardware, IReadOnlyList<TSensor>
        where TInput : struct
        where TSensor : ISensor<TInput>
    {
        TSensor GetSensor(string name);
    }
}
