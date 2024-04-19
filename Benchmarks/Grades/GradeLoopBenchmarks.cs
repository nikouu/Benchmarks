using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.Etlx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks.Grades
{
    [MemoryDiagnoser]
    [Config(typeof(BenchmarkConfig))]
    public class GradeLoopBenchmarks
    {
        [Params(100, 100_000, 1_000_000)]
        public int Size { get; set; }

        public int[] _items;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random(69);
            _items = Enumerable.Range(1, Size).Select(x => random.Next(0, 101)).ToArray();
        }

        [Benchmark(Baseline = true)]
        public void Original()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                GradeMethods.Original(_items[i]);
            }
        }

        [Benchmark]
        public void AgileJebrim1()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                GradeMethods.AgileJebrim1(_items[i]);
            }
        }

        [Benchmark]
        public void AgileJebrim2()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                GradeMethods.AgileJebrim2(_items[i]);
            }
        }

        [Benchmark]
        public void AgileJebrim3()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                GradeMethods.AgileJebrim3(_items[i]);
            }
        }


        [Benchmark]
        public void amohr1()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                GradeMethods.amohr1(_items[i]);
            }
        }

        [Benchmark]
        public void amohr2()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                GradeMethods.amohr2(_items[i]);
            }
        }

        [Benchmark]
        public void FreyaHolmer()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                GradeMethods.FreyaHolmer(_items[i]);
            }
        }

        [Benchmark]
        public void iquilezles()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                GradeMethods.iquilezles(_items[i]);
            }
        }

        [Benchmark]
        public void nthnblair()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                GradeMethods.nthnblair(_items[i]);
            }
        }

        [Benchmark]
        public void xsphi()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                GradeMethods.xsphi(_items[i]);
            }
        }

        [Benchmark]
        public void SwitchExpression()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                GradeMethods.SwitchExpression(_items[i]);
            }
        }
    }
}
