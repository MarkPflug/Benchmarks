# CSV Writer Benchmarks

These benchmarks write a 100k sequence of object data containing several typed columns as well as a "grid" of 20 doubles,
to a `TextWriter.Null`.

|          Method |       Mean |     Error |    StdDev | Ratio | RatioSD |       Gen 0 | Gen 1 | Gen 2 |    Allocated |
|---------------- |-----------:|----------:|----------:|------:|--------:|------------:|------:|------:|-------------:|
|     NaiveBroken |   285.3 ms |   0.59 ms |   0.49 ms |  0.44 |    0.00 |  21750.0000 |     - |     - |  89054.41 KB |
|      SylvanSync |   294.9 ms |   0.52 ms |   0.43 ms |  0.45 |    0.00 |           - |     - |     - |     34.31 KB |
|           NReco |   350.1 ms |   0.70 ms |   0.62 ms |  0.53 |    0.00 |  21000.0000 |     - |     - |  86710.75 KB |
|     SylvanAsync |   406.8 ms |   2.15 ms |   2.01 ms |  0.62 |    0.00 |           - |     - |     - |     836.8 KB |
| SylvanDataAsync |   456.1 ms |   8.50 ms |   7.53 ms |  0.70 |    0.01 |           - |     - |     - |     848.7 KB |
|  SylvanDataSync |   468.7 ms |   8.85 ms |  10.53 ms |  0.72 |    0.02 |           - |     - |     - |    848.59 KB |
|       NLightCsv |   477.7 ms |   0.96 ms |   0.80 ms |  0.73 |    0.00 |  34000.0000 |     - |     - | 140777.71 KB |
|   CsvHelperSync |   654.3 ms |   2.63 ms |   2.46 ms |  1.00 |    0.00 |  39000.0000 |     - |     - | 161719.31 KB |
|  CsvHelperAsync | 6,464.6 ms | 126.95 ms | 186.08 ms |  9.92 |    0.33 | 163000.0000 |     - |     - | 662504.84 KB |
