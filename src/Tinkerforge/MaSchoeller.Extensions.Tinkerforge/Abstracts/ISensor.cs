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

        async IAsyncEnumerable<TInput> GetInputQueue()
        {
            var iterator = new EventToAsyncEnumerableWrapper<TInput>();
            var callback = iterator.GetCallback();
            InputUpdated += (s, e) => callback(e);
            while (true)
            {
                yield return await iterator
                    .GetNextItem().ConfigureAwait(false);
            }
        }
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
