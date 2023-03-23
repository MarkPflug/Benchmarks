# CSV Writer Benchmarks

These benchmarks write 65k "SalesRecord" objects to a CSV file.

|                Method |      Mean |    Error |   StdDev |       Gen0 |   Allocated |
|---------------------- |----------:|---------:|---------:|-----------:|------------:|
| RecordParserParallel* |  41.19 ms | 0.570 ms | 0.031 ms |          - |   469.25 KB |
|        SylvanDataSync |  56.75 ms | 0.940 ms | 0.146 ms |          - |   453.89 KB |
|       SylvanDataAsync |  65.43 ms | 1.739 ms | 0.620 ms |          - |   527.71 KB |
|          RecordParser |  70.64 ms | 1.356 ms | 0.210 ms |          - |   467.04 KB |
|     RecordParserAsync |  72.96 ms | 1.016 ms | 0.264 ms |          - |   512.55 KB |
|           NaiveBroken |  75.42 ms | 1.198 ms | 0.311 ms | 10142.8571 | 42298.39 KB |
|                 NReco |  97.48 ms | 1.418 ms | 0.219 ms | 10166.6667 | 42430.21 KB |
|             NLightCsv | 108.79 ms | 2.036 ms | 0.112 ms | 10200.0000 | 42299.43 KB |
|         CsvHelperSync | 170.62 ms | 2.487 ms | 0.646 ms | 13666.6667 | 56221.15 KB |
|        CsvHelperAsync | 178.24 ms | 3.226 ms | 0.499 ms | 13666.6667 | 56301.05 KB |

* Uses 4x parallelization.