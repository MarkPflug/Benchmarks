# CSV Reader Benchmarks

This benchmark set measures CSV readers processing every field in the dataset
as a string, which is the most basic functionality common to all CSV parsers. 
The test dataset is 65k sample sales records from [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).
This data set does not include any quoted fields, and thus does not test correctness of any implementation.

|               Method |      Mean |     Error |   StdDev |       Gen 0 |   Gen 1 | Allocated |
|--------------------- |----------:|----------:|---------:|------------:|--------:|----------:|
|               Sylvan |  24.58 ms |  0.476 ms | 0.074 ms |   8750.0000 | 31.2500 |     35 MB |
|         CursivelyCsv |  29.07 ms |  0.306 ms | 0.047 ms |   8718.7500 |       - |     35 MB |
|          NaiveBroken |  30.40 ms |  2.393 ms | 0.621 ms |  15906.2500 |       - |     63 MB |
|              CtlData |  30.81 ms |  0.579 ms | 0.090 ms |  20281.2500 |       - |     81 MB |
|               Fluent |  32.76 ms |  0.600 ms | 0.156 ms |  15866.6667 |       - |     63 MB |
|       MgholamFastCSV |  34.02 ms |  0.213 ms | 0.033 ms |   9133.3333 | 66.6667 |     37 MB |
|         SoftCircuits |  35.09 ms |  1.370 ms | 0.356 ms |  14266.6667 |       - |     57 MB |
|                NReco |  44.24 ms |  6.950 ms | 2.478 ms |   8666.6667 |       - |     35 MB |
|        FastCsvParser |  46.79 ms |  0.902 ms | 0.049 ms |   8727.2727 | 90.9091 |     35 MB |
|         CsvHelperCsv |  47.40 ms |  0.447 ms | 0.069 ms |   8727.2727 |       - |     35 MB |
|            NLightCsv |  63.38 ms |  1.076 ms | 0.279 ms |   8666.6667 |       - |     35 MB |
| NotVBTextFieldParser |  73.79 ms |  1.154 ms | 0.411 ms |  32714.2857 |       - |    131 MB |
|           Lumenworks |  74.85 ms | 12.677 ms | 4.521 ms |  41250.0000 |       - |    165 MB |
|           FSharpData |  81.98 ms |  1.191 ms | 0.065 ms |  61428.5714 |       - |    245 MB |
|         FlatFilesCsv | 193.24 ms |  3.018 ms | 0.784 ms |  30000.0000 |       - |    122 MB |
|          VisualBasic | 505.76 ms |  8.033 ms | 2.086 ms | 285000.0000 |       - |  1,138 MB |
|           CsvBySteve | 540.01 ms |  8.648 ms | 2.246 ms |  86000.0000 |       - |    343 MB |
|             OleDbCsv | 586.51 ms |  6.483 ms | 1.003 ms |   6000.0000 |       - |     26 MB |
