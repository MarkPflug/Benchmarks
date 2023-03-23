# CSV Data Binder Benchmarks

These benchmarks test binding CSV data to a strongly-typed .NET object. 
This benchmark set includes libraries that provide some form of data binding capabilites.
Libraries that only provide raw access are covered in the CsvReaderBenchmarks.
These measurements combine both CSV processing with data binding time.

|              Method |      Mean |    Error |   StdDev |       Gen0 |      Gen1 |     Gen2 | Allocated |
|-------------------- |----------:|---------:|---------:|-----------:|----------:|---------:|----------:|
|        SylvanManual |  62.22 ms | 0.807 ms | 0.044 ms |  5666.6667 |         - |        - |  22.93 MB |
|          SylvanData |  63.83 ms | 0.900 ms | 0.321 ms |  5625.0000 |  125.0000 |        - |  22.93 MB |
|    SylvanDataPooled |  66.18 ms | 1.348 ms | 0.074 ms |  2500.0000 |  125.0000 |        - |  10.04 MB |
|     SylvanDataAsync |  68.36 ms | 1.118 ms | 0.173 ms |  5750.0000 |  125.0000 |        - |  22.96 MB |
|   RecordParserAsync |  78.20 ms | 1.316 ms | 0.342 ms |  5571.4286 |         - |        - |  22.24 MB |
|        SylvanDapper |  84.88 ms | 1.624 ms | 0.089 ms | 15166.6667 |  166.6667 |        - |  60.98 MB |
|       TinyCsvManual | 130.97 ms | 7.317 ms | 2.609 ms | 52500.0000 | 4250.0000 | 500.0000 | 228.56 MB |
|       CsvHelperAuto | 139.36 ms | 1.244 ms | 0.192 ms | 15500.0000 |         - |        - |  62.49 MB |
|           CesilAuto | 159.27 ms | 2.647 ms | 0.687 ms |  5750.0000 |         - |        - |  23.14 MB |
|        SoftCircuits | 167.49 ms | 2.621 ms | 0.406 ms | 26333.3333 |         - |        - |  105.4 MB |
| CsvHelperAutoPooled | 230.13 ms | 4.953 ms | 1.766 ms |  6333.3333 | 2000.0000 | 333.3333 |  46.56 MB |