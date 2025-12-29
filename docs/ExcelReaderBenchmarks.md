# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k row (max .xls row count) Excel file, and accessing each value as "strongly-typed" as the library allows.

## Excel .xlsx Benchmarks

The `Baseline` in this group, measure the time taken to unzip the Sheet1.xml and using `XmlReader.Read` to process every node.
It represents the minimal work that must be done assuming those classes are used for processing.

| Method                | Mean       | Error    | Ratio | Allocated     | Alloc Ratio |
|---------------------- |-----------:|---------:|------:|--------------:|------------:|
| Baseline              |   137.7 ms | 54.20 ms |  1.02 |     246.69 KB |        1.00 |
| SylvanXlsx            |   166.4 ms |  1.92 ms |  1.23 |     666.32 KB |        2.70 |
| SylvanXlsx_BindT      |   168.8 ms |  1.75 ms |  1.25 |   11931.32 KB |       48.37 |
| SylvanXlsxObj         |   172.0 ms |  1.83 ms |  1.27 |   14474.54 KB |       58.68 |
| PrimeXlsx             |   186.9 ms |  2.69 ms |  1.38 |   98711.03 KB |      400.15 |
| XlsxHelperXlsx        |   204.1 ms |  1.72 ms |  1.51 |   93306.52 KB |      378.24 |
| HypeLabXlsx_SheetData |   223.4 ms |  8.05 ms |  1.65 |   85184.81 KB |      345.31 |
| HypeLabXlsx_BindT     |   227.5 ms |  7.48 ms |  1.68 |   95985.48 KB |      389.10 |
| AsposeXlsx            |   312.2 ms |  3.70 ms |  2.31 |  209869.09 KB |      850.75 |
| MiniExcelXlsx         |   413.6 ms |  6.71 ms |  3.05 |  648459.77 KB |    2,628.67 |
| ExcelDataReaderXlsx   |   451.8 ms |  4.87 ms |  3.34 |  267931.52 KB |    1,086.12 |
| Lightweight           |   453.9 ms | 25.61 ms |  3.35 |  178188.62 KB |      722.33 |
| EPPlusXlsx            |   650.0 ms |  8.14 ms |  4.80 |  417822.37 KB |    1,693.73 |
| ClosedXmlXlsx         | 1,052.3 ms | 15.42 ms |  7.77 |  671743.29 KB |    2,723.05 |
| FastExcelXlsx         | 1,139.8 ms | 45.66 ms |  8.42 | 1189998.17 KB |    4,823.91 |
| OpenXmlXlsx           | 1,376.8 ms | 75.89 ms | 10.16 |  502385.12 KB |    2,036.52 |
| NpoiXlsx              | 1,756.7 ms | 55.75 ms | 12.97 | 1050100.87 KB |    4,256.81 |

## Excel .xlsb Benchmarks

| Method              | Mean        | Error     | Ratio  | Allocated    | Alloc Ratio |
|-------------------- |------------:|----------:|-------:|-------------:|------------:|
| SylvanXlsb          |    24.89 ms |  0.470 ms |   1.00 |    359.12 KB |        1.00 |
| ExcelDataReaderXlsb |   136.58 ms |  1.263 ms |   5.49 | 150657.41 KB |      419.52 |
| AsposeXlsb          |   221.08 ms | 17.821 ms |   8.88 | 247967.76 KB |      690.49 |
| AceOleDbXlsb        | 3,520.58 ms | 61.037 ms | 141.43 |  27035.63 KB |       75.28 |

## Excel .xls Benchmarks

| Method             | Mean        | Error     | Ratio | Allocated     | Alloc Ratio |
|------------------- |------------:|----------:|------:|--------------:|------------:|
| SylvanSchemaXls    |    18.32 ms |  0.267 ms |  0.42 |     189.07 KB |       0.008 |
| SylvanStringXls    |    43.77 ms |  1.020 ms |  1.00 |   23565.42 KB |       1.000 |
| ExcelDataReaderXls |   108.39 ms |  1.654 ms |  2.48 |  237566.18 KB |      10.081 |
| AsposeXls          |   203.44 ms | 14.196 ms |  4.65 |  227989.91 KB |       9.675 |
| AceOleDbXls        |   205.41 ms |  2.665 ms |  4.69 |    27033.8 KB |       1.147 |
| NpoiXls            | 1,272.08 ms | 36.454 ms | 29.06 | 2679897.56 KB |     113.722 |
