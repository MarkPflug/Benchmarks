# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k row (max .xls row count) Excel file, and accessing each value as "strongly-typed" as the library allows.

## Excel .xlsx Benchmarks

The `Baseline` in this group, measure the time taken to unzip the Sheet1.xml and using `XmlReader.Read` to process every node.
It represents the minimal work that must be done assuming those classes are used for processing.

| Method                | Mean       | Error    | Ratio | Allocated     | Alloc Ratio |
|---------------------- |-----------:|---------:|------:|--------------:|------------:|
| Baseline              |   119.1 ms |  1.46 ms |  1.00 |     246.69 KB |        1.00 |
| SylvanXlsx            |   167.9 ms |  3.48 ms |  1.41 |     666.42 KB |        2.70 |
| SylvanXlsx_BindT      |   170.3 ms |  2.94 ms |  1.43 |   11931.32 KB |       48.37 |
| SylvanXlsxObj         |   174.9 ms |  2.53 ms |  1.47 |   14474.54 KB |       58.68 |
| XlsxHelperXlsx        |   208.6 ms |  3.69 ms |  1.75 |   93306.52 KB |      378.24 |
| PrimeXlsxObj          |   222.7 ms |  3.65 ms |  1.87 |   83860.64 KB |      339.95 |
| HypeLabXlsx_SheetData |   231.1 ms |  6.04 ms |  1.94 |   85184.77 KB |      345.31 |
| HypeLabXlsx_BindT     |   236.4 ms |  7.22 ms |  1.98 |    95985.4 KB |      389.10 |
| PrimeXlsx             |   245.1 ms |  3.37 ms |  2.06 |   85396.79 KB |      346.17 |
| AsposeXlsx            |   318.8 ms | 23.87 ms |  2.68 |  209869.11 KB |      850.75 |
| MiniExcelXlsx         |   425.4 ms |  6.70 ms |  3.57 |  648459.31 KB |    2,628.67 |
| Lightweight           |   441.9 ms | 24.47 ms |  3.71 |  167948.33 KB |      680.81 |
| ExcelDataReaderXlsx   |   453.1 ms |  8.84 ms |  3.80 |  267931.52 KB |    1,086.12 |
| EPPlusXlsx            |   670.9 ms | 11.61 ms |  5.63 |  417822.21 KB |    1,693.73 |
| ClosedXmlXlsx         | 1,076.3 ms | 21.00 ms |  9.03 |  671740.68 KB |    2,723.04 |
| FastExcelXlsx         | 1,160.5 ms | 15.23 ms |  9.74 | 1189997.51 KB |    4,823.91 |
| OpenXmlXlsx           | 1,427.0 ms | 85.97 ms | 11.98 |  502384.91 KB |    2,036.52 |
| NpoiXlsx              | 1,791.9 ms | 32.25 ms | 15.04 | 1050100.93 KB |    4,256.81 |
## Excel .xlsb Benchmarks

| Method              | Mean        | Error     | Ratio  | Allocated    | Alloc Ratio |
|-------------------- |------------:|----------:|-------:|-------------:|------------:|
| SylvanXlsb          |    25.15 ms |  0.451 ms |   1.00 |    359.12 KB |        1.00 |
| PrimeXlsbObj        |    59.06 ms |  0.908 ms |   2.35 | 143663.32 KB |      400.05 |
| PrimeXlsb           |    61.42 ms |  1.108 ms |   2.44 | 131375.32 KB |      365.83 |
| ExcelDataReaderXlsb |   140.21 ms |  4.392 ms |   5.58 | 150655.71 KB |      419.52 |
| AsposeXlsb          |   226.85 ms | 19.297 ms |   9.02 | 247967.74 KB |      690.49 |
| AceOleDbXlsb        | 3,651.75 ms | 42.592 ms | 145.21 |   27032.4 KB |       75.27 |

## Excel .xls Benchmarks

| Method             | Mean        | Error     | Ratio | Allocated     | Alloc Ratio |
|------------------- |------------:|----------:|------:|--------------:|------------:|
| SylvanSchemaXls    |    18.32 ms |  0.267 ms |  0.42 |     189.07 KB |       0.008 |
| SylvanStringXls    |    43.77 ms |  1.020 ms |  1.00 |   23565.42 KB |       1.000 |
| ExcelDataReaderXls |   108.39 ms |  1.654 ms |  2.48 |  237566.18 KB |      10.081 |
| AsposeXls          |   203.44 ms | 14.196 ms |  4.65 |  227989.91 KB |       9.675 |
| AceOleDbXls        |   205.41 ms |  2.665 ms |  4.69 |    27033.8 KB |       1.147 |
| NpoiXls            | 1,272.08 ms | 36.454 ms | 29.06 | 2679897.56 KB |     113.722 |
