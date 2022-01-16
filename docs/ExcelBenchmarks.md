# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k row (max .xls row count) Excel file, and accessing each value as "strongly-typed" as the library allows.
Data files contain sample sales records from [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).

Updated with latest packages as of 2022-01-16.

## Excel .xlsx Benchmarks

|              Method |       Mean |     Error |   StdDev |       Gen 0 |      Gen 1 |     Gen 2 |    Allocated |
|-------------------- |-----------:|----------:|---------:|------------:|-----------:|----------:|-------------:|
|          SylvanXlsx |   409.2 ms |   6.92 ms |  0.38 ms |           - |          - |         - |       604 KB |
|          AsposeXlsx |   753.8 ms |  40.24 ms | 10.45 ms |  53000.0000 | 10000.0000 | 2000.0000 |   270,990 KB |
|       MiniExcelXlsx |   923.8 ms | 111.03 ms | 28.84 ms |  45000.0000 |          - |         - |   186,200 KB |
|          EPPlusXlsx | 1,094.5 ms |  20.22 ms |  5.25 ms |  75000.0000 | 14000.0000 | 3000.0000 |   445,720 KB |
| ExcelDataReaderXlsx | 1,151.0 ms |  12.92 ms |  2.00 ms | 109000.0000 |          - |         - |   449,049 KB |
|         OpenXmlXlsx | 2,689.9 ms |  92.94 ms | 33.15 ms |  81000.0000 | 29000.0000 | 3000.0000 |   478,897 KB |
|       FastExcelXlsx | 2,815.0 ms |  51.62 ms | 18.41 ms | 294000.0000 | 19000.0000 | 3000.0000 | 1,290,381 KB |
|            NpoiXlsx | 3,644.0 ms |  65.64 ms |  3.60 ms | 284000.0000 | 84000.0000 | 5000.0000 | 1,583,880 KB |
|       ClosedXmlXlsx | 5,572.8 ms | 187.68 ms | 66.93 ms | 184000.0000 | 49000.0000 | 5000.0000 | 1,105,062 KB |

## Excel .xlsb Benchmarks

|              Method |         Mean |        Error |       StdDev |      Gen 0 |      Gen 1 |     Gen 2 |     Allocated |
|-------------------- |-------------:|-------------:|-------------:|-----------:|-----------:|----------:|--------------:|
|          SylvanXlsb |     49.43 ms |     0.755 ms |     0.041 ms |          - |          - |         - |     302,612 B |
|          AsposeXlsb |    433.75 ms |    27.122 ms |     9.672 ms | 51000.0000 | 10000.0000 | 2000.0000 | 269,403,456 B |
| ExcelDataReaderXlsb | 14,511.80 ms | 5,426.711 ms | 1,409.300 ms | 49000.0000 |  1000.0000 |         - | 205,082,600 B |

## Excel .xls Benchmarks

|             Method |       Mean |     Error |   StdDev |       Gen 0 |       Gen 1 |     Gen 2 | Allocated |
|------------------- |-----------:|----------:|---------:|------------:|------------:|----------:|----------:|
|          SylvanXls |   176.3 ms |   5.24 ms |  1.87 ms |   5666.6667 |           - |         - |     23 MB |
| ExcelDataReaderXls |   313.0 ms |   3.48 ms |  0.19 ms |  74000.0000 |   6000.0000 | 1000.0000 |    323 MB |
|          AsposeXls |   384.2 ms |   8.14 ms |  2.11 ms |  53000.0000 |  10000.0000 | 3000.0000 |    252 MB |
|            NpoiXls | 3,304.2 ms | 251.13 ms | 89.56 ms | 549000.0000 | 147000.0000 | 7000.0000 |  2,343 MB |