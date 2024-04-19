using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks.Grades
{
    [MemoryDiagnoser]
    [Config(typeof(BenchmarkConfig))]
    public class GradeBenchmarks
    {
        // Thought a long time about whether this should end up being an array of 0-100 inclusive and looping
        // but I didn't want the loop overhead in the benchmark
        public IEnumerable<int> Scores()
        {
            yield return 50;
            yield return 60;
            yield return 70;
            yield return 80;
            yield return 90;
        }

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(Scores))]
        public string Original(int score) =>
            GradeMethods.Original(score);

        [Benchmark]
        [ArgumentsSource(nameof(Scores))]
        public char AgileJebrim1(int score) =>
            GradeMethods.AgileJebrim1(score);

        [Benchmark]
        [ArgumentsSource(nameof(Scores))]
        public char AgileJebrim2(int score) =>
            GradeMethods.AgileJebrim2(score);

        [Benchmark]
        [ArgumentsSource(nameof(Scores))]
        public char AgileJebrim3(int score) =>
            GradeMethods.AgileJebrim3(score);

        [Benchmark]
        [ArgumentsSource(nameof(Scores))]
        public char amohr1(int score) =>
            GradeMethods.amohr1(score);

        [Benchmark]
        [ArgumentsSource(nameof(Scores))]
        public char amohr2(int score) =>
            GradeMethods.amohr2(score);

        [Benchmark]
        [ArgumentsSource(nameof(Scores))]
        public char FreyaHolmer(int score) =>
            GradeMethods.FreyaHolmer(score);

        [Benchmark]
        [ArgumentsSource(nameof(Scores))]
        public char iquilezles(int score) =>
            GradeMethods.iquilezles(score);

        [Benchmark]
        [ArgumentsSource(nameof(Scores))]
        public char nthnblair(int score) =>
            GradeMethods.nthnblair(score);

        [Benchmark]
        [ArgumentsSource(nameof(Scores))]
        public char xsphi(int score) =>
            GradeMethods.xsphi(score);

        [Benchmark]
        [ArgumentsSource(nameof(Scores))]
        public string SwitchExpression(int score) =>
            GradeMethods.SwitchExpression(score);
    }
}
