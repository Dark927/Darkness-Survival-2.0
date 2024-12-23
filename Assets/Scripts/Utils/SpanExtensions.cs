using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Dark.Utils
{

    public static class SpanExtensions
    {
        /// <summary>
        /// Extension method to return the custom stride enumerator
        /// </summary>
        /// <typeparam name="T">Element Type</typeparam>
        /// <param name="span">Target Span</param>
        /// <param name="stride">How far to move (equivalent to i+=stride in for loop)</param>
        /// <param name="bs">Block Size (how many sequential elements to take before stride jump)</param>
        /// <param name="limit">How many times to iterate before stop. Set to 0 to use Span.Length</param>
        /// <returns></returns>
        public static StrideEnumerator<T> Stride<T>(this Span<T> span, int stride, int bs = 1, int limit = 0) => new(span, stride, bs, limit);

        // Custom stride enumerator struct
        public ref struct StrideEnumerator<T>
        {
            private readonly Span<T> _span;
            private readonly int _stride;
            private readonly int _blockSize;
            private readonly int _limit;

            private int _index;
            private int _blk; //block number, starts from 1, 0 is initializer value
            private int _ctr; //counter for _limit check

            // Constructor
            public StrideEnumerator(Span<T> span, int stride, int blockSize, int size)
            {
                _span = span;
                _stride = stride;
                _blockSize = blockSize;
                _limit = size;

                _index = -stride; // Start before 0
                _blk = 0;//initializer value
                _ctr = 0;
            }

            // Current element
            public readonly ref T Current => ref _span[_index];

            // Move to the next even element
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                if (_blk >= _blockSize || _blk == 0)
                {
                    _index += _stride;
                    _blk = 1;
                }
                else
                {
                    _index += 1;//move to next element in the block
                    _blk++;
                }
                // index is in Span's bounds  AND no limit  or  counter < _limit
                return _index < _span.Length && (_limit == 0 || _ctr++ < _limit);
            }

            //code from chatgpt, havent checked
            public ref T this[int index]
            {
                get
                {
                    // Calculate the logical position in the original span
                    int logicalIndex = (index / _blockSize) * _stride + (index % _blockSize);

                    if (logicalIndex >= _span.Length || (_limit > 0 && index >= _limit))
                        throw new IndexOutOfRangeException("Index exceeds the bounds or limit of the sequence.");

                    return ref _span[logicalIndex];
                }
            }


            //the hack, unity, please switch to netcore i dare you
            //avoid using it, unless you want to copy entire span to managed(heap) type
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public List<T> ToList()
            {
                List<T> list;
                //prealloc limit for performance
                if (_limit != 0)
                    list = new List<T>(_limit);
                else
                    list = new List<T>();

                while (MoveNext())
                {
                    list.Add(Current);
                }
                return list;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override string ToString()
            {
                return string.Join(", ", this.ToList());
            }
            // Resetting is typically not supported for custom enumerators (ref structs)

        }

        // To allow `foreach` usage, expose a `GetEnumerator` method.
        public static StrideEnumerator<T> GetEnumerator<T>(this StrideEnumerator<T> enumerator) => enumerator;
    }
}