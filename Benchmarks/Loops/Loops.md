# ArrayLoops

```
BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4291/22H2/2022Update)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
```


| Method                                | Size    | Mean          | Error        | StdDev        | Median        | Ratio    | RatioSD | Allocated | Alloc Ratio |
|-------------------------------------- |-------- |--------------:|-------------:|--------------:|--------------:|---------:|--------:|----------:|------------:|
| For                                   | 100     |      41.10 ns |     0.862 ns |      0.806 ns |      40.71 ns | baseline |         |         - |          NA |
| ForEach                               | 100     |      31.36 ns |     0.463 ns |      0.433 ns |      31.17 ns |     -24% |    2.5% |         - |          NA |
| For_Span                              | 100     |      32.78 ns |     0.712 ns |      1.635 ns |      32.46 ns |     -18% |    7.7% |         - |          NA |
| Unsafe_For_Span_GetReference          | 100     |      32.22 ns |     0.695 ns |      1.404 ns |      31.78 ns |     -21% |    5.6% |         - |          NA |
| Unsafe_For_Span_GetArrayDataReference | 100     |      32.07 ns |     0.530 ns |      0.413 ns |      32.10 ns |     -22% |    2.5% |         - |          NA |
|                                       |         |               |              |               |               |          |         |           |             |
| For                                   | 100000  |  24,623.34 ns |   485.167 ns |  1,328.136 ns |  24,096.24 ns | baseline |         |         - |          NA |
| ForEach                               | 100000  |  23,889.12 ns |   333.867 ns |    312.299 ns |  23,838.04 ns |      -8% |    6.4% |         - |          NA |
| For_Span                              | 100000  |  25,163.90 ns |   501.252 ns |  1,405.563 ns |  24,833.18 ns |      +2% |    7.8% |         - |          NA |
| Unsafe_For_Span_GetReference          | 100000  |  23,501.64 ns |   215.424 ns |    201.508 ns |  23,466.12 ns |      -9% |    6.2% |         - |          NA |
| Unsafe_For_Span_GetArrayDataReference | 100000  |  23,589.25 ns |   391.641 ns |    327.038 ns |  23,486.10 ns |     -10% |    6.3% |         - |          NA |
|                                       |         |               |              |               |               |          |         |           |             |
| For                                   | 1000000 | 245,090.67 ns | 4,889.115 ns |  6,357.231 ns | 243,529.90 ns | baseline |         |         - |          NA |
| ForEach                               | 1000000 | 246,979.75 ns | 4,921.122 ns | 11,107.793 ns | 244,174.17 ns |      +2% |    5.0% |         - |          NA |
| For_Span                              | 1000000 | 281,965.90 ns | 7,010.210 ns | 20,337.891 ns | 277,361.23 ns |     +10% |    5.1% |         - |          NA |
| Unsafe_For_Span_GetReference          | 1000000 | 249,678.46 ns | 4,933.540 ns |  9,738.327 ns | 248,485.30 ns |      +2% |    4.4% |         - |          NA |
| Unsafe_For_Span_GetArrayDataReference | 1000000 | 249,511.12 ns | 4,908.556 ns |  8,064.899 ns | 249,637.40 ns |      +2% |    4.2% |         - |          NA |

# ListLoops

```
BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4291/22H2/2022Update)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.200
  [Host]   : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  .NET 8.0 : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
```


| Method                                | Size    | Mean          | Error        | StdDev       | Ratio    | RatioSD | Allocated | Alloc Ratio |
|-------------------------------------- |-------- |--------------:|-------------:|-------------:|---------:|--------:|----------:|------------:|
| For                                   | 100     |      51.60 ns |     0.844 ns |     2.101 ns | baseline |         |         - |          NA |
| ForEach                               | 100     |      53.03 ns |     0.414 ns |     0.346 ns |      -4% |    4.7% |         - |          NA |
| For_Span                              | 100     |      31.17 ns |     0.671 ns |     0.627 ns |     -43% |    4.7% |         - |          NA |
| Unsafe_For_Span_GetReference          | 100     |      50.69 ns |     0.514 ns |     0.481 ns |      -7% |    5.0% |         - |          NA |
| Unsafe_For_Span_GetArrayDataReference | 100     |      38.71 ns |     0.190 ns |     0.158 ns |     -30% |    4.4% |         - |          NA |
|                                       |         |               |              |              |          |         |           |             |
| For                                   | 100000  |  45,324.94 ns |   333.427 ns |   295.574 ns | baseline |         |         - |          NA |
| ForEach                               | 100000  |  45,467.03 ns |   320.029 ns |   299.355 ns |      +0% |    0.8% |         - |          NA |
| For_Span                              | 100000  |  22,936.31 ns |   189.636 ns |   177.385 ns |     -49% |    1.0% |         - |          NA |
| Unsafe_For_Span_GetReference          | 100000  |  45,628.28 ns |    84.678 ns |    75.065 ns |      +1% |    0.6% |         - |          NA |
| Unsafe_For_Span_GetArrayDataReference | 100000  |  32,220.96 ns |   346.396 ns |   307.071 ns |     -29% |    1.2% |         - |          NA |
|                                       |         |               |              |              |          |         |           |             |
| For                                   | 1000000 | 452,007.65 ns | 1,437.729 ns | 1,200.569 ns | baseline |         |         - |          NA |
| ForEach                               | 1000000 | 458,468.90 ns | 8,771.622 ns | 8,204.980 ns |      +2% |    1.9% |         - |          NA |
| For_Span                              | 1000000 | 239,026.90 ns | 1,976.223 ns | 1,751.870 ns |     -47% |    0.8% |         - |          NA |
| Unsafe_For_Span_GetReference          | 1000000 | 466,613.86 ns | 3,112.884 ns | 2,759.491 ns |      +3% |    0.5% |         - |          NA |
| Unsafe_For_Span_GetArrayDataReference | 1000000 | 334,349.62 ns | 4,663.923 ns | 4,362.637 ns |     -26% |    1.2% |         - |          NA |