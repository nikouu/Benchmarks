using BenchmarkDotNet.Attributes;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Benchmarks.Ref
{
    // Via Konrad Kokosa 
    // https://gist.github.com/kkokosa/b13b4ada81dc77d2f79eaf05b9f87d37
    [MemoryDiagnoser]
    public class RefField
    {
        private S1 _s1;
        [GlobalSetup]
        public void Setup()
        {
            _s1 = new S1()
            {
                StructField =
                {
                    StructArray = new S3[1]
                    {
                        new S3()
                        {
                            StructField = new D()
                            {
                                Field = 44
                            }
                        }
                    }
                }
            };

        }

        [Benchmark(OperationsPerInvoke = 16)]
        public void AccessByRefField()
        {
            RS rs = new RS();
            rs.RefStructField = ref _s1.StructField.StructArray[0].StructField;

            ConsumeRefField(rs); ConsumeRefField(rs); ConsumeRefField(rs); ConsumeRefField(rs);
            ConsumeRefField(rs); ConsumeRefField(rs); ConsumeRefField(rs); ConsumeRefField(rs);
            ConsumeRefField(rs); ConsumeRefField(rs); ConsumeRefField(rs); ConsumeRefField(rs);
            ConsumeRefField(rs); ConsumeRefField(rs); ConsumeRefField(rs); ConsumeRefField(rs);
        }

        [Benchmark(OperationsPerInvoke = 16, Baseline = true)]
        public void AccessByField()
        {
            C c = new C();
            c.StructField = _s1;

            ConsumeClass(c); ConsumeClass(c); ConsumeClass(c); ConsumeClass(c);
            ConsumeClass(c); ConsumeClass(c); ConsumeClass(c); ConsumeClass(c);
            ConsumeClass(c); ConsumeClass(c); ConsumeClass(c); ConsumeClass(c);
            ConsumeClass(c); ConsumeClass(c); ConsumeClass(c); ConsumeClass(c);
        }



        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ConsumeRefField(RS rs)
        {
            return rs.RefStructField.Field;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private int ConsumeClass(C c)
        {
            return c.StructField.StructField.StructArray[0].StructField.Field;
        }
    }

    public ref struct RS
    {
        public ref D RefStructField;
    }

    public class C
    {
        public S1 StructField;
    }

    public struct S1
    {
        public S2 StructField;
    }

    public struct S2
    {
        public S3[] StructArray;
    }

    public struct S3
    {
        public D StructField;
    }

    public struct D
    {
        public int Field;

        [UnscopedRef]
        public ref int ByRefField => ref Field;
    }

}