
# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k (max .xls row count) file, and accessing each value as "strongly-typed" as the library allows.
Data files contain sample sales records from [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).

## Excel .xlsx Benchmarks


|              Method |       Mean |    Error |   StdDev |       Gen 0 |      Gen 1 |     Gen 2 |    Allocated |
|-------------------- |-----------:|---------:|---------:|------------:|-----------:|----------:|-------------:|
|          SylvanXlsx |   412.2 ms |  5.73 ms |  1.49 ms |           - |          - |         - |       613 KB |
|          AsposeXlsx |   758.3 ms | 10.27 ms |  2.67 ms |  53000.0000 | 10000.0000 | 2000.0000 |   270,986 KB |
|       MiniExcelXlsx |   868.3 ms | 70.87 ms | 18.40 ms |  45000.0000 |          - |         - |   186,200 KB |
|          EPPlusXlsx | 1,092.1 ms | 36.11 ms | 12.88 ms |  75000.0000 | 14000.0000 | 3000.0000 |   445,733 KB |
| ExcelDataReaderXlsx | 1,156.2 ms | 16.47 ms |  5.87 ms | 109000.0000 |          - |         - |   449,047 KB |
|         OpenXmlXlsx | 2,312.5 ms | 42.69 ms | 11.09 ms |  81000.0000 | 29000.0000 | 3000.0000 |   478,898 KB |
|       FastExcelXlsx | 2,775.4 ms | 28.51 ms |  4.41 ms | 294000.0000 | 19000.0000 | 3000.0000 | 1,290,385 KB |
|            NpoiXlsx | 3,627.4 ms | 57.43 ms |  3.15 ms | 284000.0000 | 84000.0000 | 5000.0000 | 1,583,889 KB |
|       ClosedXmlXlsx | 5,105.8 ms | 87.76 ms | 22.79 ms | 184000.0000 | 49000.0000 | 5000.0000 | 1,105,078 KB |

## Excel .xls Benchmarks

|             Method |       Mean |    Error |   StdDev |       Gen 0 |       Gen 1 |     Gen 2 | Allocated |
|------------------- |-----------:|---------:|---------:|------------:|------------:|----------:|----------:|
|          SylvanXls |   192.4 ms |  4.18 ms |  1.09 ms |   5666.6667 |           - |         - |     23 MB |
| ExcelDataReaderXls |   325.2 ms | 24.19 ms |  8.63 ms |  74000.0000 |   6000.0000 | 1000.0000 |    323 MB |
|          AsposeXls |   384.8 ms |  6.04 ms |  2.15 ms |  53000.0000 |  10000.0000 | 3000.0000 |    252 MB |
|            NpoiXls | 3,188.7 ms | 58.57 ms | 15.21 ms | 549000.0000 | 148000.0000 | 7000.0000 |  2,343 MB |
