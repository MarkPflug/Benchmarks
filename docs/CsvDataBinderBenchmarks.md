# CSV Data Binder Benchmarks

These benchmarks test binding CSV data to a strongly-typed .NET object.

|           Method |      Mean |     Error |    StdDev | Ratio |      Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|----------------- |----------:|----------:|----------:|------:|-----------:|---------:|---------:|----------:|
|    CsvHelperAuto | 26.058 ms | 0.1760 ms | 0.1470 ms |  1.00 |  6000.0000 |        - |        - |  27.67 MB |
|  CsvHelperManual | 25.624 ms | 0.3588 ms | 0.2996 ms |  0.98 |  6843.7500 |  31.2500 |        - |  27.32 MB |
|        CesilAuto | 23.864 ms | 0.4166 ms | 0.3693 ms |  0.92 |   312.5000 |        - |        - |   1.28 MB |
| MgholamCsvManual |  4.474 ms | 0.0855 ms | 0.0915 ms |  0.17 |   414.0625 | 218.7500 |  23.4375 |   2.35 MB |
|    TinyCsvManual | 18.350 ms | 0.3153 ms | 0.2949 ms |  0.70 | 13093.7500 | 562.5000 | 281.2500 |   54.9 MB |
|       SylvanData |  3.392 ms | 0.0278 ms | 0.0232 ms |  0.13 |   308.5938 |  50.7813 |        - |   1.24 MB |
|     SylvanManual |  3.354 ms | 0.0226 ms | 0.0200 ms |  0.13 |   285.1563 |  46.8750 |        - |   1.14 MB |
|     SylvanDapper |  4.016 ms | 0.0780 ms | 0.0899 ms |  0.15 |   375.0000 |  62.5000 |        - |   1.52 MB |

### Auto

Measures libraries that can automatically bind CSV data to an object. 

### Manual

Tests the performance of hand-written code that reads data fields and assigns them to object properties.

TinyCsv and MgholamCsv only supports manual binding as far as I know. As they don't implement IDataReader a general purpose binder cannot be used..

### Dapper

This tests the performance of using Dapper's `GetRowParser<T>()` as a general purpose data binder. It requires that the data
source has a strongly-typed schema, so not all CSV libraries can be used with it.