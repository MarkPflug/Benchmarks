# CSV Reader Benchmarks

This benchmark set measures CSV readers processing every field in the dataset
as a string, which is the most basic functionality common to all CSV parsers. 
The test dataset is 65k sample sales records from [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).
This data set does not include any quoted fields, and thus does not test correctness of any implementation.

|               Method |      Mean |      Error |    StdDev |       Gen 0 |   Gen 1 | Gen 2 |  Allocated |
|--------------------- |----------:|-----------:|----------:|------------:|--------:|------:|-----------:|
|               Sylvan |  23.25 ms |   0.351 ms |  0.019 ms |   8750.0000 | 31.2500 |     - |   34.92 MB |
|          NaiveBroken |  29.86 ms |   1.956 ms |  0.697 ms |  15906.2500 |       - |     - |   63.49 MB |
|         CursivelyCsv |  29.96 ms |   0.569 ms |  0.088 ms |   8718.7500 |       - |     - |   34.88 MB |
|              CtlData |  31.46 ms |   0.559 ms |  0.086 ms |  20250.0000 |       - |     - |   81.02 MB |
|               Fluent |  34.07 ms |   0.657 ms |  0.171 ms |  15866.6667 |       - |     - |    63.5 MB |
|       MgholamFastCSV |  34.08 ms |   0.536 ms |  0.139 ms |   9133.3333 | 66.6667 |     - |   36.59 MB |
|         SoftCircuits |  35.75 ms |   1.914 ms |  0.683 ms |  13785.7143 |       - |     - |      55 MB |
|                NReco |  44.65 ms |   0.743 ms |  0.265 ms |   8666.6667 |       - |     - |   35.02 MB |
|        FastCsvParser |  49.57 ms |   0.918 ms |  0.238 ms |   8727.2727 | 90.9091 |     - |   35.22 MB |
|         CsvHelperCsv |  51.90 ms |  13.480 ms |  4.807 ms |   8700.0000 |       - |     - |   35.02 MB |
|            NLightCsv |  66.69 ms |   9.247 ms |  3.298 ms |   8625.0000 |       - |     - |   35.01 MB |
|           Lumenworks |  72.26 ms |   8.178 ms |  2.124 ms |  41250.0000 |       - |     - |  165.12 MB |
| NotVBTextFieldParser |  77.99 ms |   3.569 ms |  1.273 ms |  32714.2857 |       - |     - |  130.79 MB |
|           FSharpData |  80.40 ms |   1.309 ms |  0.340 ms |  59666.6667 |       - |     - |  238.48 MB |
|         FlatFilesCsv | 206.74 ms |  10.560 ms |  3.766 ms |  34666.6667 |       - |     - |  138.28 MB |
|           CsvBySteve | 467.87 ms | 134.728 ms | 48.045 ms |  84000.0000 |       - |     - |  336.26 MB |
|          VisualBasic | 480.00 ms |  22.836 ms |  8.144 ms | 285000.0000 |       - |     - | 1138.07 MB |
