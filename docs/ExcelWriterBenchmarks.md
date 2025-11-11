# Excel Data Writer Benchmarks

These benchmarks measure writing a 65k row Excel file, with data read from in-memory objects.

| Method              | Mean        | Error      | Allocated     |
|-------------------- |------------:|-----------:|--------------:|
| SylvanXlsb          |    84.49 ms |   1.009 ms |     111.58 KB |
| SylvanXlsx          |   125.13 ms |   2.141 ms |     165.98 KB |
| LargeXlsx           |   154.53 ms |   5.046 ms |     136.74 KB |
| MiniXl              |   332.62 ms |  15.411 ms |  333659.11 KB |
| SwiftExcelXlsx      |   433.81 ms |   7.731 ms |  101931.89 KB |
| AsposeXlsb          |   950.09 ms |  19.658 ms |     308166 KB |
| AsposeXlsx          |   986.65 ms |  20.256 ms |  248157.97 KB |
| NanoXlsxWrite       | 1,754.58 ms |  70.732 ms | 1416898.35 KB |
| OpenXmlXlsx         | 2,034.94 ms |  62.248 ms |  541582.13 KB |
| EPPlusViaDataReader | 2,140.62 ms |  40.638 ms |  400809.33 KB |
| EPPlusXlsx          | 2,344.79 ms |  35.420 ms |  643065.15 KB |
| NpoiXlsx            | 3,506.06 ms | 292.514 ms | 1135317.88 KB |