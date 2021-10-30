# CSV Reader Benchmarks

This benchmark set measures CSV readers processing every field in the dataset
as a string, which is the most basic functionality common to all CSV parsers. 
The test dataset is 65k sample sales records from [eforexcel.com].
This data set does not include any quoted fields, and thus does not test correctness of any implementation.

|               Method |      Mean |     Error |   StdDev |       Gen 0 |    Gen 1 | Allocated |
|--------------------- |----------:|----------:|---------:|------------:|---------:|----------:|
|               Sylvan |  23.66 ms |  0.358 ms | 0.093 ms |   8750.0000 |  31.2500 |     35 MB |
|         CursivelyCsv |  29.71 ms |  0.567 ms | 0.088 ms |   8718.7500 |        - |     35 MB |
|          NaiveBroken |  36.10 ms |  7.509 ms | 2.678 ms |  15875.0000 |        - |     63 MB |
|              CtlData |  32.92 ms |  0.508 ms | 0.181 ms |  20250.0000 |        - |     81 MB |
|       MgholamFastCSV |  34.32 ms |  0.276 ms | 0.043 ms |   9133.3333 |  66.6667 |     37 MB |
|               Fluent |  35.30 ms |  2.085 ms | 0.541 ms |  15857.1429 |        - |     63 MB |
|         SoftCircuits |  37.55 ms |  0.646 ms | 0.100 ms |  14285.7143 |        - |     57 MB |
|                NReco |  44.79 ms |  0.533 ms | 0.082 ms |   8727.2727 |        - |     35 MB |
|        FastCsvParser |  47.75 ms |  0.730 ms | 0.190 ms |   8727.2727 |  90.9091 |     35 MB |
|            NLightCsv |  61.77 ms |  0.871 ms | 0.048 ms |   8666.6667 |        - |     35 MB |
|           Lumenworks |  73.27 ms |  7.900 ms | 2.817 ms |  41250.0000 |        - |    165 MB |
|         CsvHelperCsv |  78.79 ms |  1.269 ms | 0.196 ms |  19571.4286 | 142.8571 |     78 MB |
|           FSharpData |  81.01 ms |  1.303 ms | 0.202 ms |  61428.5714 |        - |    245 MB |
| NotVBTextFieldParser |  81.27 ms |  1.236 ms | 0.321 ms |  32714.2857 |        - |    131 MB |
|         FlatFilesCsv | 194.03 ms |  2.814 ms | 0.731 ms |  30333.3333 |        - |    122 MB |
|          VisualBasic | 591.76 ms | 30.126 ms | 7.824 ms | 285000.0000 |        - |  1,138 MB |
|           CsvBySteve | 593.82 ms | 26.343 ms | 6.841 ms |  86000.0000 |        - |    343 MB |
|             OleDbCsv | 597.38 ms | 24.956 ms | 8.900 ms |   6000.0000 |        - |     26 MB |

