# CSV Writer Benchmarks

These benchmarks write 65k "SalesRecord" objects to a CSV file.

| Method                       | Mean     | Error    | StdDev   | Gen0      | Allocated   |
|----------------------------- |---------:|---------:|---------:|----------:|------------:|
| RecordParser_Parallel_Manual | 19.80 ms | 0.602 ms | 0.215 ms |         - |    482.9 KB |
| SylvanDataSync               | 24.18 ms | 0.384 ms | 0.059 ms |         - |   453.99 KB |
| SylvanDataAsync              | 27.44 ms | 0.414 ms | 0.148 ms |         - |   519.49 KB |
| NaiveBroken                  | 32.50 ms | 0.533 ms | 0.190 ms |  812.5000 |  42239.5 KB |
| NReco                        | 41.21 ms | 0.650 ms | 0.101 ms |  846.1538 | 42239.89 KB |
| NLightCsv                    | 44.58 ms | 0.551 ms | 0.143 ms |  833.3333 | 42241.59 KB |
| CsvHelperSync                | 57.02 ms | 1.165 ms | 0.303 ms | 1111.1111 | 55911.09 KB |
| CsvHelperAsync               | 59.16 ms | 0.809 ms | 0.210 ms | 1111.1111 |  55982.1 KB |
| SoftCircuitsWriter           | 68.32 ms | 0.600 ms | 0.093 ms | 1625.0000 | 83404.51 KB |

* Uses 4x parallelization.