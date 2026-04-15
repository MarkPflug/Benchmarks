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


| Method                | Mean       | Error    | Ratio | Allocated     | Alloc Ratio |
|---------------------- |-----------:|---------:|------:|--------------:|------------:|
| Baseline              |   110.2 ms |  0.34 ms |  1.00 |     246.77 KB |        1.00 |
| SylvanXlsx            |   160.6 ms |  0.35 ms |  1.46 |     665.69 KB |        2.70 |
| SylvanXlsx_BindT      |   161.6 ms |  0.32 ms |  1.47 |   10906.27 KB |       44.20 |
| SylvanXlsxDynamic     |   167.4 ms |  0.40 ms |  1.52 |   14474.07 KB |       58.66 |
| PrimeXlsx             |   179.6 ms |  0.57 ms |  1.63 |   83860.72 KB |      339.84 |
| XlsxHelperXlsx        |   199.5 ms |  0.52 ms |  1.81 |   93305.77 KB |      378.11 |
| HypeLabXlsx_SheetData |   235.1 ms |  1.22 ms |  2.13 |   85186.46 KB |      345.21 |
| AsposeXlsx            |   307.0 ms |  6.03 ms |  2.79 |  209868.67 KB |      850.48 |
| MiniExcelXlsx         |   404.7 ms |  1.22 ms |  3.67 |  648452.45 KB |    2,627.81 |
| Lightweight           |   407.5 ms |  4.75 ms |  3.70 |  167948.73 KB |      680.60 |
| ExcelDataReaderXlsx   |   449.1 ms |  1.06 ms |  4.08 |  267930.68 KB |    1,085.77 |
| EPPlusXlsx            |   688.6 ms | 10.64 ms |  6.25 |  570354.45 KB |    2,311.32 |
| AceOleDbXls           |   707.6 ms |  6.08 ms |  6.42 |   27035.35 KB |      109.56 |
| ClosedXmlXlsx         | 1,060.0 ms |  4.94 ms |  9.62 |   727577.3 KB |    2,948.45 |
| FastExcelXlsx         | 1,171.3 ms | 22.78 ms | 10.63 | 1189984.38 KB |    4,822.33 |
| NpoiXlsx              | 1,540.4 ms | 12.91 ms | 13.98 | 1003666.73 KB |    4,067.29 |

## Excel .xlsb Benchmarks

| Method              | Mean        | Error    | Ratio  | Allocated    | Alloc Ratio |
|-------------------- |------------:|---------:|-------:|-------------:|------------:|
| SylvanXlsb          |    25.39 ms | 0.115 ms |   1.00 |    359.02 KB |        1.00 |
| PrimeXlsb           |    60.30 ms | 0.178 ms |   2.38 | 129839.35 KB |      361.65 |
| ExcelDataReaderXlsb |   134.52 ms | 0.406 ms |   5.30 | 150655.29 KB |      419.63 |
| AsposeXlsb          |   226.10 ms | 2.605 ms |   8.91 | 247964.96 KB |      690.67 |
| AceOleDbXlsb        | 3,337.40 ms | 8.328 ms | 131.45 |  27035.63 KB |       75.30 |

## Excel .xls Benchmarks

| Method             | Mean      | Error     | Ratio | Allocated    | Alloc Ratio |
|------------------- |----------:|----------:|------:|-------------:|------------:|
| SylvanXls          |  16.31 ms |  0.058 ms |  1.00 |    190.41 KB |        1.00 |
| ExcelDataReaderXls | 110.48 ms |  1.679 ms |  6.78 | 237569.16 KB |    1,247.66 |
| AceOleDbXls        | 205.85 ms |  0.968 ms | 12.62 |  27033.59 KB |      141.97 |
| AsposeXls          | 212.78 ms |  4.149 ms | 13.05 | 252010.76 KB |    1,323.50 |
| NpoiXls            | 557.45 ms | 11.082 ms | 34.19 | 657427.16 KB |    3,452.65 |