using MaSchoeller.Extensions.Tinkerforge.Internals;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts
{
    public interface ISensor<TInput> where TInput : struct
    {
        event EventHandler<SensorUpdatedEventArgs<TInput>> InputUpdated;

        TInput CurrentInput { get; }
    }

    public class SensorUpdatedEventArgs<TInput>
    {
        public SensorUpdatedEventArgs(TInput input)
        {
            Input = input;
        }

        public TInput Input { get; }
    }
}
