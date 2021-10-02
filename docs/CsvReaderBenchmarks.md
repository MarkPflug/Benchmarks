# CSV Reader Benchmarks

This benchmark set measures CSV readers processing every field in the dataset
as a string. 
The test dataset is 1 million sample sales records from [eforexcel.com].
This measures functionality that is common to all CSV parsers.
This data set does not include any quoted fields, this so doesn't test correctness of any implementation.

|               Method |        Mean |       Error |    StdDev | Ratio | RatioSD |        Gen 0 |     Gen 1 | Allocated |
|--------------------- |------------:|------------:|----------:|------:|--------:|-------------:|----------:|----------:|
|          NaiveBroken |    486.1 ms |    37.94 ms |  13.53 ms |  1.00 |    0.00 |  244000.0000 |         - |    975 MB |
|         CsvHelperCsv |  1,169.8 ms |    38.17 ms |   9.91 ms |  2.40 |    0.08 |  301000.0000 |         - |  1,201 MB |
| NotVBTextFieldParser |  1,342.9 ms |   110.10 ms |  39.26 ms |  2.77 |    0.13 |  504000.0000 |         - |  2,012 MB |
|        FastCsvParser |    707.1 ms |    24.46 ms |   8.72 ms |  1.46 |    0.05 |  134000.0000 |         - |    536 MB |
|           CsvBySteve | 11,261.5 ms | 2,771.08 ms | 988.19 ms | 23.21 |    2.48 | 1315000.0000 | 1000.0000 |  5,246 MB |
|           Lumenworks |  1,072.7 ms |    12.17 ms |   3.16 ms |  2.20 |    0.07 |  632000.0000 |         - |  2,525 MB |
|            NLightCsv |    974.9 ms |    82.35 ms |  21.39 ms |  2.00 |    0.06 |  134000.0000 |         - |    535 MB |
|          VisualBasic |  9,870.7 ms |   410.44 ms | 146.37 ms | 20.32 |    0.66 | 6347000.0000 |         - | 25,333 MB |
|             OleDbCsv |  6,655.1 ms |   289.76 ms |  75.25 ms | 13.63 |    0.25 |  100000.0000 |         - |    403 MB |
|         FlatFilesCsv |  3,347.3 ms |   386.95 ms | 137.99 ms |  6.89 |    0.39 |  469000.0000 |         - |  1,872 MB |
|           FSharpData |  1,262.1 ms |    58.80 ms |  20.97 ms |  2.60 |    0.09 |  940000.0000 |         - |  3,750 MB |
|               Fluent |    560.3 ms |    73.41 ms |  26.18 ms |  1.15 |    0.06 |  244000.0000 |         - |    975 MB |
|       MgholamFastCSV |    548.0 ms |    20.19 ms |   7.20 ms |  1.13 |    0.03 |  140000.0000 |         - |    559 MB |
|         CursivelyCsv |    472.5 ms |    14.38 ms |   5.13 ms |  0.97 |    0.03 |  134000.0000 |         - |    535 MB |
|              CtlData |    496.8 ms |    12.21 ms |   3.17 ms |  1.02 |    0.03 |  310000.0000 |         - |  1,238 MB |
|                NReco |    670.8 ms |    20.13 ms |   5.23 ms |  1.37 |    0.04 |  134000.0000 |         - |    536 MB |
|         SoftCircuits |    597.4 ms |    16.24 ms |   5.79 ms |  1.23 |    0.03 |  219000.0000 |         - |    876 MB |
|               Sylvan |    376.1 ms |    11.44 ms |   4.08 ms |  0.77 |    0.02 |  134000.0000 |         - |    536 MB |

