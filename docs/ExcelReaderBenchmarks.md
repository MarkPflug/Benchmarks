# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k row (max .xls row count) Excel file, and accessing each value as "strongly-typed" as the library allows.

## Excel .xlsx Benchmarks

The `Baseline` in this group, measure the time taken to unzip the Sheet1.xml and using `XmlReader.Read` to process every node.
It represents the minimal work that must be done assuming those classes are used for processing.

| Method              | Mean       | Error     | Ratio | Allocated     | Alloc Ratio |
|-------------------- |-----------:|----------:|------:|--------------:|------------:|
| Baseline            |   252.8 ms |  32.44 ms |  1.00 |     243.42 KB |        1.00 |
| SylvanXlsx          |   334.7 ms |   7.80 ms |  1.33 |     660.85 KB |        2.71 |
| XlsxHelperXlsx      |   442.2 ms |   3.08 ms |  1.75 |   93301.87 KB |      383.29 |
| AsposeXlsx          |   541.6 ms |  29.09 ms |  2.15 |  209750.95 KB |      861.68 |
| MiniExcelXlsx       | 1,016.8 ms |  50.98 ms |  4.01 |  729374.68 KB |    2,996.34 |
| ExcelDataReaderXlsx | 1,063.1 ms |  19.09 ms |  4.21 |  441878.48 KB |    1,815.28 |
| EPPlusXlsx          | 1,074.5 ms |  24.68 ms |  4.25 |   564392.8 KB |    2,318.58 |
| AceOleDbXls         | 1,227.5 ms | 105.88 ms |  4.89 |    27033.4 KB |      111.06 |
| FastExcelXlsx       | 2,588.3 ms |  66.90 ms | 10.25 | 1190024.77 KB |    4,888.73 |
| OpenXmlXlsx         | 2,793.1 ms |  92.67 ms | 11.05 |  502456.73 KB |    2,064.14 |
| ClosedXmlXlsx       | 2,917.8 ms | 226.07 ms | 11.62 | 1190520.72 KB |    4,890.77 |
| NpoiXlsx            | 3,557.8 ms |  51.56 ms | 14.09 | 1564691.02 KB |    6,427.90 |

## Excel .xlsb Benchmarks

| Method              | Mean         | Error      | Allocated    |
|-------------------- |-------------:|-----------:|-------------:|
| SylvanXlsb          |     49.98 ms |   0.885 ms |    355.38 KB |
| AsposeXlsb          |    413.68 ms |  28.096 ms | 247781.34 KB |
| AceOleDbXlsb        |  5,706.38 ms | 181.312 ms |   27033.4 KB |
| ExcelDataReaderXlsb | 15,912.64 ms | 681.043 ms | 193105.45 KB |## Excel .xls Benchmarks

## Excel .xls Benchmarks


| Method             | Mean        | Error      | Allocated     |
|------------------- |------------:|-----------:|--------------:|
| SylvanSchemaXls    |    40.32 ms |   0.368 ms |     523.72 KB |
| SylvanStringXls    |   110.03 ms |   0.761 ms |    23900.5 KB |
| ExcelDataReaderXls |   251.91 ms |   8.890 ms |  323458.07 KB |
| AsposeXls          |   368.56 ms |   3.223 ms |  227914.99 KB |
| AceOleDbXls        |   407.76 ms |   6.031 ms |    27033.4 KB |
| NpoiXls            | 2,575.05 ms | 113.102 ms | 2476504.72 KB |