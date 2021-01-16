# CSV Data Binder Benchmarks

These benchmarks test binding CSV data to a strongly-typed .NET object.

|           Method |      Mean | Ratio | Allocated |
|----------------- |----------:|------:|----------:|
|     SylvanManual |  3.585 ms |  0.13 |   1.24 MB |
|     SylvanDapper |  4.063 ms |  0.15 |   1.61 MB |
|       SylvanData |  4.096 ms |  0.15 |   1.37 MB |
| MgholamCsvManual |  4.765 ms |  0.18 |   2.36 MB |
|    TinyCsvManual | 18.780 ms |  0.70 |   54.9 MB |
|        CesilAuto | 24.704 ms |  0.93 |   1.29 MB |
|  CsvHelperManual | 25.977 ms |  0.97 |  27.32 MB |
|    CsvHelperAuto | 26.641 ms |  1.00 |  27.67 MB |

### Auto

Measures libraries that can automatically bind CSV data to an object. 

### Manual

Tests the performance of hand-written code that reads data fields and assigns them to object properties.

TinyCsv and MgholamCsv only supports manual binding as far as I know. As they don't implement IDataReader a general purpose binder cannot be used..

### Dapper

This tests the performance of using Dapper's `GetRowParser<T>()` as a general purpose data binder. It requires that the data
source has a strongly-typed schema, so not all CSV libraries can be used with it.

### Sylvan.Data

Pairs the Sylvan.Data.Csv parser with a general purpose data binder provided by Sylvan.Data (prerelease).