# CSV Reader Benchmarks

|             Method |       Mean |     Error |    StdDev |     Median | Ratio | RatioSD |      Gen 0 |    Gen 1 |    Gen 2 |    Allocated |
|------------------- |-----------:|----------:|----------:|-----------:|------:|--------:|-----------:|---------:|---------:|-------------:|
|          CsvHelper |  23.445 ms | 0.4663 ms | 0.8527 ms |  23.238 ms |  1.00 |    0.00 |  6000.0000 |        - |        - |  27258.73 KB |
| CsvTextFieldParser |  11.993 ms | 0.2216 ms | 0.2073 ms |  11.866 ms |  0.51 |    0.02 |  5437.5000 |        - |        - |  22235.21 KB |
|      FastCsvParser |   9.180 ms | 0.0366 ms | 0.0343 ms |   9.171 ms |  0.39 |    0.01 |  1828.1250 | 125.0000 |  46.8750 |   7548.89 KB |
|         CsvBySteve | 127.528 ms | 2.4387 ms | 6.3386 ms | 125.299 ms |  5.43 |    0.33 | 22000.0000 |        - |        - |  90289.51 KB |
|         Lumenworks |  18.440 ms | 0.3656 ms | 0.5359 ms |  18.212 ms |  0.78 |    0.04 | 10468.7500 |        - |        - |  42798.25 KB |
|        NaiveBroken |   4.929 ms | 0.0810 ms | 0.1081 ms |   4.898 ms |  0.21 |    0.01 |  2757.8125 |        - |        - |  11266.87 KB |
|          NLightCsv |  13.941 ms | 0.0676 ms | 0.0632 ms |  13.939 ms |  0.59 |    0.02 |  1750.0000 |        - |        - |   7323.02 KB |
|        VisualBasic | 109.613 ms | 0.6261 ms | 0.4888 ms | 109.550 ms |  4.68 |    0.17 | 45600.0000 |        - |        - | 187058.47 KB |
|           OleDbCsv | 197.860 ms | 3.8904 ms | 3.8209 ms | 198.721 ms |  8.39 |    0.38 |  1000.0000 |        - |        - |   7812.21 KB |
|       FlatFilesCsv |  37.389 ms | 0.6692 ms | 0.6260 ms |  37.046 ms |  1.59 |    0.07 |  6285.7143 |  71.4286 |        - |  25882.75 KB |
|         FSharpData |  16.001 ms | 0.1000 ms | 0.0935 ms |  15.975 ms |  0.68 |    0.02 | 15406.2500 |  31.2500 |        - |  62950.09 KB |
|              NReco |   6.677 ms | 0.0321 ms | 0.0300 ms |   6.674 ms |  0.28 |    0.01 |  1765.6250 |  15.6250 |        - |   7214.94 KB |
|             Sylvan |   5.789 ms | 0.0238 ms | 0.0198 ms |   5.791 ms |  0.25 |    0.01 |  1757.8125 |        - |        - |   7197.21 KB |
|       SylvanSchema |   5.376 ms | 0.0192 ms | 0.0160 ms |   5.375 ms |  0.23 |    0.01 |   203.1250 |  31.2500 |        - |    858.49 KB |
|        NRecoSelect |   2.918 ms | 0.0581 ms | 0.1032 ms |   2.899 ms |  0.12 |    0.01 |   113.2813 |  15.6250 |        - |    471.01 KB |
|       SylvanSelect |   2.436 ms | 0.0149 ms | 0.0116 ms |   2.439 ms |  0.10 |    0.00 |    66.4063 |   7.8125 |        - |    282.22 KB |

### Sync/Async
For libraries that support it, this measures using both the synchronous and asynchronos APIs.

### SylvanSchema
This measures using the Sylvan CsvDataReader with a provided schema.
This adds a slight amount of time to parse the primitive values, but this is time which would be spent in `Parse` methods anyway if consuming only strings.

### *Select
The approach that Sylvan and NReco use for processing CSV make them even more efficient when reading only a subset of the columns in a file. 
These benchmarks measures reading only 3 of the 85 columns.
Notably, these are both faster than even the naive approach to parsing CSV.
