# Excel Data Writer Benchmarks

These benchmarks measure writing a 65k row Excel file, with data read from in-memory objects.

## Excel .xlsx Writer Benchmarks

|               Method |       Mean |     Error |    StdDev |        Gen0 |        Gen1 |      Gen2 |    Allocated |
|--------------------- |-----------:|----------:|----------:|------------:|------------:|----------:|-------------:|
|           SylvanXlsx |   389.7 ms |  18.81 ms |   6.71 ms |           - |           - |         - |     179368 B |
|            LargeXlsx |   429.7 ms |   4.67 ms |   1.21 ms |   6000.0000 |   1000.0000 |         - |   31266528 B |
|           AsposeXlsb | 1,235.5 ms | 360.66 ms | 128.62 ms |  88000.0000 |  10000.0000 | 1000.0000 |  469791648 B |
|           AsposeXlsx | 1,238.4 ms |  13.27 ms |   2.05 ms |  77000.0000 |  10000.0000 | 1000.0000 |  390682376 B |
|       SwiftExcelXlsx | 1,358.0 ms |  96.74 ms |  34.50 ms |  90000.0000 |           - |         - |  378303592 B |
|  EPPlusViaDataReader | 1,662.6 ms |  29.88 ms |   4.62 ms |  64000.0000 |   6000.0000 | 1000.0000 |  409981992 B |
|           EPPlusXlsx | 1,857.4 ms |  20.57 ms |   1.13 ms | 103000.0000 |  15000.0000 | 1000.0000 |  650835592 B |
|        NanoXlsxWrite | 2,138.3 ms |  47.38 ms |  12.30 ms | 187000.0000 |  40000.0000 | 3000.0000 | 1461703144 B |
|             NpoiXlsx | 3,970.2 ms |  77.96 ms |  12.06 ms | 193000.0000 |  42000.0000 | 2000.0000 | 1264863016 B |
|          OpenXmlXlsx | 4,376.8 ms | 182.45 ms |  65.06 ms |  99000.0000 |  46000.0000 | 3000.0000 |  706155272 B |
| SpreadsheetLightXlsx | 8,163.8 ms | 134.03 ms |  47.80 ms | 498000.0000 | 106000.0000 | 3000.0000 | 2862069976 B |
