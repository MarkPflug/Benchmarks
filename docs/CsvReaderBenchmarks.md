# CSV Reader Benchmarks

|               Method |       Mean | Ratio |   Allocated |
|--------------------- |-----------:|------:|------------:|
|            CsvHelper |  23.666 ms |  1.00 | 27261.96 KB |
| NotVBTextFieldParser |  11.900 ms |  0.50 | 22238.45 KB |
|        FastCsvParser |   8.824 ms |  0.37 |  7548.92 KB |
|           CsvBySteve | 106.636 ms |  4.49 | 93808.72 KB |
|           Lumenworks |  18.742 ms |  0.80 | 42801.48 KB |
|          NaiveBroken |   5.053 ms |  0.21 | 11867.72 KB |
|            NLightCsv |  13.833 ms |  0.58 |  7327.44 KB |
|          VisualBasic | 108.816 ms |  4.59 | 187061.7 KB |
|             OleDbCsv | 237.363 ms | 10.03 |  6848.06 KB |
|         FlatFilesCsv |  37.692 ms |  1.59 | 25885.99 KB |
|           FSharpData |  15.733 ms |  0.66 | 62953.31 KB |
|         FluentSelect |  94.929 ms |  4.01 |  1734.57 KB |
|       MgholamFastCSV |   6.071 ms |  0.26 |  7884.31 KB |
|         CursivelyCsv |   6.927 ms |  0.29 |   7164.4 KB |
|              CtlData |   7.059 ms |  0.30 |  20466.5 KB |
|                NReco |   6.672 ms |  0.28 |  7313.96 KB |
|               Sylvan |   5.770 ms |  0.24 |  7323.12 KB |
|     SylvanSimplePool |   4.386 ms |  0.19 |  1926.67 KB |
|         SylvanSchema |   7.136 ms |  0.30 |   952.35 KB |
| MgholamFastCsvSelect |   2.928 ms |  0.12 |  1140.07 KB |
|          NRecoSelect |   2.959 ms |  0.13 |   570.38 KB |
|   CursivelyCsvSelect |   1.732 ms |  0.07 |   214.35 KB |
|         SylvanSelect |   2.082 ms |  0.09 |   376.23 KB |

### SylvanSchema

This measures using the Sylvan CsvDataReader with a provided schema and processing columns as their appropriate type.
This adds some amount of time to process the data as it must parse the values,
but this is time which would be spent in `Parse` methods anyway when using other libraries that only expose strings.

### SylvanSimplePool

This measures applying a very simple string-pooling strategy to the Sylvan data reader where all 1-char-length strings 
are precalculated and reused rather than allocating a new string for each value. Admittedly, this does take some advantage of
fore-knowledge of the dataset.

### *Select
Some CSV parsers are more efficient when reading only a subset of the columns in a file. 
These benchmarks measures reading only 3 of the 85 columns. Two of these columns are processed as integer values, and one as a string.
Notably, these are all faster than even the naive approach to parsing CSV.
