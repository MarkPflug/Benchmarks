# Excel Data Writer Benchmarks

These benchmarks measure writing a 65k row Excel file, with data read from in-memory objects.

| Method              | Mean       | Error     | Allocated     |
|-------------------- |-----------:|----------:|--------------:|
| SylvanXlsb          |   120.3 ms |   6.95 ms |     112.19 KB |
| SylvanXlsx          |   170.3 ms |   1.29 ms |     167.07 KB |
| LargeXlsx           |   381.8 ms |   5.70 ms |   23691.97 KB |
| MiniXl              |   472.9 ms |  11.27 ms |  431144.13 KB |
| SwiftExcelXlsx      |   475.8 ms |  11.07 ms |  130628.72 KB |
| AsposeXlsb          |   987.7 ms |  34.98 ms |  308136.93 KB |
| AsposeXlsx          | 1,072.6 ms | 159.94 ms |  248124.15 KB |
| EPPlusViaDataReader | 1,521.6 ms |  35.40 ms |  400737.41 KB |
| EPPlusXlsx          | 1,852.2 ms | 162.26 ms |  642987.66 KB |
| NanoXlsxWrite       | 2,025.1 ms |  48.05 ms | 1416882.82 KB |
| OpenXmlXlsx         | 2,901.6 ms | 215.12 ms |  689526.84 KB |
| NpoiXlsx            | 3,893.7 ms | 444.76 ms |  1232129.7 KB |