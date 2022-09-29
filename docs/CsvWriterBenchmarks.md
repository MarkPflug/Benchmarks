# CSV Writer Benchmarks

These benchmarks write 65k "SalesRecord" objects to a CSV file.

|               Method |      Mean |     Error |    StdDev | CPU User Time | CPU Kernel Time |      Gen 0 | Gen 1 | Gen 2 |   Allocated |
|--------------------- |----------:|----------:|----------:|--------------:|----------------:|-----------:|------:|------:|------------:|
| RecordParserParallel |  46.06 ms |  0.637 ms |  0.165 ms |   163.9205 ms |      10.2273 ms |          - |     - |     - |   465.45 KB |
|       SylvanDataSync |  50.87 ms |  3.616 ms |  1.290 ms |    48.4375 ms |       3.6458 ms |          - |     - |     - |   454.01 KB |
|      SylvanDataAsync |  55.48 ms |  1.882 ms |  0.671 ms |    54.1088 ms |       7.8125 ms |          - |     - |     - |   526.96 KB |
|          NaiveBroken |  78.16 ms |  1.259 ms |  0.195 ms |    93.7500 ms |       7.8125 ms | 10142.8571 |     - |     - | 42298.38 KB |
|    RecordParserAsync |  82.26 ms |  1.307 ms |  0.202 ms |    81.4732 ms |      10.6027 ms |          - |     - |     - |   508.67 KB |
|                NReco | 105.96 ms |  4.033 ms |  1.438 ms |   106.2500 ms |       4.1667 ms | 10200.0000 |     - |     - | 42430.22 KB |
|            NLightCsv | 121.12 ms |  5.984 ms |  2.134 ms |   115.1042 ms |      10.4167 ms | 10200.0000 |     - |     - | 42299.42 KB |
|        CsvHelperSync | 175.40 ms |  8.265 ms |  2.947 ms |   176.2153 ms |       5.2083 ms | 10000.0000 |     - |     - | 42397.77 KB |
|       CsvHelperAsync | 238.31 ms | 54.349 ms | 14.114 ms |   292.7083 ms |      16.6667 ms | 10000.0000 |     - |     - | 42473.85 KB |


**NOTE**: This benchmark includes a custom BenchmarkDotNet diagnoser `CpuDiagnoser` which attempts to measure CPU usage in addition to execution time. This was intended to highlight that parallel implementations (RecordParserParallel), while being faster overall, also come at the cost of higher overall CPU utilization. Interestingly, async implementations also seem to me more costly from a CPU utilization.