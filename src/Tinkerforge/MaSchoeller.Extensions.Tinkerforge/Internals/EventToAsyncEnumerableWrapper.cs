using MaSchoeller.Extensions.Tinkerforge.Abstracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MaSchoeller.Extensions.Tinkerforge.Internals
{
    internal class EventToAsyncEnumerableWrapper<TInput> where TInput : struct
    {
        private TInput? _currentItem;
        public void Update(TInput input)
        {
            _currentItem = input;
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
