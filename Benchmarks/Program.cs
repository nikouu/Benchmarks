using BenchmarkDotNet.Running;
using Benchmarks.Grades;
using Benchmarks.Json;
using Benchmarks.Loops;
using Benchmarks.Ref;
using Benchmarks.Strings;

var g = new Serialization();
g.Setup();

var f = g.Deserialize_Utf8JsonReader();

BenchmarkRunner.Run<Serialization>();

