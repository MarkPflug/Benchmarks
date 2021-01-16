# CSV Reader Benchmarks

The main benchmark set measures CSV readers processing every field in the dataset
as a string. This is the functionality that is common to all CSV parsers.

|               Method |       Mean | Ratio | Allocated |
|--------------------- |-----------:|------:|----------:|
|          NaiveBroken |   5.479 ms |  1.00 |  11.59 MB |
|               Sylvan |   5.908 ms |  1.10 |   7.15 MB |
|       MgholamFastCSV |   6.057 ms |  1.12 |    7.7 MB |
|                NReco |   6.606 ms |  1.23 |   7.14 MB |
|              CtlData |   7.099 ms |  1.31 |  19.99 MB |
|         CursivelyCsv |   7.302 ms |  1.36 |      7 MB |
|        FastCsvParser |   9.033 ms |  1.68 |   7.37 MB |
| NotVBTextFieldParser |  12.036 ms |  2.23 |  21.72 MB |
|            NLightCsv |  14.744 ms |  2.74 |   7.17 MB |
|           FSharpData |  16.882 ms |  3.09 |  61.48 MB |
|           Lumenworks |  19.501 ms |  3.62 |  41.92 MB |
|            CsvHelper |  23.961 ms |  4.44 |  26.81 MB |
|         FlatFilesCsv |  40.281 ms |  7.45 |  25.28 MB |
|           CsvBySteve | 103.463 ms | 19.00 |  91.61 MB |
|         FluentSelect | 108.925 ms | 20.25 |   1.69 MB |
|          VisualBasic | 115.329 ms | 21.23 | 182.68 MB |
|             OleDbCsv | 189.211 ms | 34.51 |   6.69 MB |

### Select

Some CSV parsers are more efficient when accessing only a subset of the columns in a file.
These benchmarks measure reading only 3 of the 85 columns. Two of these columns are processed as integer values, and one as a string.
Notably, these are all faster than even the naive approach to parsing CSV.

|               Method |      Mean | Ratio |   Allocated |
|--------------------- |----------:|------:|------------:|
|          NaiveBroken |  5.176 ms |  1.00 | 11867.72 KB |
|   CursivelyCsvSelect |  1.820 ms |  0.35 |   215.28 KB |
|         SylvanSelect |  2.148 ms |  0.41 |   376.25 KB |
| MgholamFastCsvSelect |  2.934 ms |  0.57 |  1140.07 KB |
|          NRecoSelect |  2.944 ms |  0.57 |    570.4 KB |
