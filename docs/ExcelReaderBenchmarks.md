# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k row (max .xls row count) Excel file, and accessing each value as "strongly-typed" as the library allows.

## Excel .xlsx Benchmarks

The `Baseline` in this group, measure the time taken to unzip the Sheet1.xml and using `XmlReader.Read` to process every node.
It represents the minimal work that must be done assuming those classes are used for processing.

| Method                | Mean       | Error     | Ratio | Allocated     | Alloc Ratio |
|---------------------- |-----------:|----------:|------:|--------------:|------------:|
| Baseline              |   178.0 ms |   3.55 ms |  1.00 |     246.68 KB |        1.00 |
| SylvanXlsx            |   271.4 ms |  17.05 ms |  1.52 |     666.95 KB |        2.70 |
| SylvanXlsx_BindT      |   296.6 ms |  13.52 ms |  1.67 |   11931.77 KB |       48.37 |
| SylvanXlsxObj         |   301.0 ms |   6.92 ms |  1.69 |   14475.52 KB |       58.68 |
| XlsxHelperXlsx        |   360.7 ms |   8.03 ms |  2.03 |   93309.16 KB |      378.26 |
| HypeLabXlsx_SheetData |   372.7 ms |   5.10 ms |  2.09 |   85193.23 KB |      345.36 |
| HypeLabXlsx_BindT     |   389.1 ms |  21.97 ms |  2.19 |   95989.57 KB |      389.13 |
| AsposeXlsx            |   514.8 ms |  25.62 ms |  2.89 |  209869.05 KB |      850.78 |
| MiniExcelXlsx         |   699.2 ms |  12.24 ms |  3.93 |  649338.94 KB |    2,632.32 |
| ExcelDataReaderXlsx   |   748.7 ms |  12.81 ms |  4.21 |   267930.8 KB |    1,086.15 |
| Lightweight           |   821.9 ms |  36.25 ms |  4.62 |  178191.19 KB |      722.36 |
| EPPlusXlsx            | 1,066.4 ms |  38.15 ms |  5.99 |  417840.98 KB |    1,693.86 |
| ClosedXmlXlsx         | 1,724.1 ms |  66.05 ms |  9.69 |  671841.37 KB |    2,723.54 |
| FastExcelXlsx         | 2,003.1 ms | 116.70 ms | 11.25 |  1190037.8 KB |    4,824.22 |
| OpenXmlXlsx           | 2,455.4 ms |  47.96 ms | 13.80 |  502510.18 KB |    2,037.10 |
| NpoiXlsx              | 2,977.1 ms |  43.25 ms | 16.73 | 1050151.75 KB |    4,257.15 |

## Excel .xlsb Benchmarks

| Method              | Mean        | Error     | Allocated    |
|-------------------- |------------:|----------:|-------------:|
| SylvanXlsb          |    37.00 ms |  0.482 ms |    358.95 KB |
| ExcelDataReaderXlsb |   203.76 ms |  8.584 ms | 150655.43 KB |
| AsposeXlsb          |   403.89 ms | 37.985 ms | 247971.47 KB |
| AceOleDbXlsb        | 5,995.29 ms | 94.781 ms |  27034.37 KB |

## Excel .xls Benchmarks

| Method             | Mean        | Error      | Ratio | Allocated     | Alloc Ratio |
|------------------- |------------:|-----------:|------:|--------------:|------------:|
| SylvanSchemaXls    |    30.40 ms |   1.144 ms |  0.40 |     189.01 KB |       0.008 |
| SylvanStringXls    |    75.85 ms |   1.204 ms |  1.00 |   23565.63 KB |       1.000 |
| ExcelDataReaderXls |   209.34 ms |   3.958 ms |  2.76 |   237565.8 KB |      10.081 |
| AsposeXls          |   357.09 ms |  25.121 ms |  4.71 |  227994.77 KB |       9.675 |
| AceOleDbXls        |   357.59 ms |  12.247 ms |  4.71 |   27036.29 KB |       1.147 |
| NpoiXls            | 2,182.48 ms | 127.395 ms | 28.77 | 2677245.21 KB |     113.608 |