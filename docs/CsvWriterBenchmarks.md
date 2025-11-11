# CSV Writer Benchmarks

These benchmarks write 65k "SalesRecord" objects to a CSV file.

| Method                       | Mean      | Error    | StdDev   | Gen0       | Allocated   |
|----------------------------- |----------:|---------:|---------:|-----------:|------------:|
| RecordParser_Parallel_Manual |  36.23 ms | 0.619 ms | 0.161 ms |          - |   481.99 KB |
| SylvanDataSync               |  37.77 ms | 0.517 ms | 0.134 ms |          - |   454.18 KB |
| SylvanDataAsync              |  43.68 ms | 0.962 ms | 0.343 ms |          - |   519.74 KB |
| NaiveBroken                  |  55.57 ms | 1.988 ms | 0.516 ms | 10222.2222 | 42239.54 KB |
| NReco                        |  72.02 ms | 4.435 ms | 1.582 ms | 10142.8571 | 42240.15 KB |
| NLightCsv                    |  76.46 ms | 1.299 ms | 0.337 ms | 10142.8571 | 42241.16 KB |
| CsvHelperSync                |  93.53 ms | 1.507 ms | 0.233 ms | 13500.0000 | 55913.34 KB |
| CsvHelperAsync               | 100.25 ms | 1.802 ms | 0.468 ms | 13600.0000 | 55984.28 KB |
| SoftCircuitsWriter           | 115.19 ms | 1.937 ms | 0.691 ms | 20400.0000 | 83405.29 KB |

* Uses 4x parallelization.