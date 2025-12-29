# Excel Data Writer Benchmarks

These benchmarks measure writing a 65k row Excel file, with data read from in-memory objects.

| Method              | Mean        | Error     | Allocated     |
|-------------------- |------------:|----------:|--------------:|
| SylvanXlsb          |    46.03 ms |  0.773 ms |     111.19 KB |
| SylvanXlsx          |    69.46 ms |  1.211 ms |      165.8 KB |
| LargeXlsx           |    92.45 ms |  2.173 ms |     136.07 KB |
| MiniXl              |   187.48 ms |  5.299 ms |  333634.53 KB |
| SwiftExcelXlsx      |   299.97 ms |  2.572 ms |  101921.76 KB |
| AsposeXlsx          |   569.09 ms |  7.782 ms |  248143.48 KB |
| AsposeXlsb          |   593.43 ms |  8.379 ms |   308156.8 KB |
| OpenXmlXlsx         |   990.82 ms | 20.935 ms |  541553.01 KB |
| NanoXlsxWrite       | 1,080.34 ms | 20.613 ms | 1414844.21 KB |
| EPPlusViaDataReader | 1,412.21 ms | 14.683 ms |  400815.04 KB |
| EPPlusXlsx          | 1,521.88 ms | 22.968 ms |  643065.62 KB |
| NpoiXlsx            | 2,098.84 ms | 34.782 ms | 1135254.88 KB |