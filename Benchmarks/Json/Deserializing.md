The utf8 work isn't perfect, but close enough to get the gist of speed I feel. Lots of optimisations to be made, especially with the list allocations I feel.

Utf8 also suits finding specific bits more too. This benchmarking is a bludgeon that doesn't use the finesse of it.

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4780/22H2/2022Update)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 9.0.100-preview.7.24407.12
  [Host]     : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2


| Method                          | Mean     | Error     | StdDev    | Gen0     | Gen1     | Gen2     | Allocated  |
|-------------------------------- |---------:|----------:|----------:|---------:|---------:|---------:|-----------:|
| Deserialize_SystemTextJson      | 1.606 ms | 0.0307 ms | 0.0797 ms | 353.5156 | 298.8281 | 166.0156 | 1740.35 KB |
| Deserialize_SystemTextJson_Byte | 1.191 ms | 0.0238 ms | 0.0423 ms | 185.5469 | 126.9531 |        - | 1003.41 KB |
| Deserialize_JsonNode            | 2.428 ms | 0.0478 ms | 0.0469 ms | 386.7188 | 328.1250 | 199.2188 | 1949.88 KB |
| Deserialize_Utf8JsonReader      | 1.262 ms | 0.0225 ms | 0.0199 ms | 261.7188 | 248.0469 |        - | 1556.99 KB |
