# CSV Reader Benchmarks

This benchmark set measures CSV readers processing every field in the dataset
as a string, which is the most basic functionality common to all CSV parsers. 
The test dataset is 65k sample sales records from [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).
This data set does not include any quoted fields, and thus does not test correctness of any implementation.

|               Method |      Mean |     Error |   StdDev |        Gen0 |     Gen1 |  Allocated |
|--------------------- |----------:|----------:|---------:|------------:|---------:|-----------:|
|               Sylvan |  21.33 ms |  0.421 ms | 0.150 ms |   8750.0000 |  31.2500 |   34.92 MB |
|          NaiveBroken |  25.00 ms |  0.816 ms | 0.291 ms |  15866.6667 |        - |   63.49 MB |
|         CursivelyCsv |  29.95 ms |  0.586 ms | 0.032 ms |   8718.7500 |        - |   34.88 MB |
|         SoftCircuits |  30.35 ms |  0.548 ms | 0.142 ms |  13781.2500 |        - |      55 MB |
|               Fluent |  31.70 ms |  1.493 ms | 0.532 ms |  15906.2500 |        - |   63.49 MB |
|              CtlData |  32.10 ms |  0.786 ms | 0.280 ms |  20250.0000 |        - |   81.02 MB |
|       MgholamFastCSV |  36.59 ms |  2.892 ms | 0.751 ms |   9142.8571 |        - |   36.59 MB |
|                NReco |  44.37 ms |  0.805 ms | 0.209 ms |   8666.6667 |        - |   35.02 MB |
|        FastCsvParser |  50.58 ms |  0.914 ms | 0.237 ms |   8700.0000 | 100.0000 |   35.22 MB |
|         CsvHelperCsv |  63.65 ms |  2.524 ms | 0.900 ms |   8666.6667 |        - |   35.02 MB |
|            NLightCsv |  66.35 ms |  2.435 ms | 0.868 ms |   8714.2857 |        - |   35.01 MB |
|           Lumenworks |  68.45 ms |  0.682 ms | 0.106 ms |  41250.0000 |        - |  165.12 MB |
| NotVBTextFieldParser |  69.12 ms | 13.862 ms | 4.943 ms |  32666.6667 |        - |  130.79 MB |
|           FSharpData |  76.35 ms |  0.976 ms | 0.254 ms |  59714.2857 |        - |  238.48 MB |
|           CsvBySteve | 197.22 ms |  9.442 ms | 2.452 ms |  84000.0000 |        - |  336.26 MB |
|         FlatFilesCsv | 197.66 ms |  2.844 ms | 1.014 ms |  34666.6667 |        - |  138.28 MB |
|          VisualBasic | 529.88 ms |  8.794 ms | 3.136 ms | 285000.0000 |        - | 1138.07 MB |