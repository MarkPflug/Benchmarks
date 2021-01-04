# CSV Reader Benchmarks

|             Method |       Mean |     Error |    StdDev |     Median | Ratio | RatioSD |      Gen 0 |    Gen 1 |    Gen 2 |    Allocated |
|------------------- |-----------:|----------:|----------:|-----------:|------:|--------:|-----------:|---------:|---------:|-------------:|
|          CsvHelper |  22.882 ms | 0.2537 ms | 0.2249 ms |  22.822 ms |  1.00 |    0.00 |  6000.0000 |        - |        - |  27258.73 KB |
| CsvTextFieldParser |  11.905 ms | 0.2320 ms | 0.2279 ms |  11.882 ms |  0.52 |    0.01 |  5437.5000 |        - |        - |  22235.21 KB |
|      FastCsvParser |   9.029 ms | 0.0309 ms | 0.0289 ms |   9.024 ms |  0.39 |    0.00 |  1828.1250 | 125.0000 |  46.8750 |   7548.88 KB |
|         CsvBySteve | 104.188 ms | 0.7868 ms | 0.6570 ms | 104.167 ms |  4.55 |    0.05 | 22800.0000 |        - |        - |  93808.72 KB |
|         Lumenworks |  19.225 ms | 0.3585 ms | 0.6373 ms |  19.041 ms |  0.86 |    0.03 | 10468.7500 |        - |        - |  42798.25 KB |
|        NaiveBroken |   4.802 ms | 0.0951 ms | 0.1364 ms |   4.773 ms |  0.21 |    0.01 |  2757.8125 |        - |        - |  11266.87 KB |
|          NLightCsv |  13.980 ms | 0.0888 ms | 0.0830 ms |  13.972 ms |  0.61 |    0.01 |  1750.0000 |        - |        - |   7323.02 KB |
|        VisualBasic | 112.028 ms | 1.2861 ms | 1.2030 ms | 112.154 ms |  4.90 |    0.08 | 45600.0000 |        - |        - | 187058.47 KB |
|       FlatFilesCsv |  37.592 ms | 0.4542 ms | 0.4248 ms |  37.422 ms |  1.64 |    0.03 |  6307.6923 |  76.9231 |        - |  25882.76 KB |
|         FSharpData |  16.054 ms | 0.2752 ms | 0.2440 ms |  16.113 ms |  0.70 |    0.01 | 15406.2500 |  15.6250 |        - |  62950.08 KB |
|       FluentSelect | 133.519 ms | 2.8286 ms | 7.5011 ms | 130.604 ms |  5.76 |    0.15 |          - |        - |        - |   1734.95 KB |
|     MgholamFastCSV |  13.294 ms | 0.2631 ms | 0.3132 ms |  13.263 ms |  0.57 |    0.01 |  1765.6250 | 875.0000 | 218.7500 |  10116.91 KB |
|              NReco |   6.527 ms | 0.0726 ms | 0.0606 ms |   6.525 ms |  0.29 |    0.00 |  1765.6250 |  15.6250 |        - |   7214.94 KB |
|             Sylvan |   5.247 ms | 0.0912 ms | 0.0762 ms |   5.219 ms |  0.23 |    0.00 |  1757.8125 |        - |        - |   7197.21 KB |
|       SylvanSchema |   4.938 ms | 0.0840 ms | 0.0786 ms |   4.935 ms |  0.22 |    0.00 |   203.1250 |  31.2500 |        - |    858.49 KB |
|        NRecoSelect |   2.807 ms | 0.0300 ms | 0.0266 ms |   2.801 ms |  0.12 |    0.00 |   113.2813 |  15.6250 |        - |    471.01 KB |
|       SylvanSelect |   1.912 ms | 0.0257 ms | 0.0201 ms |   1.908 ms |  0.08 |    0.00 |    68.3594 |   9.7656 |        - |    282.22 KB |

### Sync/Async
For libraries that support it, this measures using both the synchronous and asynchronos APIs.

### SylvanSchema
This measures using the Sylvan CsvDataReader with a provided schema.
This adds a slight amount of time to parse the primitive values, but this is time which would be spent in `Parse` methods anyway if consuming only strings.

### *Select
The approach that Sylvan and NReco use for processing CSV make them even more efficient when reading only a subset of the columns in a file. 
These benchmarks measures reading only 3 of the 85 columns.
Notably, these are both faster than even the naive approach to parsing CSV.
