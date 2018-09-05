using System;
using System.Collections.Generic;

namespace Playmode.Util.Collections
{
    public class LoopingEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public LoopingEnumerator(IEnumerable<T> enumerable)
        {
            _enumerator = enumerable.GetEnumerator();

            if (!_enumerator.MoveNext())
                throw new ArgumentException("Can't do an infinite loop on an empty Enumerator.");
        }

        public T Next()
        {
            var current = _enumerator.Current;
            if (!_enumerator.MoveNext())
            {
                _enumerator.Reset();
                _enumerator.MoveNext();
            }
            return current;
        }
    }
}