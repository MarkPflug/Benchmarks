# CSV Reader Benchmarks

This benchmark set measures CSV readers processing every field in the dataset
as a string, which is the most basic functionality common to all CSV parsers. 
The test dataset is 65k sample sales records.
This data set does not include any quoted fields, and thus does not test correctness of any implementation.

|               Method |      Mean |    Error |   StdDev |        Gen0 |     Gen1 |  Allocated |
|--------------------- |----------:|---------:|---------:|------------:|---------:|-----------:|
|               Sylvan |  20.71 ms | 0.229 ms | 0.059 ms |   8750.0000 |  31.2500 |   34.92 MB |
|          NaiveBroken |  25.18 ms | 0.333 ms | 0.119 ms |  15906.2500 |        - |   63.49 MB |
|         SoftCircuits |  27.22 ms | 0.318 ms | 0.083 ms |   8718.7500 |        - |   34.89 MB |
|         CursivelyCsv |  29.69 ms | 0.576 ms | 0.150 ms |   8718.7500 |        - |   34.88 MB |
|               Fluent |  30.56 ms | 0.491 ms | 0.175 ms |  15906.2500 |        - |   63.49 MB |
|              CtlData |  32.23 ms | 0.591 ms | 0.091 ms |  20266.6667 |        - |   81.02 MB |
|       MgholamFastCSV |  37.07 ms | 4.779 ms | 1.704 ms |   9083.3333 |        - |   36.59 MB |
|                NReco |  45.00 ms | 0.759 ms | 0.118 ms |   8727.2727 |        - |   35.02 MB |
|        FastCsvParser |  51.81 ms | 0.995 ms | 0.355 ms |   8700.0000 | 100.0000 |   35.22 MB |
|         CsvHelperCsv |  61.29 ms | 0.797 ms | 0.123 ms |   8666.6667 |        - |   35.02 MB |
| NotVBTextFieldParser |  64.29 ms | 1.017 ms | 0.264 ms |  32750.0000 |        - |  130.79 MB |
|            NLightCsv |  64.81 ms | 0.898 ms | 0.139 ms |   8625.0000 |        - |   35.01 MB |
|           Lumenworks |  69.66 ms | 1.084 ms | 0.282 ms |  41250.0000 |        - |  165.12 MB |
|           FSharpData |  74.10 ms | 1.076 ms | 0.384 ms |  59714.2857 |        - |  238.48 MB |
|           CsvBySteve | 193.03 ms | 3.016 ms | 0.467 ms |  84000.0000 |        - |  336.26 MB |
|         FlatFilesCsv | 199.73 ms | 8.860 ms | 3.160 ms |  34666.6667 |        - |  138.28 MB |
|          VisualBasic | 517.93 ms | 7.740 ms | 2.010 ms | 285000.0000 |        - | 1138.07 MB |
|             OleDbCsv | 520.73 ms | 5.305 ms | 0.821 ms |   6000.0000 |        - |   26.39 MB |