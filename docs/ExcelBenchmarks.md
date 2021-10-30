
# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k (max .xls row count) file, and accessing each value as "strongly-typed" as the library allows.
Data files contain sample sales records from [eforexcel.com](https://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).

## Excel .xlsx Benchmarks

Note that these benchmarks were run on .NET 5 instead of 6, as .NET 6 has a significant performance regression in System.Xml,
which I assume will be fixed at some point.

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1319 (21H2)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.100-rc.2.21505.57
  [Host] : .NET 5.0.10 (5.0.1021.41214), X64 RyuJIT
```


|              Method |       Mean |     Error |    StdDev |       Gen 0 |      Gen 1 |     Gen 2 |    Allocated |
|-------------------- |-----------:|----------:|----------:|------------:|-----------:|----------:|-------------:|
|          SylvanXlsx |   549.6 ms |  92.36 ms |  32.93 ms |           - |          - |         - |       599 KB |
|          AsposeXlsx |   840.5 ms |  43.77 ms |  15.61 ms |  53000.0000 | 10000.0000 | 2000.0000 |   270,982 KB |
|       MiniExcelXlsx | 1,207.3 ms |  19.64 ms |   7.00 ms |  45000.0000 |          - |         - |   186,180 KB |
|          EPPlusXlsx | 1,370.7 ms |  72.67 ms |  25.92 ms |  75000.0000 | 14000.0000 | 3000.0000 |   445,945 KB |
| ExcelDataReaderXlsx | 1,570.8 ms |  25.91 ms |   1.42 ms | 111000.0000 |          - |         - |   456,726 KB |
|         OpenXmlXlsx | 2,933.5 ms |  55.92 ms |  19.94 ms |  81000.0000 | 29000.0000 | 3000.0000 |   478,987 KB |
|       FastExcelXlsx | 4,224.4 ms | 368.52 ms |  95.70 ms | 294000.0000 | 19000.0000 | 3000.0000 | 1,290,382 KB |
|            NpoiXlsx | 4,706.8 ms | 499.53 ms | 178.14 ms | 284000.0000 | 84000.0000 | 5000.0000 | 1,583,881 KB |
|       ClosedXmlXlsx | 6,193.0 ms | 213.26 ms |  55.38 ms | 184000.0000 | 49000.0000 | 5000.0000 | 1,105,260 KB |

## Excel .xls Benchmarks

|             Method |       Mean |     Error |    StdDev |       Gen 0 |       Gen 1 |     Gen 2 | Allocated |
|------------------- |-----------:|----------:|----------:|------------:|------------:|----------:|----------:|
|          SylvanXls |   198.8 ms |  12.34 ms |   4.40 ms |   5666.6667 |           - |         - |     23 MB |
| ExcelDataReaderXls |   327.6 ms |   6.06 ms |   2.16 ms |  74000.0000 |   6000.0000 | 1000.0000 |    323 MB |
|          AsposeXls |   379.9 ms |  14.25 ms |   3.70 ms |  53000.0000 |  10000.0000 | 3000.0000 |    252 MB |
|            NpoiXls | 4,171.6 ms | 318.21 ms | 113.48 ms | 549000.0000 | 148000.0000 | 7000.0000 |  2,343 MB |
