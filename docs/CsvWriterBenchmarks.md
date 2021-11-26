# CSV Writer Benchmarks

These benchmarks write a 100k sequence of object data containing several typed columns as well as a "grid" of 20 doubles,
to a `TextWriter.Null`.

|          Method |     Mean |    Error |   StdDev |      Gen 0 |  Allocated |
|---------------- |---------:|---------:|---------:|-----------:|-----------:|
|     NaiveBroken | 240.9 ms |  6.37 ms |  2.27 ms | 21666.6667 |  89,056 KB |
|  SylvanDataSync | 242.5 ms |  8.50 ms |  0.47 ms |          - |      46 KB |
| SylvanDataAsync | 256.5 ms |  4.14 ms |  1.08 ms |          - |     375 KB |
|           NReco | 298.1 ms |  4.72 ms |  0.73 ms | 21000.0000 |  86,711 KB |
|       NLightCsv | 378.4 ms | 79.55 ms | 28.37 ms | 34000.0000 | 140,779 KB |
|   CsvHelperSync | 539.2 ms | 23.71 ms |  8.46 ms | 34000.0000 | 140,636 KB |
|  CsvHelperAsync | 699.7 ms | 85.42 ms | 30.46 ms | 42000.0000 | 174,228 KB |