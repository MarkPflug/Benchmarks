# CSV Reader Benchmarks

|               Method |       Mean |     Error |    StdDev | Ratio | RatioSD |      Gen 0 |    Gen 1 |   Gen 2 |    Allocated |
|--------------------- |-----------:|----------:|----------:|------:|--------:|-----------:|---------:|--------:|-------------:|
|            CsvHelper |  23.578 ms | 0.1707 ms | 0.1513 ms |  1.00 |    0.00 |  6000.0000 |        - |       - |  27258.73 KB |
| NotVBTextFieldParser |  11.796 ms | 0.1121 ms | 0.0936 ms |  0.50 |    0.01 |  5437.5000 |        - |       - |  22235.21 KB |
|        FastCsvParser |   8.859 ms | 0.0449 ms | 0.0351 ms |  0.38 |    0.00 |  1828.1250 | 125.0000 | 46.8750 |   7548.88 KB |
|           CsvBySteve | 100.752 ms | 0.6834 ms | 0.6058 ms |  4.27 |    0.04 | 22800.0000 |        - |       - |  93808.72 KB |
|           Lumenworks |  18.410 ms | 0.2928 ms | 0.2595 ms |  0.78 |    0.01 | 10468.7500 |        - |       - |  42798.25 KB |
|          NaiveBroken |   4.818 ms | 0.0947 ms | 0.0972 ms |  0.20 |    0.00 |  2757.8125 |        - |       - |  11266.87 KB |
|            NLightCsv |  13.648 ms | 0.0544 ms | 0.0483 ms |  0.58 |    0.00 |  1750.0000 |        - |       - |   7323.29 KB |
|          VisualBasic | 108.579 ms | 1.0928 ms | 1.2146 ms |  4.62 |    0.07 | 45600.0000 |        - |       - | 187058.47 KB |
|             OleDbCsv | 246.026 ms | 3.5010 ms | 2.7333 ms | 10.44 |    0.15 |  1000.0000 |        - |       - |   6848.06 KB |
|         FlatFilesCsv |  38.299 ms | 0.7444 ms | 0.7311 ms |  1.63 |    0.02 |  6285.7143 |  71.4286 |       - |  25882.75 KB |
|           FSharpData |  15.493 ms | 0.3041 ms | 0.2696 ms |  0.66 |    0.01 | 15406.2500 |  31.2500 |       - |  62950.09 KB |
|         FluentSelect |  94.921 ms | 1.0658 ms | 0.8321 ms |  4.03 |    0.04 |          - |        - |       - |   1734.57 KB |
|       MgholamFastCSV |   5.627 ms | 0.0759 ms | 0.0634 ms |  0.24 |    0.00 |  1921.8750 |  62.5000 | 23.4375 |   7881.08 KB |
|                NReco |   6.630 ms | 0.0369 ms | 0.0308 ms |  0.28 |    0.00 |  1781.2500 |  62.5000 | 31.2500 |   7310.73 KB |
|               Sylvan |   5.828 ms | 0.1007 ms | 0.0942 ms |  0.25 |    0.00 |  1781.2500 |  46.8750 | 23.4375 |   7319.95 KB |
|     SylvanSimplePool |   4.198 ms | 0.0306 ms | 0.0256 ms |  0.18 |    0.00 |   460.9375 |  46.8750 | 23.4375 |   1923.53 KB |
|         SylvanSchema |   6.912 ms | 0.0521 ms | 0.0462 ms |  0.29 |    0.00 |   218.7500 |  46.8750 | 23.4375 |    949.15 KB |
| MgholamFastCSVSelect |   2.746 ms | 0.0114 ms | 0.0089 ms |  0.12 |    0.00 |   277.3438 |  62.5000 | 31.2500 |   1136.81 KB |
|          NRecoSelect |   2.795 ms | 0.0122 ms | 0.0102 ms |  0.12 |    0.00 |   136.7188 |  35.1563 | 35.1563 |    567.12 KB |
|         SylvanSelect |   2.021 ms | 0.0131 ms | 0.0123 ms |  0.09 |    0.00 |   140.6250 |  35.1563 | 31.2500 |    576.35 KB |

### SylvanSchema

This measures using the Sylvan CsvDataReader with a provided schema.
This adds some amount of time to process the data as it must parse the values,
but this is time which would be spent in `Parse` methods anyway when using other libraries that only expose strings.

### SylvanSimplePool

This measures applying a very simple string-pooling strategy to the Sylvan data reader where all 1-char-length strings 
are precalculated and reused rather than allocating a new string for each value. Admittedly, this does take some advantage of
fore-knowledge of the dataset.

### *Select
Some CSV parsers are more efficient when reading only a subset of the columns in a file. 
These benchmarks measures reading only 3 of the 85 columns.
Notably, these are all faster than even the naive approach to parsing CSV.
