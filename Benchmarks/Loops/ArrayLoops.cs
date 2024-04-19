using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks.Loops
{
    [MemoryDiagnoser]
    [Config(typeof(BenchmarkConfig))]
    public class ArrayLoops
    {
        [Params(100, 100_000, 1_000_000)]
        public int Size { get; set; }

        public int[] _items;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random(69);
            _items = Enumerable.Range(1, Size).Select(x => random.Next()).ToArray();
        }

        [Benchmark(Baseline = true)]
        public int[] For()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                DoSomething(_items[i]);
            }

            return _items;
        }

        [Benchmark]
        public int[] ForEach()
        {
            foreach (var item in _items)
            {
                DoSomething(item);
            }

            return _items;
        }

        [Benchmark]
        public int[] For_Span()
        {
            var itemsSpan = _items.AsSpan();
            for (int i = 0; i < itemsSpan.Length; i++)
            {
                DoSomething(itemsSpan[i]);
            }

            return _items;
        }

        [Benchmark]
        public int[] Unsafe_For_Span_GetReference()
        {
            var itemsSpan = _items.AsSpan();
            ref var searchSpace = ref MemoryMarshal.GetReference(itemsSpan);
            for (var i = 0; i < itemsSpan.Length; i++)
            {
                //var item = Unsafe.Add(ref searchSpace, 1);
                DoSomething(Unsafe.Add(ref searchSpace, 1));
            }
            return _items;
        }

        [Benchmark]
        public int[] Unsafe_For_Span_GetArrayDataReference()
        {
            ref var searchSpace = ref MemoryMarshal.GetArrayDataReference(_items);
            for (var i = 0; i < _items.Length; i++)
            {
                // var item = Unsafe.Add(ref searchSpace, 1);
                DoSomething(Unsafe.Add(ref searchSpace, 1));
            }

            return _items;
        }

        //[Benchmark]
        //public int[] Unsafe_While_Span_GetReference()
        //{
        //    ref var start = ref MemoryMarshal.GetArrayDataReference(_items);
        //    ref var end = ref Unsafe.Add(ref start, _items.Length);

        //    while (Unsafe.IsAddressLessThan(ref start, ref end))
        //    {
        //        DoSomething(start);
        //        start = ref Unsafe.Add(ref start, 1);
        //    }

        //    return _items;
        //}

        private void DoSomething(int i) { }
    }
}
