# CSV Reader Benchmarks

|             Method |       Mean |     Error |    StdDev |     Median | Ratio | RatioSD |      Gen 0 |    Gen 1 |    Gen 2 |    Allocated |
|------------------- |-----------:|----------:|----------:|-----------:|------:|--------:|-----------:|---------:|---------:|-------------:|
|          CsvHelper |  23.216 ms | 0.3279 ms | 0.2907 ms |  23.180 ms |  1.00 |    0.00 |  6000.0000 |        - |        - |  27258.73 KB |
| CsvTextFieldParser |  12.300 ms | 0.2068 ms | 0.3567 ms |  12.179 ms |  0.53 |    0.01 |  5437.5000 |        - |        - |  22235.21 KB |
|      FastCsvParser |   9.286 ms | 0.1782 ms | 0.2254 ms |   9.206 ms |  0.40 |    0.01 |  1828.1250 | 125.0000 |  46.8750 |   7548.92 KB |
|         CsvBySteve | 107.428 ms | 2.1133 ms | 3.6454 ms | 106.508 ms |  4.68 |    0.18 | 22800.0000 |        - |        - |  93808.72 KB |
|         Lumenworks |  18.812 ms | 0.1735 ms | 0.1623 ms |  18.781 ms |  0.81 |    0.01 | 10468.7500 |        - |        - |  42798.25 KB |
|        NaiveBroken |   4.919 ms | 0.0980 ms | 0.2309 ms |   4.827 ms |  0.22 |    0.01 |  2757.8125 |        - |        - |  11266.87 KB |
|          NLightCsv |  13.832 ms | 0.2646 ms | 0.2475 ms |  13.774 ms |  0.60 |    0.01 |  1765.6250 |        - |        - |   7333.87 KB |
|        VisualBasic | 116.276 ms | 1.7155 ms | 1.5207 ms | 116.689 ms |  5.01 |    0.08 | 45600.0000 |        - |        - | 187058.47 KB |
|       FlatFilesCsv |  37.812 ms | 0.6582 ms | 0.6157 ms |  37.553 ms |  1.63 |    0.03 |  6285.7143 |  71.4286 |        - |  25882.75 KB |
|         FSharpData |  16.071 ms | 0.2683 ms | 0.2378 ms |  15.984 ms |  0.69 |    0.01 | 15406.2500 |  31.2500 |        - |  62950.09 KB |
|       FluentSelect | 128.418 ms | 1.4073 ms | 1.2475 ms | 128.184 ms |  5.53 |    0.09 |          - |        - |        - |   1734.76 KB |
|     MgholamFastCSV |  13.694 ms | 0.2604 ms | 0.4891 ms |  13.635 ms |  0.59 |    0.02 |  1781.2500 | 890.6250 | 218.7500 |  10116.82 KB |
|              NReco |   6.614 ms | 0.1220 ms | 0.1019 ms |   6.617 ms |  0.29 |    0.01 |  1765.6250 |  15.6250 |        - |   7214.94 KB |
|             Sylvan |   5.645 ms | 0.0271 ms | 0.0211 ms |   5.642 ms |  0.24 |    0.00 |  1757.8125 |        - |        - |   7197.21 KB |
|       SylvanSchema |   5.342 ms | 0.0391 ms | 0.0305 ms |   5.337 ms |  0.23 |    0.00 |   203.1250 |  31.2500 |        - |    858.49 KB |
|        NRecoSelect |   2.836 ms | 0.0559 ms | 0.0523 ms |   2.811 ms |  0.12 |    0.00 |   113.2813 |  15.6250 |        - |    471.01 KB |
|       SylvanSelect |   2.418 ms | 0.0126 ms | 0.0105 ms |   2.416 ms |  0.10 |    0.00 |    66.4063 |   7.8125 |        - |    282.22 KB |

### Sync/Async
For libraries that support it, this measures using both the synchronous and asynchronos APIs.

### SylvanSchema
This measures using the Sylvan CsvDataReader with a provided schema.
This adds a slight amount of time to parse the primitive values, but this is time which would be spent in `Parse` methods anyway if consuming only strings.

### *Select
The approach that Sylvan and NReco use for processing CSV make them even more efficient when reading only a subset of the columns in a file. 
These benchmarks measures reading only 3 of the 85 columns.
Notably, these are both faster than even the naive approach to parsing CSV.
