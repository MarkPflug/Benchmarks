# CSV Data Binder Benchmarks

These benchmarks test binding CSV data to a strongly-typed .NET object. 
This benchmark set includes libraries that provide some form of data binding capabilites.
Libraries that only provide raw access are covered in the CsvReaderBenchmarks.
These measurements combine both CSV processing with data binding time.

|              Method |      Mean |     Error |   StdDev |       Gen0 |      Gen1 |     Gen2 | Allocated |
|-------------------- |----------:|----------:|---------:|-----------:|----------:|---------:|----------:|
|        SylvanManual |  63.23 ms |  0.121 ms | 0.019 ms |  5625.0000 |         - |        - |  22.93 MB |
|          SylvanData |  71.20 ms |  2.155 ms | 0.560 ms | 11142.8571 |  142.8571 |        - |  44.92 MB |
|    SylvanDataPooled |  71.71 ms |  2.824 ms | 1.007 ms | 11142.8571 |  142.8571 |        - |  44.92 MB |
|     SylvanDataAsync |  75.76 ms |  0.882 ms | 0.229 ms | 11142.8571 |  142.8571 |        - |  44.95 MB |
|   RecordParserAsync |  80.18 ms |  3.196 ms | 1.140 ms |  5571.4286 |         - |        - |  22.24 MB |
|        SylvanDapper |  91.42 ms | 10.711 ms | 2.782 ms | 15166.6667 |  166.6667 |        - |  60.98 MB |
|       TinyCsvManual | 137.34 ms | 12.497 ms | 4.456 ms | 51250.0000 | 4750.0000 | 750.0000 | 228.67 MB |
|       CsvHelperAuto | 141.59 ms |  3.082 ms | 1.099 ms | 15500.0000 |         - |        - |  62.49 MB |
|           CesilAuto | 171.12 ms | 21.123 ms | 7.533 ms |  5666.6667 |         - |        - |  23.14 MB |
| CsvHelperAutoPooled | 252.14 ms | 12.347 ms | 3.206 ms |  6500.0000 | 2000.0000 | 500.0000 |  46.55 MB |