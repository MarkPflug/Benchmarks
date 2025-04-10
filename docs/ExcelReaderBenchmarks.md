# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k row (max .xls row count) Excel file, and accessing each value as "strongly-typed" as the library allows.

## Excel .xlsx Benchmarks

The `Baseline` in this group, measure the time taken to unzip the Sheet1.xml and using `XmlReader.Read` to process every node.
It represents the minimal work that must be done assuming those classes are used for processing.

| Method              | Mean       | Error    | Ratio | Allocated     | Alloc Ratio |
|-------------------- |-----------:|---------:|------:|--------------:|------------:|
| Baseline            |   188.6 ms |  2.72 ms |  1.00 |     245.16 KB |        1.00 |
| SylvanXlsx          |   286.1 ms |  2.09 ms |  1.52 |     660.07 KB |        2.69 |
| SylvanXlsxObj       |   314.8 ms |  1.84 ms |  1.67 |   14468.77 KB |       59.02 |
| XlsxHelperXlsx      |   403.2 ms |  8.22 ms |  2.14 |   93303.91 KB |      380.58 |
| AsposeXlsx          |   518.5 ms | 23.27 ms |  2.75 |  209787.98 KB |      855.70 |
| MiniExcelXlsx       |   799.0 ms | 15.58 ms |  4.24 |   610402.3 KB |    2,489.77 |
| Lightweight         |   833.7 ms | 20.96 ms |  4.42 |  178184.38 KB |      726.80 |
| ExcelDataReaderXlsx |   929.2 ms | 15.05 ms |  4.93 |  353883.76 KB |    1,443.46 |
| EPPlusXlsx          | 1,113.5 ms | 37.35 ms |  5.91 |  417873.66 KB |    1,704.47 |
| ClosedXmlXlsx       | 1,809.7 ms | 86.84 ms |  9.60 |  671818.41 KB |    2,740.28 |
| FastExcelXlsx       | 2,142.7 ms | 53.01 ms | 11.36 | 1190031.69 KB |    4,854.02 |
| OpenXmlXlsx         | 2,637.0 ms | 42.25 ms | 13.99 |   502496.9 KB |    2,049.64 |
| NpoiXlsx            | 3,247.1 ms | 36.77 ms | 17.22 | 1089795.73 KB |    4,445.17 |

## Excel .xlsb Benchmarks

| Method              | Mean        | Error     | Allocated    |
|-------------------- |------------:|----------:|-------------:|
| SylvanXlsb          |    39.92 ms |  4.119 ms |    354.36 KB |
| ExcelDataReaderXlsb |   238.53 ms |  2.868 ms | 193145.92 KB |
| AsposeXlsb          |   405.49 ms | 22.895 ms | 247931.05 KB |
| AceOleDbXlsb        | 5,992.73 ms | 58.035 ms |   27035.7 KB |
## Excel .xls Benchmarks


| Method             | Mean        | Error      | Ratio | Allocated     | Alloc Ratio |
|------------------- |------------:|-----------:|------:|--------------:|------------:|
| SylvanSchemaXls    |    28.95 ms |   0.484 ms |  0.31 |     181.04 KB |       0.008 |
| SylvanStringXls    |    93.18 ms |  15.484 ms |  1.00 |   23558.69 KB |       1.000 |
| ExcelDataReaderXls |   248.17 ms |  16.226 ms |  2.67 |  323461.18 KB |      13.730 |
| AsposeXls          |   360.39 ms |  26.880 ms |  3.88 |  227939.27 KB |       9.675 |
| AceOleDbXls        |   369.55 ms |   7.789 ms |  3.98 |   27036.68 KB |       1.148 |
| NpoiXls            | 2,342.26 ms | 164.443 ms | 25.21 | 2499409.84 KB |     106.093 |