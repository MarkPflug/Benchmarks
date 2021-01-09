# CSV Reader Benchmarks

|               Method |       Mean |     Error |    StdDev |     Median | Ratio | RatioSD |      Gen 0 |    Gen 1 |   Gen 2 |    Allocated |
|--------------------- |-----------:|----------:|----------:|-----------:|------:|--------:|-----------:|---------:|--------:|-------------:|
|            CsvHelper |  23.575 ms | 0.1881 ms | 0.1571 ms |  23.574 ms |  1.00 |    0.00 |  6000.0000 |        - |       - |  27259.29 KB |
| NotVBTextFieldParser |  11.632 ms | 0.0707 ms | 0.0661 ms |  11.634 ms |  0.49 |    0.00 |  5437.5000 |        - |       - |  22235.21 KB |
|        FastCsvParser |   9.129 ms | 0.1331 ms | 0.1180 ms |   9.141 ms |  0.39 |    0.01 |  1828.1250 | 125.0000 | 46.8750 |   7548.93 KB |
|           CsvBySteve | 105.897 ms | 1.2288 ms | 1.0893 ms | 106.004 ms |  4.49 |    0.04 | 22800.0000 |        - |       - |  93808.72 KB |
|           Lumenworks |  19.299 ms | 0.3829 ms | 0.3933 ms |  19.132 ms |  0.82 |    0.02 | 10468.7500 |        - |       - |  42798.25 KB |
|          NaiveBroken |   4.685 ms | 0.0618 ms | 0.0578 ms |   4.688 ms |  0.20 |    0.00 |  2757.8125 |        - |       - |  11266.87 KB |
|            NLightCsv |  13.704 ms | 0.1127 ms | 0.0880 ms |  13.701 ms |  0.58 |    0.00 |  1765.6250 |        - |       - |   7334.02 KB |
|          VisualBasic | 111.378 ms | 1.6323 ms | 1.3631 ms | 110.913 ms |  4.72 |    0.07 | 45600.0000 |        - |       - | 187058.47 KB |
|             OleDbCsv | 240.748 ms | 4.7353 ms | 7.3723 ms | 245.435 ms | 10.28 |    0.33 |  1000.0000 |        - |       - |   6848.06 KB |
|         FlatFilesCsv |  37.257 ms | 0.2655 ms | 0.2483 ms |  37.247 ms |  1.58 |    0.01 |  6285.7143 |  71.4286 |       - |  25882.75 KB |
|           FSharpData |  15.379 ms | 0.1102 ms | 0.1031 ms |  15.341 ms |  0.65 |    0.01 | 15406.2500 |  15.6250 |       - |  62950.08 KB |
|         FluentSelect | 128.655 ms | 1.1743 ms | 0.9168 ms | 128.612 ms |  5.46 |    0.05 |          - |        - |       - |   1734.76 KB |
|       MgholamFastCSV |   6.035 ms | 0.0710 ms | 0.0664 ms |   6.010 ms |  0.26 |    0.00 |  1921.8750 |  62.5000 | 23.4375 |   7881.09 KB |
|              CtlData |   7.029 ms | 0.1335 ms | 0.1183 ms |   7.017 ms |  0.30 |    0.00 |  5000.0000 |  70.3125 | 23.4375 |  20463.27 KB |
|                NReco |   6.714 ms | 0.1311 ms | 0.1839 ms |   6.679 ms |  0.28 |    0.01 |  1781.2500 |  62.5000 | 31.2500 |   7310.73 KB |
|               Sylvan |   6.200 ms | 0.1161 ms | 0.1141 ms |   6.163 ms |  0.26 |    0.01 |  1781.2500 |  46.8750 | 23.4375 |   7319.95 KB |
|     SylvanSimplePool |   4.361 ms | 0.0247 ms | 0.0206 ms |   4.356 ms |  0.18 |    0.00 |   460.9375 |  46.8750 | 23.4375 |   1923.49 KB |
|         SylvanSchema |   7.116 ms | 0.0430 ms | 0.0336 ms |   7.106 ms |  0.30 |    0.00 |   218.7500 |  46.8750 | 23.4375 |    949.16 KB |
| MgholamFastCSVSelect |   2.626 ms | 0.0512 ms | 0.0547 ms |   2.606 ms |  0.11 |    0.00 |   277.3438 |  62.5000 | 31.2500 |   1136.87 KB |
|          NRecoSelect |   2.688 ms | 0.0330 ms | 0.0293 ms |   2.684 ms |  0.11 |    0.00 |   136.7188 |  35.1563 | 35.1563 |    567.15 KB |
|         SylvanSelect |   2.294 ms | 0.0097 ms | 0.0091 ms |   2.296 ms |  0.10 |    0.00 |   140.6250 |  35.1563 | 31.2500 |    576.34 KB |

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
