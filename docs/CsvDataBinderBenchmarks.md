# CSV Data Binder Benchmarks

These benchmarks test binding CSV data to a strongly-typed .NET object. 
This benchmark set includes libraries that provide some form of data binding capabilites.
Libraries that only provide raw access are covered in the CsvReaderBenchmarks.
These measurements combine both CSV processing with data binding time.

|              Method |      Mean |     Error |    StdDev |      Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|-------------------- |----------:|----------:|----------:|-----------:|----------:|----------:|----------:|
|        SylvanManual |  64.36 ms |  0.735 ms |  0.114 ms |  5625.0000 |         - |         - |     23 MB |
|          SylvanData |  67.67 ms |  5.911 ms |  2.108 ms |  5625.0000 |  125.0000 |         - |     23 MB |
|     SylvanDataAsync |  68.26 ms |  6.316 ms |  1.640 ms |  5714.2857 |         - |         - |     23 MB |
|    SylvanDataPooled |  68.60 ms |  0.262 ms |  0.014 ms |  2500.0000 |  125.0000 |         - |     10 MB |
|        SylvanDapper |  75.45 ms |  1.713 ms |  0.611 ms |  9714.2857 |         - |         - |     39 MB |
|   RecordParserAsync |  85.49 ms |  2.899 ms |  1.034 ms |  5500.0000 |         - |         - |     22 MB |
|       TinyCsvManual | 131.60 ms | 13.079 ms |  4.664 ms | 53000.0000 | 3500.0000 | 1000.0000 |    229 MB |
|       CsvHelperAuto | 140.79 ms |  2.443 ms |  0.378 ms | 26500.0000 |         - |         - |    106 MB |
|           CesilAuto | 166.96 ms |  5.215 ms |  1.354 ms |  5000.0000 |         - |         - |     23 MB |
| CsvHelperAutoPooled | 261.43 ms | 38.524 ms | 13.738 ms |  8666.6667 | 3000.0000 | 1333.3333 |     55 MB |