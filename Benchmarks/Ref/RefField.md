# Ref Field

[Via Konrad Kokosa](https://twitter.com/konradkokosa/status/1729908112960667991)

```
BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4291/22H2/2022Update)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 9.0.100-preview.4.24221.5
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
```

| Method           | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|----------------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| AccessByRefField | 0.9974 ns | 0.0121 ns | 0.0108 ns |  0.65 |    0.03 |      - |         - |        0.00 |
| AccessByField    | 1.5580 ns | 0.0317 ns | 0.0746 ns |  1.00 |    0.00 | 0.0004 |       2 B |        1.00 |