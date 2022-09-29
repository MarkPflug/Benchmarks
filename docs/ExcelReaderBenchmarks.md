# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k row (max .xls row count) Excel file, and accessing each value as "strongly-typed" as the library allows.
Data files contain sample sales records from [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).

## Excel .xlsx Benchmarks

|              Method |       Mean |     Error |    StdDev |       Gen 0 |      Gen 1 |     Gen 2 |     Allocated |
|-------------------- |-----------:|----------:|----------:|------------:|-----------:|----------:|--------------:|
|          SylvanXlsx |   461.4 ms |  72.43 ms |  25.83 ms |           - |          - |         - |     631.71 KB |
|          AsposeXlsx |   710.5 ms | 182.63 ms |  47.43 ms |  39000.0000 |  8000.0000 | 2000.0000 |  181634.96 KB |
|       MiniExcelXlsx | 1,083.7 ms |  30.85 ms |  11.00 ms | 178000.0000 |          - |         - |   729307.2 KB |
|          EPPlusXlsx | 1,209.3 ms |  90.63 ms |  32.32 ms |  79000.0000 | 18000.0000 | 6000.0000 |   600551.9 KB |
| ExcelDataReaderXlsx | 1,212.3 ms |  56.60 ms |  20.18 ms | 109000.0000 |          - |         - |  449046.79 KB |
|       FastExcelXlsx | 2,847.8 ms | 147.25 ms |  52.51 ms | 294000.0000 | 19000.0000 | 3000.0000 | 1290380.53 KB |
|         OpenXmlXlsx | 2,978.6 ms | 174.07 ms |  45.21 ms |  86000.0000 | 31000.0000 | 4000.0000 |  502458.62 KB |
|            NpoiXlsx | 3,944.5 ms | 248.43 ms |  88.59 ms | 284000.0000 | 84000.0000 | 5000.0000 | 1583895.02 KB |
|       ClosedXmlXlsx | 5,634.4 ms | 303.86 ms | 108.36 ms | 188000.0000 | 51000.0000 | 5000.0000 | 1128636.24 KB |

## Excel .xlsb Benchmarks

|              Method |         Mean |      Error |    StdDev |      Gen 0 |      Gen 1 |     Gen 2 |    Allocated |
|-------------------- |-------------:|-----------:|----------:|-----------:|-----------:|----------:|-------------:|
|          SylvanXlsb |     55.70 ms |   1.109 ms |  0.172 ms |          - |          - |         - |    320.23 KB |
|          AsposeXlsb |    475.36 ms |  24.115 ms |  8.600 ms | 52000.0000 | 11000.0000 | 3000.0000 |  262760.8 KB |
| ExcelDataReaderXlsb | 15,645.85 ms | 160.593 ms | 24.852 ms | 49000.0000 |  1000.0000 |         - | 200274.27 KB |

## Excel .xls Benchmarks

|             Method |       Mean |     Error |   StdDev |       Gen 0 |       Gen 1 |     Gen 2 |     Allocated |
|------------------- |-----------:|----------:|---------:|------------:|------------:|----------:|--------------:|
|    SylvanSchemaXls |   107.7 ms |   0.90 ms |  0.14 ms |           - |           - |         - |      608.7 KB |
|    SylvanStringXls |   190.7 ms |   3.45 ms |  1.23 ms |   5666.6667 |           - |         - |   23991.33 KB |
| ExcelDataReaderXls |   328.4 ms |  33.80 ms | 12.05 ms |  73000.0000 |   5000.0000 |         - |  330625.76 KB |
|          AsposeXls |   392.8 ms |   7.13 ms |  1.10 ms |  52000.0000 |   9000.0000 | 2000.0000 |  257594.48 KB |
|            NpoiXls | 3,243.7 ms | 106.94 ms | 38.14 ms | 545000.0000 | 143000.0000 | 3000.0000 | 2399567.03 KB |