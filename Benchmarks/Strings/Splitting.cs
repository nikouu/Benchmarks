using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks.Strings
{
    [MemoryDiagnoser]
    [Config(typeof(BenchmarkConfig))]
    public class Splitting
    {
        [Params("Ada Lovelace", "Lorem Ipsum is simply dummy text of the printing and typesetting industry.")]
        public string TestString { get; set; }

        [Benchmark(Baseline = true)]
        public string Split()
        {
            var firstname = TestString.Split(' ')[0];
            return firstname;
        }

        [Benchmark]
        public string Substring()
        {
            var firstname = TestString.Substring(0, TestString.IndexOf(' '));
            return firstname;
        }

        [Benchmark]
        public string Split_Span()
        {
            Span<Range> segments = stackalloc Range[2];
            var itemSpan = TestString.AsSpan();
            int segmentCount = itemSpan.Split(segments, ' ');

            return new string(itemSpan[segments[0]]);
        }

        [Benchmark]
        public string Substring_Span()
        {
            var firstname = TestString.AsSpan(0, TestString.IndexOf(' '));
            return new string(firstname);
        }

        [Benchmark]
        public string Slice_Span()
        {
            var itemSpan = TestString.AsSpan();
            var spaceIndex = itemSpan.IndexOf(' ');
            var firstname = itemSpan.Slice(0, spaceIndex);

            return new string(firstname);
        }

        //[Benchmark]
        //public string Split_Span_String_Create()
        //{
        //    return string.Create(TestString.Length, TestString, (span, name) =>
        //    {
        //        var itemSpan = name.AsSpan();
        //        var spaceIndex = itemSpan.IndexOf(" ");
        //        var firstname = itemSpan.Slice(0, spaceIndex);

        //        firstname.CopyTo(span);
        //    });
        //}
    }
}
