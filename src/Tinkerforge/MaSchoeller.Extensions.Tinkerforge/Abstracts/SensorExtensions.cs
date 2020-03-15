using MaSchoeller.Extensions.Tinkerforge.Internals;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaSchoeller.Extensions.Tinkerforge.Abstracts
{
    public static class SensorExtensions
    {

        public static async IAsyncEnumerable<TInput> GetInputQueue<TInput>(ISensor<TInput> sensor)
            where TInput : struct
        {
            if (sensor is null)
            {
                throw new ArgumentNullException(nameof(sensor));
            }

            var iterator = new EventToAsyncEnumerableWrapper<TInput>();
            sensor.InputUpdated += (s, e) => iterator.Update(e.Input);
            while (true)
            {
                yield return await iterator
                    .GetNextItem()
                    .ConfigureAwait(false);
            }
        }
    }
}
