
These benchmarks measure reading a 65k (max .xls row count) file, and accessing each value as "strongly-typed" as the library allows.
Data files contain sample sales records from [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).


Excel .xlsx benchmarks:

|              Method |       Mean |       Error |    StdDev |       Gen 0 |      Gen 1 |     Gen 2 |    Allocated |
|-------------------- |-----------:|------------:|----------:|------------:|-----------:|----------:|-------------:|
|          SylvanXlsx |   796.4 ms |    44.13 ms |  11.46 ms |           - |          - |         - |       599 KB |
| ExcelDataReaderXlsx | 2,071.4 ms |   121.15 ms |  43.20 ms | 109000.0000 |          - |         - |   449,047 KB |
|       FastExcelXlsx | 4,082.2 ms |    68.86 ms |  17.88 ms | 294000.0000 | 19000.0000 | 3000.0000 | 1,290,381 KB |
|            NpoiXlsx | 5,532.3 ms | 1,581.15 ms | 563.85 ms | 283000.0000 | 84000.0000 | 5000.0000 | 1,580,295 KB |
|          EPPlusXlsx | 1,679.1 ms |    82.19 ms |  29.31 ms |  76000.0000 | 15000.0000 | 4000.0000 |   445,734 KB |
|      ClosedXmlsXlsx | 5,542.3 ms |   349.04 ms | 124.47 ms | 162000.0000 | 44000.0000 | 5000.0000 |   971,373 KB |
|          AsposeXlsx |   831.9 ms |    38.44 ms |   9.98 ms |  54000.0000 | 11000.0000 | 3000.0000 |   270,986 KB |


Excel .xls benchmarks:

|             Method |       Mean |     Error |   StdDev |       Gen 0 |       Gen 1 |     Gen 2 | Allocated |
|------------------- |-----------:|----------:|---------:|------------:|------------:|----------:|----------:|
|          SylvanXls |   193.7 ms |   2.31 ms |  0.60 ms |   5666.6667 |           - |         - |     23 MB |
| ExcelDataReaderXls |   323.1 ms |   5.83 ms |  0.90 ms |  74000.0000 |   6000.0000 | 1000.0000 |    323 MB |
|            NpoiXls | 5,404.7 ms | 148.14 ms | 52.83 ms | 548000.0000 | 146000.0000 | 6000.0000 |  2,343 MB |
|          AsposeXls |   382.6 ms |   8.00 ms |  2.08 ms |  53000.0000 |  10000.0000 | 3000.0000 |    252 MB |