# Excel Data Writer Benchmarks

These benchmarks measure writing a 65k row Excel file, with data read from in-memory objects.
The data set is sales records data from: [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).

## Excel .xlsx Writer Benchmarks

|        Method |       Mean |     Error |    StdDev |       Gen 0 |      Gen 1 |     Gen 2 |     Allocated |
|-------------- |-----------:|----------:|----------:|------------:|-----------:|----------:|--------------:|
|    SylvanXlsx |   349.4 ms |   4.24 ms |   0.23 ms |           - |          - |         - |     174.17 KB |
|    AsposeXlsx | 1,195.7 ms |  23.56 ms |   6.12 ms |  77000.0000 | 10000.0000 | 1000.0000 |  381557.63 KB |
|    EPPlusXlsx | 1,943.8 ms |  96.25 ms |  34.32 ms | 137000.0000 | 25000.0000 | 2000.0000 |  750259.42 KB |
| NanoXlsxWrite | 2,126.4 ms | 392.28 ms | 139.89 ms | 199000.0000 | 51000.0000 | 4000.0000 | 1425397.34 KB |
|   OpenXmlXlsx | 3,356.2 ms |  52.04 ms |  13.52 ms |  99000.0000 | 33000.0000 | 3000.0000 |  689600.41 KB |
|      NpoiXlsx | 4,311.1 ms | 195.78 ms |  69.82 ms | 199000.0000 | 44000.0000 | 5000.0000 | 1249585.01 KB |
