# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k row (max .xls row count) Excel file, and accessing each value as "strongly-typed" as the library allows.

## Excel .xlsx Benchmarks

The `Baseline` in this group, measure the time taken to unzip the Sheet1.xml and using `XmlReader.Read` to process every node.
It represents the minimal work that must be done assuming those classes are used for processing.

| Method                | Mean       | Error    | Ratio | Allocated     | Alloc Ratio |
|---------------------- |-----------:|---------:|------:|--------------:|------------:|
| Baseline              |   118.2 ms |  1.43 ms |  1.00 |     246.69 KB |        1.00 |
| SylvanXlsx            |   165.0 ms |  1.09 ms |  1.40 |     665.85 KB |        2.70 |
| SylvanXlsx_BindT      |   168.7 ms |  3.35 ms |  1.43 |    11931.1 KB |       48.37 |
| SylvanXlsxObj         |   173.2 ms |  3.36 ms |  1.47 |   14474.45 KB |       58.68 |
| PrimeXlsx             |   184.8 ms |  2.26 ms |  1.56 |   98710.93 KB |      400.15 |
| XlsxHelperXlsx        |   205.6 ms |  3.94 ms |  1.74 |   93306.37 KB |      378.24 |
| HypeLabXlsx_SheetData |   223.6 ms |  8.34 ms |  1.89 |   85184.28 KB |      345.31 |
| HypeLabXlsx_BindT     |   228.3 ms |  5.74 ms |  1.93 |   95985.43 KB |      389.10 |
| AsposeXlsx            |   308.7 ms | 13.54 ms |  2.61 |  209869.41 KB |      850.75 |
| Lightweight           |   416.4 ms | 32.16 ms |  3.52 |  167947.38 KB |      680.81 |
| MiniExcelXlsx         |   416.8 ms |  7.56 ms |  3.53 |  648459.31 KB |    2,628.67 |
| ExcelDataReaderXlsx   |   450.6 ms |  3.89 ms |  3.81 |   267931.8 KB |    1,086.12 |
| EPPlusXlsx            |   668.7 ms |  7.15 ms |  5.66 |   417820.7 KB |    1,693.72 |
| ClosedXmlXlsx         | 1,067.8 ms | 40.01 ms |  9.04 |  671747.17 KB |    2,723.07 |
| FastExcelXlsx         | 1,140.7 ms | 27.91 ms |  9.65 | 1189998.52 KB |    4,823.91 |
| OpenXmlXlsx           | 1,375.4 ms | 98.78 ms | 11.64 |  502385.81 KB |    2,036.53 |
| NpoiXlsx              | 1,766.2 ms | 58.83 ms | 14.94 | 1050100.94 KB |    4,256.81 |
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
