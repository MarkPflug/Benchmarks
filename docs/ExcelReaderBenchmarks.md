# Excel Data Reader Benchmarks

These benchmarks measure reading a 65k row (max .xls row count) Excel file, and accessing each value as "strongly-typed" as the library allows.

## Excel .xlsx Benchmarks

The `Baseline` in this group, measure the time taken to unzip the Sheet1.xml and using `XmlReader.Read` to process every node.
It represents the minimal work that must be done assuming those classes are used for processing.

|              Method |       Mean |    Error |   StdDev | Ratio | RatioSD |        Gen0 |        Gen1 |      Gen2 |     Allocated | Alloc Ratio |
|-------------------- |-----------:|---------:|---------:|------:|--------:|------------:|------------:|----------:|--------------:|------------:|
|            Baseline |   221.2 ms |  3.45 ms |  1.23 ms |  1.00 |    0.00 |           - |           - |         - |     242.84 KB |        1.00 |
|          SylvanXlsx |   386.1 ms |  7.29 ms |  1.89 ms |  1.74 |    0.02 |           - |           - |         - |     647.92 KB |        2.67 |
|          AsposeXlsx |   647.9 ms | 12.88 ms |  3.35 ms |  2.93 |    0.01 |  39000.0000 |  10000.0000 | 2000.0000 |  181630.64 KB |      747.94 |
|       MiniExcelXlsx | 1,021.6 ms | 21.04 ms |  5.46 ms |  4.61 |    0.05 | 178000.0000 |           - |         - |  729449.88 KB |    3,003.82 |
|          EPPlusXlsx | 1,104.8 ms | 27.27 ms |  7.08 ms |  4.99 |    0.03 |  63000.0000 |  16000.0000 | 3000.0000 |  417544.98 KB |    1,719.42 |
|         AceOleDbXls | 1,153.8 ms | 26.41 ms |  9.42 ms |  5.22 |    0.06 |   6000.0000 |           - |         - |    27033.4 KB |      111.32 |
| ExcelDataReaderXlsx | 1,176.6 ms |  9.37 ms |  0.51 ms |  5.30 |    0.02 | 108000.0000 |           - |         - |  441878.33 KB |    1,819.62 |
|         OpenXmlXlsx | 2,960.2 ms | 51.12 ms |  7.91 ms | 13.34 |    0.06 |  87000.0000 |  46000.0000 | 5000.0000 |  502459.98 KB |    2,069.09 |
|       FastExcelXlsx | 2,993.8 ms | 32.38 ms |  1.77 ms | 13.48 |    0.05 | 293000.0000 |  26000.0000 | 2000.0000 | 1290381.01 KB |    5,313.69 |
|       ClosedXmlXlsx | 3,401.0 ms | 37.72 ms |  5.84 ms | 15.33 |    0.06 | 159000.0000 |  46000.0000 | 5000.0000 |  953141.31 KB |    3,924.96 |
|            NpoiXlsx | 3,670.8 ms | 56.97 ms | 20.31 ms | 16.60 |    0.17 | 279000.0000 | 104000.0000 | 5000.0000 | 1564250.52 KB |    6,441.46 |

## Excel .xlsb Benchmarks

|              Method |         Mean |     Error |    StdDev |       Gen0 |       Gen1 |      Gen2 |    Allocated |
|-------------------- |-------------:|----------:|----------:|-----------:|-----------:|----------:|-------------:|
|          SylvanXlsb |     59.37 ms |  1.164 ms |  0.415 ms |          - |          - |         - |    341.46 KB |
|          AsposeXlsb |    463.43 ms | 29.621 ms | 10.563 ms | 51000.0000 | 13000.0000 | 3000.0000 | 247251.47 KB |
|        AceOleDbXlsb |  5,699.32 ms | 67.872 ms | 10.503 ms |  6000.0000 |          - |         - |   27033.4 KB |
| ExcelDataReaderXlsb | 15,124.43 ms | 34.577 ms |  1.895 ms | 47000.0000 |  1000.0000 |         - |  193105.3 KB |

## Excel .xls Benchmarks

|             Method |       Mean |    Error |  StdDev |        Gen0 |        Gen1 |      Gen2 |     Allocated |
|------------------- |-----------:|---------:|--------:|------------:|------------:|----------:|--------------:|
|    SylvanSchemaXls |   113.8 ms |  1.31 ms | 0.20 ms |           - |           - |         - |     585.11 KB |
|    SylvanStringXls |   191.9 ms |  2.91 ms | 0.45 ms |   5666.6667 |           - |         - |   23961.39 KB |
| ExcelDataReaderXls |   289.0 ms |  7.08 ms | 2.52 ms |  72000.0000 |   5000.0000 |  500.0000 |  323457.86 KB |
|          AsposeXls |   378.7 ms |  9.20 ms | 3.28 ms |  51000.0000 |  12000.0000 | 2000.0000 |  257592.49 KB |
|        AceOleDbXls |   402.4 ms | 15.68 ms | 5.59 ms |   6000.0000 |           - |         - |    27033.4 KB |
|            NpoiXls | 2,910.2 ms | 33.11 ms | 5.12 ms | 542000.0000 | 146000.0000 | 4000.0000 | 2385395.24 KB |