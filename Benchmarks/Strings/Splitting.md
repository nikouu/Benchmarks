# Splitting
```
BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4170/22H2/2022Update)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
```

| Method         | TestString           | Mean       | Error     | StdDev    | Ratio    | RatioSD | Gen0   | Allocated | Alloc Ratio |
|--------------- |--------------------- |-----------:|----------:|----------:|---------:|--------:|-------:|----------:|------------:|
| Split          | Ada Lovelace         |  40.068 ns | 0.8553 ns | 1.8774 ns | baseline |         | 0.0268 |     112 B |             |
| Substring      | Ada Lovelace         |   9.468 ns | 0.2408 ns | 0.3749 ns |     -77% |    5.9% | 0.0076 |      32 B |        -71% |
| Split_Span     | Ada Lovelace         |  32.355 ns | 0.6229 ns | 0.5522 ns |     -20% |    3.9% | 0.0076 |      32 B |        -71% |
| Substring_Span | Ada Lovelace         |  10.399 ns | 0.1801 ns | 0.1504 ns |     -74% |    4.2% | 0.0076 |      32 B |        -71% |
| Slice_Span     | Ada Lovelace         |  10.560 ns | 0.2680 ns | 0.6316 ns |     -74% |    7.2% | 0.0076 |      32 B |        -71% |
|                |                      |            |           |           |          |         |        |           |             |
| Split          | Lorem(...)stry. [74] | 150.878 ns | 3.0574 ns | 5.8905 ns | baseline |         | 0.1299 |     544 B |             |
| Substring      | Lorem(...)stry. [74] |   8.485 ns | 0.1962 ns | 0.1638 ns |     -94% |    3.3% | 0.0076 |      32 B |        -94% |
| Split_Span     | Lorem(...)stry. [74] |  49.625 ns | 0.8920 ns | 0.7908 ns |     -66% |    3.0% | 0.0076 |      32 B |        -94% |
| Substring_Span | Lorem(...)stry. [74] |  11.000 ns | 0.2673 ns | 0.3182 ns |     -93% |    3.9% | 0.0076 |      32 B |        -94% |
| Slice_Span     | Lorem(...)stry. [74] |   9.958 ns | 0.2505 ns | 0.3344 ns |     -93% |    5.6% | 0.0076 |      32 B |        -94% |
