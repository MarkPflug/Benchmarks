# CSV Writer Benchmarks

These benchmarks write a 100k sequence of object data containing several typed columns as well as a "grid" of 20 doubles, to a `TextWriter.Null`.

|          Method |       Mean |     Error |    StdDev | Ratio | RatioSD |       Gen 0 | Gen 1 | Gen 2 |    Allocated |
|---------------- |-----------:|----------:|----------:|------:|--------:|------------:|------:|------:|-------------:|
|   CsvHelperSync |   693.1 ms |  11.96 ms |  10.60 ms |  1.00 |    0.00 |  39000.0000 |     - |     - | 161719.31 KB |
|  CsvHelperAsync | 6,737.3 ms | 127.27 ms | 209.10 ms |  9.81 |    0.40 | 163000.0000 |     - |     - | 662502.94 KB |
|     NaiveBroken |   289.4 ms |   1.67 ms |   1.56 ms |  0.42 |    0.01 |  21000.0000 |     - |     - |  89054.87 KB |
|       NLightCsv |   474.5 ms |   2.11 ms |   1.87 ms |  0.68 |    0.01 |  34000.0000 |     - |     - | 140778.31 KB |
|           NReco |   367.0 ms |   2.58 ms |   2.16 ms |  0.53 |    0.01 |  21000.0000 |     - |     - |  86711.21 KB |
|      SylvanSync |   298.6 ms |   3.19 ms |   2.98 ms |  0.43 |    0.01 |           - |     - |     - |     34.77 KB |
| SylvanDataAsync |   470.0 ms |   4.01 ms |   3.75 ms |  0.68 |    0.01 |           - |     - |     - |    849.66 KB |
|  SylvanDataSync |   466.7 ms |   6.38 ms |   5.96 ms |  0.67 |    0.01 |           - |     - |     - |    849.55 KB |
