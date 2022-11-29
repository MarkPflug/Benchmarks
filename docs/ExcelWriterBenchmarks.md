# Excel Data Writer Benchmarks

These benchmarks measure writing a 65k row Excel file, with data read from in-memory objects.
The data set is sales records data from: [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).

## Excel .xlsx Writer Benchmarks

|              Method |       Mean |     Error |   StdDev |        Gen0 |       Gen1 |      Gen2 |     Allocated |
|-------------------- |-----------:|----------:|---------:|------------:|-----------:|----------:|--------------:|
|          SylvanXlsx |   375.4 ms |   6.17 ms |  2.20 ms |           - |          - |         - |      175.4 KB |
|          AsposeXlsx | 1,264.5 ms |  19.42 ms |  6.93 ms |  78000.0000 | 11000.0000 | 2000.0000 |  381531.31 KB |
| EPPlusViaDataReader | 1,711.7 ms |  86.65 ms | 30.90 ms |  65000.0000 |  8000.0000 | 2000.0000 |   400369.8 KB |
|          EPPlusXlsx | 1,909.6 ms |  48.53 ms | 17.31 ms | 105000.0000 | 17000.0000 | 3000.0000 |  635584.55 KB |
|       NanoXlsxWrite | 2,206.8 ms |  91.86 ms | 23.86 ms | 188000.0000 | 39000.0000 | 4000.0000 | 1427444.33 KB |
|         OpenXmlXlsx | 3,421.5 ms |  64.44 ms | 22.98 ms | 100000.0000 | 47000.0000 | 4000.0000 |  689606.16 KB |
|            NpoiXlsx | 4,282.9 ms | 223.59 ms | 79.73 ms | 195000.0000 | 43000.0000 | 4000.0000 |  1235221.2 KB |
