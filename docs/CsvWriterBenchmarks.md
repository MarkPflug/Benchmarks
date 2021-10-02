# CSV Writer Benchmarks

These benchmarks write a 100k sequence of object data containing several typed columns as well as a "grid" of 20 doubles,
to a `TextWriter.Null`.

|          Method |       Mean |     Error |    StdDev | Ratio | RatioSD |      Gen 0 |  Allocated |
|---------------- |-----------:|----------:|----------:|------:|--------:|-----------:|-----------:|
|     NaiveBroken |   294.1 ms |  15.12 ms |   5.39 ms |  1.00 |    0.00 | 21500.0000 |  89,055 KB |
|   CsvHelperSync |   632.9 ms |  41.96 ms |  14.96 ms |  2.15 |    0.06 | 34000.0000 | 140,635 KB |
|  CsvHelperAsync | 1,079.0 ms | 433.41 ms | 154.56 ms |  3.67 |    0.53 | 42000.0000 | 174,226 KB |
|       NLightCsv |   399.2 ms |   6.04 ms |   0.93 ms |  1.36 |    0.03 | 34000.0000 | 140,779 KB |
|           NReco |   323.2 ms |   2.96 ms |   0.46 ms |  1.10 |    0.02 | 21000.0000 |  86,712 KB |
| SylvanDataAsync |   289.5 ms |   6.30 ms |   2.25 ms |  0.98 |    0.02 |          - |     373 KB |
|  SylvanDataSync |   272.6 ms |   8.73 ms |   3.11 ms |  0.93 |    0.02 |          - |      46 KB |