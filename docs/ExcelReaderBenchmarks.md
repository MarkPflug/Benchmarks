# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k row (max .xls row count) Excel file, 
and accessing each cell value as the type expected by the SalesRecord class.

SylvanXlsx_BindT is the only benchmark in the group that actually creates and binds
to SalesRecord instances, the other benchmarks only produce the required values.

These benchmarks are written to be as fair as possible. One requirement is that the
monetary values must be a decimal with two digit precision. Many librarys will
produce decimal values with incorrect precision without custom handling. Specifically,
the values must be accessed as a double, then cast to a decimal, which will produce the
expected precision. Many libraries provide decimal accessors that are a "pit of failure"
in this regard. See benchmark implementations for full details.

OpenXml is no longer included in these results. It is so difficult to use that I
cannot figure out how to write a correct and fair benchmark for it. Since ClosedXml
is a wrapper around OpenXml, one could assume that they'd have similar performance.

## Excel .xlsx Benchmarks

The `Baseline` in this group, measure the time taken to unzip the Sheet1.xml and using `XmlReader.Read` to process every node.
It represents the minimal work that must be done assuming those classes are used for processing.


| Method                | Mean       | Error    | Ratio | Allocated     | Alloc Ratio |
|---------------------- |-----------:|---------:|------:|--------------:|------------:|
| Baseline              |   112.1 ms | 11.55 ms |  1.00 |      246.7 KB |        1.00 |
| SylvanXlsx            |   166.5 ms |  3.02 ms |  1.49 |     665.93 KB |        2.70 |
| SylvanXlsx_BindT      |   167.8 ms |  2.14 ms |  1.50 |   10906.74 KB |       44.21 |
| SylvanXlsxDynamic     |   172.7 ms |  3.01 ms |  1.54 |   14474.39 KB |       58.67 |
| PrimeXlsx             |   182.9 ms |  3.07 ms |  1.63 |   83860.22 KB |      339.93 |
| XlsxHelperXlsx        |   202.4 ms |  4.00 ms |  1.81 |   93306.34 KB |      378.23 |
| HypeLabXlsx_SheetData |   241.4 ms |  8.91 ms |  2.15 |   85184.43 KB |      345.30 |
| AsposeXlsx            |   305.7 ms | 19.85 ms |  2.73 |  209866.15 KB |      850.71 |
| MiniExcelXlsx         |   407.4 ms |  7.91 ms |  3.64 |  648458.91 KB |    2,628.58 |
| Lightweight           |   410.3 ms | 29.93 ms |  3.66 |  167950.77 KB |      680.80 |
| ExcelDataReaderXlsx   |   430.3 ms |  8.01 ms |  3.84 |  267931.42 KB |    1,086.08 |
| EPPlusXlsx            |   652.2 ms | 11.09 ms |  5.82 |  423878.02 KB |    1,718.22 |
| AceOleDbXls           |   693.0 ms | 11.21 ms |  6.19 |    27037.6 KB |      109.60 |
| ClosedXmlXlsx         | 1,059.4 ms | 16.31 ms |  9.46 |  727575.88 KB |    2,949.29 |
| FastExcelXlsx         | 1,170.3 ms | 45.03 ms | 10.45 | 1189985.66 KB |    4,823.71 |
| NpoiXlsx              | 1,688.9 ms | 29.66 ms | 15.08 | 1288175.14 KB |    5,221.73 |

## Excel .xlsb Benchmarks

| Method              | Mean        | Error     | Ratio  | Allocated    | Alloc Ratio |
|-------------------- |------------:|----------:|-------:|-------------:|------------:|
| SylvanXlsb          |    24.82 ms |  0.327 ms |   1.00 |    358.93 KB |        1.00 |
| PrimeXlsb           |    60.73 ms |  0.961 ms |   2.45 | 129839.36 KB |      361.74 |
| ExcelDataReaderXlsb |   138.58 ms |  3.805 ms |   5.58 | 150655.65 KB |      419.74 |
| AsposeXlsb          |   226.25 ms |  3.101 ms |   9.12 | 247968.29 KB |      690.86 |
| AceOleDbXlsb        | 3,359.50 ms | 30.683 ms | 135.36 |  27035.96 KB |       75.32 |

## Excel .xls Benchmarks

| Method             | Mean      | Error     | Ratio | Allocated    | Alloc Ratio |
|------------------- |----------:|----------:|------:|-------------:|------------:|
| SylvanXls          |  16.35 ms |  0.273 ms |  1.00 |    190.35 KB |        1.00 |
| ExcelDataReaderXls | 110.82 ms |  5.002 ms |  6.78 | 237569.13 KB |    1,248.07 |
| AceOleDbXls        | 204.81 ms |  3.460 ms | 12.53 |  27033.59 KB |      142.02 |
| AsposeXls          | 213.70 ms | 15.953 ms | 13.07 | 252014.51 KB |    1,323.96 |
| NpoiXls            | 667.76 ms | 28.481 ms | 40.85 |  720789.6 KB |    3,786.66 |