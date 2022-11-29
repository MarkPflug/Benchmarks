# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k row (max .xls row count) Excel file, and accessing each value as "strongly-typed" as the library allows.
Data files contain sample sales records from [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).

## Excel .xlsx Benchmarks

|              Method |       Mean |     Error |   StdDev |        Gen0 |        Gen1 |      Gen2 |     Allocated |
|-------------------- |-----------:|----------:|---------:|------------:|------------:|----------:|--------------:|
|          SylvanXlsx |   375.7 ms |   5.87 ms |  2.09 ms |           - |           - |         - |     624.95 KB |
|          AsposeXlsx |   626.5 ms |  32.67 ms | 11.65 ms |  39000.0000 |  10000.0000 | 2000.0000 |  181630.57 KB |
|       MiniExcelXlsx |   994.7 ms |  20.20 ms |  5.25 ms | 178000.0000 |           - |         - |   729386.7 KB |
|          EPPlusXlsx | 1,093.3 ms |  25.14 ms |  8.96 ms |  62000.0000 |  16000.0000 | 3000.0000 |   417174.3 KB |
| ExcelDataReaderXlsx | 1,193.8 ms |  18.11 ms |  4.70 ms | 108000.0000 |           - |         - |  441878.27 KB |
|         OpenXmlXlsx | 2,961.3 ms |  99.36 ms | 35.43 ms |  87000.0000 |  46000.0000 | 5000.0000 |   502461.4 KB |
|       FastExcelXlsx | 2,966.2 ms |  51.06 ms | 13.26 ms | 295000.0000 |  28000.0000 | 4000.0000 | 1290387.92 KB |
|            NpoiXlsx | 3,651.3 ms |  37.75 ms |  5.84 ms | 279000.0000 | 104000.0000 | 5000.0000 | 1564250.45 KB |
|       ClosedXmlXlsx | 5,069.9 ms | 104.64 ms | 37.32 ms | 188000.0000 |  53000.0000 | 5000.0000 | 1128646.93 KB |

## Excel .xlsb Benchmarks

|              Method |         Mean |        Error |     StdDev |       Gen0 |       Gen1 |      Gen2 |    Allocated |
|-------------------- |-------------:|-------------:|-----------:|-----------:|-----------:|----------:|-------------:|
|          SylvanXlsb |     54.45 ms |     6.317 ms |   1.641 ms |          - |          - |         - |    318.21 KB |
|          AsposeXlsb |    466.24 ms |    46.242 ms |  16.490 ms | 51000.0000 | 13000.0000 | 3000.0000 | 247252.36 KB |
| ExcelDataReaderXlsb | 15,720.91 ms | 1,321.460 ms | 471.245 ms | 47000.0000 |  1000.0000 |         - | 193105.23 KB |
## Excel .xls Benchmarks

|             Method |       Mean |     Error |   StdDev |        Gen0 |        Gen1 |      Gen2 |    Allocated |
|------------------- |-----------:|----------:|---------:|------------:|------------:|----------:|-------------:|
|    SylvanSchemaXls |   106.0 ms |   1.89 ms |  0.49 ms |           - |           - |         - |    592.89 KB |
|    SylvanStringXls |   184.4 ms |   6.53 ms |  2.33 ms |   5666.6667 |           - |         - |  23985.25 KB |
| ExcelDataReaderXls |   281.6 ms |   9.39 ms |  3.35 ms |  72000.0000 |   5000.0000 |  500.0000 | 323457.86 KB |
|          AsposeXls |   404.8 ms |  23.71 ms |  8.46 ms |  52000.0000 |  12000.0000 | 2000.0000 | 257592.43 KB |
|            NpoiXls | 2,817.9 ms | 159.54 ms | 56.89 ms | 542000.0000 | 146000.0000 | 4000.0000 | 2385391.5 KB |