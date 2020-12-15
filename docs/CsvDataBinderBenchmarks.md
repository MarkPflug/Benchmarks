# CSV Data Binder Benchmarks

These benchmarks test binding CSV data to a strongly-typed .NET object.

|          Method |      Mean |     Error |    StdDev |    Median | Ratio | RatioSD |      Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|---------------- |----------:|----------:|----------:|----------:|------:|--------:|-----------:|---------:|---------:|----------:|
|       CsvHelper | 33.737 ms | 0.6698 ms | 1.4703 ms | 33.214 ms |  1.00 |    0.00 |  7000.0000 |        - |        - |  29.27 MB |
| CsvHelperManual | 25.468 ms | 0.2965 ms | 0.2476 ms | 25.477 ms |  0.72 |    0.02 |  6843.7500 |  31.2500 |        - |  27.32 MB |
|           Cesil | 24.086 ms | 0.3735 ms | 0.3119 ms | 24.019 ms |  0.68 |    0.03 |   312.5000 |  31.2500 |        - |   1.28 MB |
|      SylvanData |  4.446 ms | 0.0748 ms | 0.0625 ms |  4.424 ms |  0.12 |    0.00 |   289.0625 |  54.6875 |        - |   1.18 MB |
|    SylvanManual |  3.864 ms | 0.0463 ms | 0.0387 ms |  3.861 ms |  0.11 |    0.00 |   285.1563 |  46.8750 |        - |   1.14 MB |
|    SylvanDapper |  4.484 ms | 0.0890 ms | 0.0874 ms |  4.473 ms |  0.13 |    0.00 |   375.0000 |  62.5000 |        - |   1.52 MB |
|         TinyCsv | 18.915 ms | 0.3766 ms | 0.6886 ms | 18.727 ms |  0.55 |    0.03 | 13156.2500 | 437.5000 | 187.5000 |  54.91 MB |

### Manual

This tests the performance of hand-written code that reads data fields and assigns them to object properties.

### Dapper

This tests the performance of using Dapper's `GetRowParser<T>()` as a data binder. It requires that the data
source has a strongly-typed schema, so not all CSV libraries can be used with it.