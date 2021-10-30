# CSV Data Binder Benchmarks

These benchmarks test binding CSV data to a strongly-typed .NET object. 
This benchmark set includes libraries that provide some form of data binding capabilites.
Libraries that only provide raw access are covered in the CsvReaderBenchmarks.
These measurements combine both CSV processing with data binding time.

|              Method |      Mean |     Error |    StdDev |      Gen 0 |     Gen 1 |     Gen 2 | Allocated |
|-------------------- |----------:|----------:|----------:|-----------:|----------:|----------:|----------:|
|        SylvanManual |  69.35 ms |  0.681 ms |  0.105 ms |  5625.0000 |         - |         - |     23 MB |
|          SylvanData |  71.81 ms |  0.480 ms |  0.074 ms |  5714.2857 |  142.8571 |         - |     23 MB |
|     SylvanDataAsync |  72.18 ms |  0.709 ms |  0.110 ms |  5714.2857 |         - |         - |     23 MB |
|    SylvanDataPooled |  73.94 ms |  1.481 ms |  0.528 ms |  2428.5714 |  142.8571 |         - |     10 MB |
|        SylvanDapper |  82.30 ms |  2.258 ms |  0.805 ms |  9714.2857 |         - |         - |     39 MB |
|   RecordParserAsync |  95.41 ms |  2.017 ms |  0.719 ms |  5500.0000 |         - |         - |     22 MB |
|       TinyCsvManual | 140.70 ms |  9.423 ms |  3.360 ms | 51500.0000 | 4000.0000 |  750.0000 |    229 MB |
|       CsvHelperAuto | 154.99 ms |  2.652 ms |  0.689 ms | 26000.0000 |         - |         - |    106 MB |
|           CesilAuto | 172.28 ms |  2.718 ms |  0.706 ms |  5000.0000 |         - |         - |     23 MB |
| CsvHelperAutoPooled | 283.15 ms | 32.053 ms | 11.430 ms |  9000.0000 | 3500.0000 | 1500.0000 |     55 MB |
