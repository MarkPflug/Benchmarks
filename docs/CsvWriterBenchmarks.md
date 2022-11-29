# CSV Writer Benchmarks

These benchmarks write 65k "SalesRecord" objects to a CSV file.

|               Method |      Mean |     Error |    StdDev |       Gen0 | CPU User Time | CPU Kernel Time |   Allocated |
|--------------------- |----------:|----------:|----------:|-----------:|--------------:|----------------:|------------:|
| RecordParserParallel |  53.07 ms | 17.771 ms |  6.337 ms |          - |     159.33 ms |         8.52 ms |   469.71 KB |
|       SylvanDataSync |  55.59 ms |  2.276 ms |  0.591 ms |          - |      64.93 ms |         4.51 ms |   453.87 KB |
|      SylvanDataAsync |  67.64 ms |  2.631 ms |  0.938 ms |          - |      68.68 ms |         6.51 ms |   527.69 KB |
|          NaiveBroken |  76.75 ms |  8.612 ms |  2.236 ms | 10142.8571 |      89.73 ms |        14.29 ms | 42298.38 KB |
|    RecordParserAsync |  84.68 ms | 20.717 ms |  5.380 ms |          - |      105.8 ms |        10.27 ms |   512.86 KB |
|                NReco |  97.42 ms | 11.012 ms |  2.860 ms | 10200.0000 |     118.75 ms |          7.5 ms | 42430.22 KB |
|            NLightCsv | 109.36 ms | 11.860 ms |  3.080 ms | 10200.0000 |     126.88 ms |        11.25 ms | 42299.42 KB |
|       CsvHelperAsync | 187.78 ms | 15.523 ms |  5.536 ms | 13666.6667 |     189.24 ms |         5.21 ms | 56301.13 KB |
|        CsvHelperSync | 197.93 ms | 52.838 ms | 18.842 ms | 13666.6667 |     194.44 ms |         5.21 ms | 56221.08 KB |

**NOTE**: This benchmark includes a custom BenchmarkDotNet diagnoser `CpuDiagnoser` which attempts to measure CPU usage in addition to execution time. This was intended to highlight that parallel implementations (RecordParserParallel), while being faster overall, also come at the cost of higher overall CPU utilization. Interestingly, async implementations also seem to me more costly from a CPU utilization.