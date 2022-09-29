# Excel Data Writer Benchmarks

These benchmarks measure writing a 65k row Excel file, with data read from in-memory objects.
The data set is sales records data from: [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).

## Excel .xlsx Writer Benchmarks

|              Method |       Mean |     Error |    StdDev |       Gen 0 |      Gen 1 |     Gen 2 |     Allocated |
|-------------------- |-----------:|----------:|----------:|------------:|-----------:|----------:|--------------:|
|          SylvanXlsx |   363.8 ms |   5.65 ms |   0.87 ms |           - |          - |         - |     178.52 KB |
|          AsposeXlsx | 1,274.6 ms |  37.87 ms |  13.51 ms |  78000.0000 | 11000.0000 | 2000.0000 |  381560.71 KB |
| EPPlusViaDataReader | 1,809.7 ms |  63.78 ms |  16.56 ms |  66000.0000 |  6000.0000 | 2000.0000 |  400370.68 KB |
|          EPPlusXlsx | 2,112.4 ms | 204.65 ms |  72.98 ms | 135000.0000 | 24000.0000 | 2000.0000 |  750266.39 KB |
|       NanoXlsxWrite | 2,286.9 ms | 293.63 ms | 104.71 ms | 197000.0000 | 50000.0000 | 5000.0000 | 1427445.79 KB |
|         OpenXmlXlsx | 3,565.6 ms | 209.92 ms |  74.86 ms | 100000.0000 | 34000.0000 | 4000.0000 |  689606.17 KB |
|            NpoiXlsx | 4,362.5 ms | 145.96 ms |  37.91 ms | 199000.0000 | 44000.0000 | 5000.0000 | 1249561.47 KB |
