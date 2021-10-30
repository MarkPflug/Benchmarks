# CSV Reader Benchmarks

This benchmark set measures CSV readers processing every field in the dataset
as a string, which is the most basic functionality common to all CSV parsers. 
The test dataset is 65k sample sales records from [eforexcel.com].
This data set does not include any quoted fields, and thus does not test correctness of any implementation.

|               Method |      Mean |     Error |    StdDev |       Gen 0 |   Gen 1 | Allocated |
|--------------------- |----------:|----------:|----------:|------------:|--------:|----------:|
|               Sylvan |  22.89 ms |  0.250 ms |  0.039 ms |   8750.0000 | 31.2500 |     35 MB |
|         CursivelyCsv |  29.65 ms |  0.529 ms |  0.137 ms |   8718.7500 |       - |     35 MB |
|          NaiveBroken |  32.51 ms |  0.565 ms |  0.201 ms |  15875.0000 |       - |     63 MB |
|              CtlData |  33.57 ms |  3.374 ms |  1.203 ms |  20250.0000 |       - |     81 MB |
|       MgholamFastCSV |  34.05 ms |  0.683 ms |  0.177 ms |   9133.3333 | 66.6667 |     37 MB |
|               Fluent |  34.81 ms |  1.565 ms |  0.558 ms |  15866.6667 |       - |     63 MB |
|         SoftCircuits |  36.94 ms |  0.639 ms |  0.166 ms |  14285.7143 |       - |     57 MB |
|                NReco |  42.05 ms |  1.490 ms |  0.387 ms |   8666.6667 |       - |     35 MB |
|         CsvHelperCsv |  46.13 ms |  0.644 ms |  0.167 ms |   8727.2727 |       - |     35 MB |
|        FastCsvParser |  47.22 ms |  0.459 ms |  0.071 ms |   8727.2727 | 90.9091 |     35 MB |
|            NLightCsv |  60.44 ms |  0.634 ms |  0.098 ms |   8666.6667 |       - |     35 MB |
|           Lumenworks |  68.08 ms |  0.297 ms |  0.016 ms |  41250.0000 |       - |    165 MB |
|           FSharpData |  80.89 ms |  2.747 ms |  0.713 ms |  61500.0000 |       - |    245 MB |
| NotVBTextFieldParser |  83.42 ms |  4.155 ms |  1.482 ms |  32666.6667 |       - |    131 MB |
|         FlatFilesCsv | 193.94 ms |  6.595 ms |  1.713 ms |  30333.3333 |       - |    122 MB |
|           CsvBySteve | 570.53 ms | 14.832 ms |  3.852 ms |  86000.0000 |       - |    343 MB |
|             OleDbCsv | 609.13 ms | 51.975 ms | 18.535 ms |   6000.0000 |       - |     26 MB |
|          VisualBasic | 652.82 ms | 91.624 ms | 32.674 ms | 285000.0000 |       - |  1,138 MB |

