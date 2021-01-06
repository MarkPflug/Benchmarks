# CSV Reader Benchmarks

|               Method |       Mean |     Error |    StdDev |     Median | Ratio | RatioSD |      Gen 0 |    Gen 1 |   Gen 2 |    Allocated |
|--------------------- |-----------:|----------:|----------:|-----------:|------:|--------:|-----------:|---------:|--------:|-------------:|
|            CsvHelper |  23.652 ms | 0.2653 ms | 0.2482 ms |  23.681 ms |  1.00 |    0.00 |  6000.0000 |        - |       - |  27258.73 KB |
| NotVBTextFieldParser |  11.726 ms | 0.0510 ms | 0.0426 ms |  11.742 ms |  0.50 |    0.01 |  5437.5000 |        - |       - |  22235.21 KB |
|        FastCsvParser |   9.192 ms | 0.1777 ms | 0.1975 ms |   9.130 ms |  0.39 |    0.01 |  1828.1250 | 125.0000 | 46.8750 |   7548.88 KB |
|           CsvBySteve | 105.231 ms | 1.9321 ms | 2.5793 ms | 104.464 ms |  4.43 |    0.14 | 22800.0000 |        - |       - |  93808.72 KB |
|           Lumenworks |  19.660 ms | 0.3930 ms | 0.3282 ms |  19.685 ms |  0.83 |    0.02 | 10468.7500 |        - |       - |  42798.25 KB |
|          NaiveBroken |   4.902 ms | 0.0979 ms | 0.1766 ms |   4.882 ms |  0.21 |    0.01 |  2757.8125 |        - |       - |  11266.87 KB |
|            NLightCsv |  14.066 ms | 0.0792 ms | 0.0661 ms |  14.064 ms |  0.59 |    0.01 |  1750.0000 |        - |       - |   7322.86 KB |
|          VisualBasic | 108.959 ms | 0.8164 ms | 0.6817 ms | 108.916 ms |  4.61 |    0.07 | 45600.0000 |        - |       - | 187058.47 KB |
|             OleDbCsv | 240.699 ms | 4.7798 ms | 7.1542 ms | 244.503 ms | 10.13 |    0.29 |  1000.0000 |        - |       - |   6848.06 KB |
|         FlatFilesCsv |  37.828 ms | 0.3635 ms | 0.3223 ms |  37.762 ms |  1.60 |    0.02 |  6285.7143 |  71.4286 |       - |  25882.75 KB |
|           FSharpData |  16.255 ms | 0.3230 ms | 0.3456 ms |  16.287 ms |  0.69 |    0.02 | 15406.2500 |  31.2500 |       - |  62950.09 KB |
|         FluentSelect |  96.307 ms | 1.6207 ms | 1.2654 ms |  96.509 ms |  4.07 |    0.04 |          - |        - |       - |   1734.76 KB |
|       MgholamFastCSV |   5.767 ms | 0.0548 ms | 0.0458 ms |   5.768 ms |  0.24 |    0.00 |  1921.8750 |  62.5000 | 23.4375 |   7881.07 KB |
|                NReco |   6.811 ms | 0.0328 ms | 0.0256 ms |   6.812 ms |  0.29 |    0.00 |  1781.2500 |  62.5000 | 31.2500 |   7310.73 KB |
|               Sylvan |   5.933 ms | 0.0630 ms | 0.0526 ms |   5.922 ms |  0.25 |    0.00 |  1781.2500 |  46.8750 | 23.4375 |   7319.95 KB |
|     SylvanSimplePool |   4.295 ms | 0.0276 ms | 0.0230 ms |   4.291 ms |  0.18 |    0.00 |   460.9375 |  46.8750 | 23.4375 |   1923.49 KB |
|         SylvanSchema |   9.963 ms | 0.1943 ms | 0.1818 ms |   9.892 ms |  0.42 |    0.01 |   187.5000 |  15.6250 |       - |    949.12 KB |
| MgholamFastCSVSelect |   2.661 ms | 0.0396 ms | 0.0351 ms |   2.650 ms |  0.11 |    0.00 |   277.3438 |  62.5000 | 31.2500 |   1136.83 KB |
|          NRecoSelect |   2.724 ms | 0.0145 ms | 0.0113 ms |   2.725 ms |  0.12 |    0.00 |   136.7188 |  35.1563 | 35.1563 |    567.15 KB |
|         SylvanSelect |   2.333 ms | 0.0424 ms | 0.0580 ms |   2.315 ms |  0.10 |    0.00 |   140.6250 |  42.9688 | 31.2500 |    576.36 KB |


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
