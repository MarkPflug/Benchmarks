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
| Baseline              |   111.6 ms |  0.39 ms |  1.00 |     246.69 KB |        1.00 |
| SylvanXlsx            |   162.5 ms |  0.71 ms |  1.46 |     665.76 KB |        2.70 |
| SylvanXlsx_BindT      |   164.2 ms |  0.71 ms |  1.47 |   10906.27 KB |       44.21 |
| SylvanXlsxDynamic     |   169.8 ms |  0.77 ms |  1.52 |   14474.27 KB |       58.67 |
| XlsxHelperXlsx        |   201.6 ms |  0.77 ms |  1.81 |   93306.11 KB |      378.24 |
| PrimeXlsx             |   207.4 ms |  0.87 ms |  1.86 |   82980.75 KB |      336.38 |
| HypeLabXlsx_SheetData |   243.8 ms |  3.26 ms |  2.18 |   85186.37 KB |      345.32 |
| AsposeXlsx            |   306.7 ms |  5.82 ms |  2.75 |  209868.38 KB |      850.75 |
| Lightweight           |   407.5 ms |  5.31 ms |  3.65 |  167951.37 KB |      680.83 |
| MiniExcelXlsx         |   410.8 ms |  3.37 ms |  3.68 |  648458.83 KB |    2,628.67 |
| ExcelDataReaderXlsx   |   438.6 ms |  1.34 ms |  3.93 |  267930.68 KB |    1,086.11 |
| EPPlusXlsx            |   698.4 ms |  5.88 ms |  6.26 |  570350.74 KB |    2,312.04 |
| AceOleDbXlsx          |   707.6 ms |  6.08 ms | ~6.42 |   27035.35 KB |      ~109.56 |
| ClosedXmlXlsx         | 1,063.9 ms |  5.69 ms |  9.53 |  727577.53 KB |    2,949.39 |
| FastExcelXlsx         | 1,217.4 ms | 20.99 ms | 10.90 | 1189983.26 KB |    4,823.85 |
| NpoiXlsx              | 1,564.0 ms | 17.51 ms | 14.01 |  1003667.2 KB |    4,068.58 |


### Sum

This benchmark measures summing the values of a single column (`Total Profit`) from the Excel file.
It is meant to highlight an optimization employed by some libraries where you only "pay" for the
columns that you access.

| Method             | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0      | Gen1     | Allocated    | Alloc Ratio |
|------------------- |---------:|--------:|--------:|------:|--------:|----------:|---------:|-------------:|------------:|
| Baseline           | 110.9 ms | 0.51 ms | 0.43 ms |  1.00 |    0.01 |         - |        - |    246.69 KB |        1.00 |
| SylvanData         | 135.8 ms | 0.51 ms | 0.48 ms |  1.22 |    0.01 |         - |        - |    649.47 KB |        2.63 |
| XlsxHelperXlsx     | 186.7 ms | 0.99 ms | 0.87 ms |  1.68 |    0.01 | 1666.6667 | 333.3333 |  93305.33 KB |      378.23 |
| Prime              | 199.8 ms | 0.42 ms | 0.37 ms |  1.80 |    0.01 | 1666.6667 | 333.3333 |  82981.29 KB |      336.38 |
| ExcelDataReaderXls | 436.0 ms | 2.20 ms | 2.05 ms |  3.93 |    0.02 | 5000.0000 |        - | 267929.37 KB |    1,086.11 |

## Excel .xlsb Benchmarks

| Method              | Mean        | Error    | Ratio  | Allocated    | Alloc Ratio |
|-------------------- |------------:|---------:|-------:|-------------:|------------:|
| SylvanXlsb          |    24.85 ms | 0.129 ms |   1.00 |    359.02 KB |        1.00 |
| PrimeXlsb           |    62.40 ms | 0.228 ms |   2.51 | 142740.12 KB |      397.58 |
| ExcelDataReaderXlsb |   135.19 ms | 0.623 ms |   5.44 | 150655.52 KB |      419.63 |
| AsposeXlsb          |   225.41 ms | 3.271 ms |   9.07 | 247963.99 KB |      690.66 |
| AceOleDbXlsb        | 3,430.11 ms | 9.214 ms | 138.02 |  13212.17 KB |       36.80 |

## Excel .xls Benchmarks

| Method             | Mean      | Error     | Ratio | Allocated    | Alloc Ratio |
|------------------- |----------:|----------:|------:|-------------:|------------:|
| SylvanXls          |  16.31 ms |  0.058 ms |  1.00 |    190.41 KB |        1.00 |
| ExcelDataReaderXls | 110.48 ms |  1.679 ms |  6.78 | 237569.16 KB |    1,247.66 |
| AceOleDbXls        | 205.85 ms |  0.968 ms | 12.62 |  27033.59 KB |      141.97 |
| AsposeXls          | 212.78 ms |  4.149 ms | 13.05 | 252010.76 KB |    1,323.50 |
| NpoiXls            | 557.45 ms | 11.082 ms | 34.19 | 657427.16 KB |    3,452.65 |