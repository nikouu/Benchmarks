using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
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
    [SimpleJob(RuntimeMoniker.Net80)]
    public class ListLoops
    {
        [Params(100, 100_000, 1_000_000)]
        public int Size { get; set; }

        public List<int> _items;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random(69);
            _items = Enumerable.Range(1, Size).Select(x => random.Next()).ToList();
        }

        [Benchmark(Baseline = true)]
        public List<int> For()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                DoSomething(_items[i]);
            }

            return _items;
        }

        [Benchmark]
        public List<int> ForEach()
        {
            foreach (var item in _items)
            {
                DoSomething(item);
            }

            return _items;
        }

        [Benchmark]
        public List<int> For_Span()
        {
            var itemsSpan = CollectionsMarshal.AsSpan(_items);
            for (int i = 0; i < itemsSpan.Length; i++)
            {
                DoSomething(itemsSpan[i]);
            }

            return _items;
        }


        [Benchmark]
        public List<int> Unsafe_For_Span_GetReference()
        {
            var itemsSpan = CollectionsMarshal.AsSpan(_items);
            ref var start = ref MemoryMarshal.GetReference(itemsSpan);
            for (var i = 0; i < itemsSpan.Length; i++)
            {
                DoSomething(Unsafe.Add(ref start, i));
            }
            return _items;
        }

        [Benchmark]
        public List<int> Unsafe_For_Span_GetArrayDataReference()
        {
            var itemsSpan = CollectionsMarshal.AsSpan(_items);
            ref var start = ref MemoryMarshal.GetReference(itemsSpan);
            ref var end = ref Unsafe.Add(ref start, _items.Count);

            while (Unsafe.IsAddressLessThan(ref start, ref end))
            {
                //Console.WriteLine(start);
                DoSomething(start);
                start = ref Unsafe.Add(ref start, 1);
            }

            return _items;
        }


        public List<int> Unsafe_For_Span_GetArrayDataReference(List<int> items)
        {
            var itemsSpan = CollectionsMarshal.AsSpan(items);
            ref var start = ref MemoryMarshal.GetReference(itemsSpan);
            ref var end = ref Unsafe.Add(ref start, items.Count);

            while (Unsafe.IsAddressLessThan(ref start, ref end))
            {
                DoSomething(start);
                start = ref Unsafe.Add(ref start, 1);
            }

            return items;
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
