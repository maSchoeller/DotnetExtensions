using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Tinkerforge.Internals
{
    internal class EventToAsyncEnumerableWrapper<TInput> where TInput : struct
    {

        private readonly Action<SensorUpdatedEventArgs<TInput>> _callback;
        private TInput? _currentItem;


        public EventToAsyncEnumerableWrapper()
        {
            _callback += (SensorUpdatedEventArgs<TInput> e) => _currentItem = e.Input;
        }

        public Action<SensorUpdatedEventArgs<TInput>> GetCallback()
        {
            return _callback;
        }

        public async Task<TInput> GetNextItem()
        {
            while (_currentItem is null)
            {
                await Task.Delay(10)
                    .ConfigureAwait(false);
            }
            var item = _currentItem.Value;
            _currentItem = null;
            return item;
        }


    }
}
