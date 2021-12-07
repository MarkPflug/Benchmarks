# CSV Writer Benchmarks

These benchmarks write a 100k sequence of object data containing several typed columns as well as a "grid" of 20 doubles,
to a `TextWriter.Null`.

|               Method |     Mean |    Error |   StdDev |      Gen 0 | CPU User Time | CPU Kernel Time |    Gen 1 |  Allocated |
|--------------------- |---------:|---------:|---------:|-----------:|--------------:|----------------:|---------:|-----------:|
| RecordParserParallel | 119.5 ms |  7.24 ms |  2.58 ms |  5600.0000 |   447.9167 ms |       1.0417 ms | 800.0000 |  22,883 KB |
|       SylvanDataSync | 244.4 ms |  1.97 ms |  0.11 ms |          - |   244.7917 ms |               - |        - |      46 KB |
|          NaiveBroken | 248.6 ms |  9.20 ms |  3.28 ms | 21500.0000 |   251.3021 ms |               - |        - |  89,056 KB |
|      SylvanDataAsync | 260.4 ms |  4.96 ms |  1.77 ms |          - |   283.8542 ms |      13.0208 ms |        - |     373 KB |
|    RecordParserAsync | 316.0 ms |  5.61 ms |  1.46 ms |  2500.0000 | 1,243.7500 ms |     125.0000 ms |        - |  10,375 KB |
|                NReco | 304.8 ms |  5.93 ms |  1.54 ms | 21000.0000 |   309.3750 ms |               - |        - |  86,712 KB |
|            NLightCsv | 384.9 ms | 32.48 ms | 11.58 ms | 34000.0000 |   388.0208 ms |               - |        - | 140,764 KB |
|        CsvHelperSync | 552.8 ms | 15.94 ms |  4.14 ms | 34000.0000 |   665.6250 ms |               - |        - | 140,635 KB |
|       CsvHelperAsync | 699.0 ms | 16.90 ms |  4.39 ms | 42000.0000 | 2,450.0000 ms |     456.2500 ms |        - | 174,227 KB |


**NOTE**: This benchmark includes a custom BenchmarkDotNet diagnoser `CpuDiagnoser` which attempts to measure CPU usage in addition to execution time. This was intended to highlight that parallel implementations (RecordParserParallel), while being faster overall, also come at the cost of higher overall CPU utilization. Interestingly, async implementations also seem to me more costly from a CPU utilization.