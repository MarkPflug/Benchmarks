# CSV Writer Benchmarks

These benchmarks write a 100k sequence of object data containing several typed columns as well as a "grid" of 20 doubles,
to a `TextWriter.Null`.

|          Method |     Mean |    Error |   StdDev |      Gen 0 |  Allocated |
|---------------- |---------:|---------:|---------:|-----------:|-----------:|
|     NaiveBroken | 261.4 ms | 10.79 ms |  2.80 ms | 21500.0000 |  89,055 KB |
|  SylvanDataSync | 276.4 ms | 24.30 ms |  6.31 ms |          - |      46 KB |
| SylvanDataAsync | 285.4 ms | 21.69 ms |  5.63 ms |          - |     373 KB |
|           NReco | 317.0 ms |  2.42 ms |  0.13 ms | 21000.0000 |  86,711 KB |
|       NLightCsv | 395.1 ms | 17.80 ms |  6.35 ms | 34000.0000 | 140,779 KB |
|   CsvHelperSync | 600.6 ms | 36.53 ms |  9.49 ms | 34000.0000 | 140,635 KB |
|  CsvHelperAsync | 824.4 ms | 55.82 ms | 14.50 ms | 42000.0000 | 174,226 KB |