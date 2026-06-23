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
is a wrapper around OpenXml, one would assume that they'd have similar performance.

## Excel .xlsx Benchmarks

The `Baseline` in this group, measure the time taken to unzip the Sheet1.xml and using `XmlReader.Read` to process every node.
It represents the minimal work that must be done assuming those classes are used for processing.
AceOleDbXlsx is approximate, as I don't run it every time, for "reasons".


| Method                | Mean       | Error    | Ratio | Allocated     | Alloc Ratio |
|---------------------- |-----------:|---------:|------:|--------------:|------------:|
| Baseline              |   112.4 ms |  0.54 ms |  1.00 |     246.69 KB |        1.00 |
| SylvanXlsx            |   162.1 ms |  0.51 ms |  1.44 |     665.76 KB |        2.70 |
| SylvanXlsx_BindT      |   163.8 ms |  0.52 ms |  1.46 |   10906.11 KB |       44.21 |
| SylvanXlsxDynamic     |   170.9 ms |  0.61 ms |  1.52 |   14474.27 KB |       58.67 |
| XlsxHelperXlsx        |   201.2 ms |  0.48 ms |  1.79 |   93306.11 KB |      378.24 |
| PrimeXlsx             |   232.9 ms |  0.59 ms |  2.07 |    2755.86 KB |       11.17 |
| HypeLabXlsx_SheetData |   245.3 ms |  2.25 ms |  2.18 |    85187.3 KB |      345.32 |
| AsposeXlsx            |   309.5 ms |  6.15 ms |  2.75 |  209868.81 KB |      850.75 |
| ExcelDataReaderXlsx   |   383.1 ms |  1.59 ms |  3.41 |  191129.39 KB |      774.78 |
| MiniExcelXlsx         |   414.0 ms |  2.13 ms |  3.68 |  648458.83 KB |    2,628.67 |
| Lightweight           |   419.9 ms |  2.13 ms |  3.73 |  167950.66 KB |      680.82 |
| EPPlusXlsx            |   699.0 ms |  7.13 ms |  6.22 |   570353.9 KB |    2,312.05 |
| AceOleDbXlsx          |   707.6 ms |  6.08 ms | ~6.42 |   27035.35 KB |      ~109.56 |
| ClosedXmlXlsx         | 1,071.6 ms |  4.85 ms |  9.53 |  727578.09 KB |    2,949.39 |
| FastExcelXlsx         | 1,200.6 ms | 23.97 ms | 10.68 | 1189985.53 KB |    4,823.86 |
| NpoiXlsx              | 1,571.3 ms | 22.07 ms | 13.97 | 1003667.34 KB |    4,068.58 |


### Sum

This benchmark measures summing the values of a single column (`Total Profit`) from the Excel file.
It is meant to highlight an optimization employed by some libraries where you only "pay" for the
columns that you access.

| Method             | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0       | Gen1      | Gen2      | Allocated    | Alloc Ratio |
|------------------- |---------:|--------:|--------:|------:|--------:|-----------:|----------:|----------:|-------------:|------------:|
| Baseline           | 111.0 ms | 0.40 ms | 0.38 ms |  1.00 |    0.00 |          - |         - |         - |    246.69 KB |        1.00 |
| SylvanData         | 137.3 ms | 0.36 ms | 0.34 ms |  1.24 |    0.00 |          - |         - |         - |    648.98 KB |        2.63 |
| XlsxHelperXlsx     | 189.9 ms | 0.69 ms | 0.58 ms |  1.71 |    0.01 |  1666.6667 |  333.3333 |         - |  93305.67 KB |      378.23 |
| Prime              | 233.1 ms | 1.03 ms | 0.96 ms |  2.10 |    0.01 |          - |         - |         - |   2755.61 KB |       11.17 |
| ExcelDataReaderXls | 389.4 ms | 4.49 ms | 4.20 ms |  3.51 |    0.04 |  3000.0000 |         - |         - | 191128.08 KB |      774.78 |
| EPPlusXlsx         | 617.1 ms | 9.57 ms | 8.96 ms |  5.56 |    0.08 | 11000.0000 | 8000.0000 | 6000.0000 | 491605.15 KB |    1,992.83 |

## Excel .xlsb Benchmarks

| Method              | Mean        | Error    | Ratio  | Allocated    | Alloc Ratio |
|-------------------- |------------:|---------:|-------:|-------------:|------------:|
| SylvanXlsb          |    24.91 ms | 0.079 ms |   1.00 |    359.02 KB |        1.00 |
| PrimeXlsb           |    62.83 ms | 0.280 ms |   2.52 |  78220.42 KB |      217.87 |
| ExcelDataReaderXlsb |   139.55 ms | 0.435 ms |   5.60 |  73852.03 KB |      205.70 |
| AsposeXlsb          |   227.01 ms | 1.703 ms |   9.12 | 247964.87 KB |      690.66 |
| AceOleDbXlsb        | 3,356.99 ms | 9.507 ms | 134.79 |  13212.17 KB |       36.80 |

## Excel .xls Benchmarks

| Method             | Mean      | Error    | Ratio | Allocated    | Alloc Ratio |
|------------------- |----------:|---------:|------:|-------------:|------------:|
| SylvanXls          |  16.26 ms | 0.035 ms |  1.00 |    190.42 KB |        1.00 |
| ExcelDataReaderXls |  86.11 ms | 0.493 ms |  5.29 |  99922.71 KB |      524.75 |
| AceOleDbXls        | 205.20 ms | 0.923 ms | 12.62 |  13209.38 KB |       69.37 |
| AsposeXls          | 213.81 ms | 4.213 ms | 13.15 | 252011.55 KB |    1,323.45 |
| NpoiXls            | 571.19 ms | 8.177 ms | 35.12 | 657430.86 KB |    3,452.53 |
