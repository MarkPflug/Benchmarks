# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k row (max .xls row count) Excel file, and accessing each value as "strongly-typed" as the library allows.
Data files contain sample sales records from [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).

Updated with latest packages as of 2022-01-16.

## Excel .xlsx Benchmarks

|              Method |       Mean |     Error |    StdDev |       Gen 0 |      Gen 1 |     Gen 2 |     Allocated |
|-------------------- |-----------:|----------:|----------:|------------:|-----------:|----------:|--------------:|
|          SylvanXlsx |   423.7 ms |   7.35 ms |   1.14 ms |           - |          - |         - |     626.85 KB |
|          AsposeXlsx |   651.9 ms |  25.31 ms |   9.03 ms |  38000.0000 |  7000.0000 | 1000.0000 |  181633.59 KB |
|       MiniExcelXlsx | 1,090.9 ms |  53.69 ms |  19.15 ms | 178000.0000 |          - |         - |  729259.45 KB |
| ExcelDataReaderXlsx | 1,194.7 ms |  24.28 ms |   8.66 ms | 109000.0000 |          - |         - |  449046.71 KB |
|          EPPlusXlsx | 1,209.9 ms |  23.67 ms |   3.66 ms |  75000.0000 | 16000.0000 | 3000.0000 |  600542.01 KB |
|       FastExcelXlsx | 3,001.4 ms | 549.00 ms | 142.57 ms | 294000.0000 | 19000.0000 | 3000.0000 | 1290379.72 KB |
|         OpenXmlXlsx | 3,019.4 ms | 139.46 ms |  36.22 ms |  86000.0000 | 31000.0000 | 4000.0000 |  502458.75 KB |
|            NpoiXlsx | 4,204.2 ms | 566.23 ms | 201.92 ms | 284000.0000 | 84000.0000 | 5000.0000 | 1583881.16 KB |
|       ClosedXmlXlsx | 5,411.7 ms |  98.55 ms |  25.59 ms | 187000.0000 | 50000.0000 | 4000.0000 | 1128634.11 KB |

## Excel .xlsb Benchmarks

|              Method |         Mean |      Error |     StdDev |      Gen 0 |      Gen 1 |     Gen 2 |    Allocated |
|-------------------- |-------------:|-----------:|-----------:|-----------:|-----------:|----------:|-------------:|
|          SylvanXlsb |     54.59 ms |   1.193 ms |   0.425 ms |          - |          - |         - |    320.19 KB |
|          AsposeXlsb |    483.04 ms |   9.470 ms |   0.519 ms | 51000.0000 | 10000.0000 | 2000.0000 | 262766.35 KB |
| ExcelDataReaderXlsb | 15,446.45 ms | 329.745 ms | 117.590 ms | 49000.0000 |  1000.0000 |         - |  200273.7 KB |

## Excel .xls Benchmarks

|             Method |       Mean |    Error |   StdDev |       Gen 0 |       Gen 1 |     Gen 2 |  Allocated |
|------------------- |-----------:|---------:|---------:|------------:|------------:|----------:|-----------:|
|          SylvanXls |   170.0 ms |  4.02 ms |  1.44 ms |   5666.6667 |           - |         - |   23.43 MB |
| ExcelDataReaderXls |   313.8 ms |  5.21 ms |  0.29 ms |  73000.0000 |   5000.0000 |         - |  322.88 MB |
|          AsposeXls |   409.6 ms | 31.61 ms | 11.27 ms |  51000.0000 |   9000.0000 | 2000.0000 |  251.56 MB |
|            NpoiXls | 3,312.7 ms | 80.66 ms | 28.76 ms | 545000.0000 | 143000.0000 | 3000.0000 | 2343.32 MB |