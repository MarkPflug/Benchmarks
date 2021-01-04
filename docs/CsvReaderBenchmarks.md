# CSV Reader Benchmarks

|             Method |       Mean |     Error |     StdDev |     Median | Ratio | RatioSD |      Gen 0 |    Gen 1 |    Gen 2 |    Allocated |
|------------------- |-----------:|----------:|-----------:|-----------:|------:|--------:|-----------:|---------:|---------:|-------------:|
|          CsvHelper |  23.200 ms | 0.2971 ms |  0.2634 ms |  23.203 ms |  1.00 |    0.00 |  6000.0000 |        - |        - |  27258.73 KB |
| CsvTextFieldParser |  12.272 ms | 0.2414 ms |  0.4593 ms |  12.130 ms |  0.54 |    0.03 |  5437.5000 |        - |        - |  22235.21 KB |
|      FastCsvParser |   9.260 ms | 0.1804 ms |  0.2006 ms |   9.200 ms |  0.40 |    0.01 |  1828.1250 | 125.0000 |  46.8750 |   7548.88 KB |
|         CsvBySteve | 109.998 ms | 2.1904 ms |  5.7705 ms | 108.898 ms |  4.68 |    0.11 | 22800.0000 |        - |        - |  93808.72 KB |
|         Lumenworks |  18.351 ms | 0.0907 ms |  0.0757 ms |  18.340 ms |  0.79 |    0.01 | 10468.7500 |        - |        - |  42798.25 KB |
|        NaiveBroken |   4.905 ms | 0.0950 ms |  0.1301 ms |   4.888 ms |  0.21 |    0.01 |  2757.8125 |        - |        - |  11266.87 KB |
|          NLightCsv |  13.723 ms | 0.0534 ms |  0.0417 ms |  13.716 ms |  0.59 |    0.01 |  1765.6250 |        - |        - |   7334.77 KB |
|        VisualBasic | 112.043 ms | 2.0726 ms |  1.9387 ms | 111.087 ms |  4.83 |    0.10 | 45600.0000 |        - |        - | 187058.47 KB |
|       FlatFilesCsv |  38.025 ms | 0.3301 ms |  0.2757 ms |  38.030 ms |  1.64 |    0.02 |  6285.7143 |  71.4286 |        - |  25882.75 KB |
|         FSharpData |  16.068 ms | 0.1166 ms |  0.1034 ms |  16.067 ms |  0.69 |    0.01 | 15406.2500 |  31.2500 |        - |  62950.09 KB |
|       FluentSelect | 104.406 ms | 4.5748 ms | 12.7526 ms |  97.699 ms |  4.53 |    0.52 |          - |        - |        - |   1734.64 KB |
|     MgholamFastCSV |  13.677 ms | 0.2718 ms |  0.5364 ms |  13.693 ms |  0.58 |    0.02 |  1765.6250 | 875.0000 | 218.7500 |  10116.81 KB |
|              NReco |   6.441 ms | 0.0460 ms |  0.0407 ms |   6.440 ms |  0.28 |    0.00 |  1765.6250 |  15.6250 |        - |   7214.94 KB |
|        NRecoSelect |   2.751 ms | 0.0244 ms |  0.0228 ms |   2.745 ms |  0.12 |    0.00 |   113.2813 |  15.6250 |        - |    471.01 KB |
|             Sylvan |   5.734 ms | 0.0655 ms |  0.0547 ms |   5.716 ms |  0.25 |    0.00 |  1757.8125 |        - |        - |    7197.2 KB |
|       SylvanSchema |   5.502 ms | 0.1040 ms |  0.1022 ms |   5.452 ms |  0.24 |    0.00 |   203.1250 |  31.2500 |        - |    858.49 KB |
|       SylvanSelect |   2.420 ms | 0.0123 ms |  0.0109 ms |   2.417 ms |  0.10 |    0.00 |    66.4063 |   7.8125 |        - |    282.22 KB |

### Sync/Async
For libraries that support it, this measures using both the synchronous and asynchronos APIs.

### SylvanSchema
This measures using the Sylvan CsvDataReader with a provided schema.
This adds a slight amount of time to parse the primitive values, but this is time which would be spent in `Parse` methods anyway if consuming only strings.

### *Select
The approach that Sylvan and NReco use for processing CSV make them even more efficient when reading only a subset of the columns in a file. 
These benchmarks measures reading only 3 of the 85 columns.
Notably, these are both faster than even the naive approach to parsing CSV.
