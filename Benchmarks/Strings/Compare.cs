using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Text;

namespace Benchmarks.Strings
{
    [MemoryDiagnoser]
    [Config(typeof(BenchmarkConfig))]
    public class Compare
    {
        private string str1 = "hello";
        private string str2 = "HELLO";

        [Benchmark]
        public string ConcatWithPlusOperator() => str1 + " " + str2;

        [Benchmark]
        public string ConcatWithStringConcat() => string.Concat(str1, " ", str2);

        [Benchmark]
        public string ConcatWithStringBuilder()
        {
            var sb = new StringBuilder();
            sb.Append(str1);
            sb.Append(" ");
            sb.Append(str2);
            return sb.ToString();
        }

        [Benchmark]
        public string ConcatWithStringFormat() => string.Format("{0} {1}", str1, str2);

        [Benchmark]
        public string ConcatWithStringInterpolation() => $"{str1} {str2}";



        public class Rootobject
        {
            public Glossary glossary { get; set; }
        }

        public class Glossary
        {
            public string title { get; set; }
            public Glossdiv GlossDiv { get; set; }
        }

        public class Glossdiv
        {
            public string title { get; set; }
            public Glosslist GlossList { get; set; }
        }

        public class Glosslist
        {
            public Glossentry GlossEntry { get; set; }
        }

        public class Glossentry
        {
            public string ID { get; set; }
            public string SortAs { get; set; }
            public string GlossTerm { get; set; }
            public string Acronym { get; set; }
            public string Abbrev { get; set; }
            public Glossdef GlossDef { get; set; }
            public string GlossSee { get; set; }
        }

        public class Glossdef
        {
            public string para { get; set; }
            public string[] GlossSeeAlso { get; set; }
        }

    }
}
