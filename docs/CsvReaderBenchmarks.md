# CSV Benchmarks

## CSV Reading

These benchmarks use a large-ish, 3254 rows by 85 columns, CSV file.

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.630 (2004/?/20H1)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.100
  [Host]     : .NET Core 5.0.0 (CoreCLR 5.0.20.51904, CoreFX 5.0.20.51904), X64 RyuJIT
  DefaultJob : .NET Core 5.0.0 (CoreCLR 5.0.20.51904, CoreFX 5.0.20.51904), X64 RyuJIT

|             Method |       Mean |     Error |    StdDev |     Median | Ratio | RatioSD |      Gen 0 |    Gen 1 |    Gen 2 |    Allocated |
|------------------- |-----------:|----------:|----------:|-----------:|------:|--------:|-----------:|---------:|---------:|-------------:|
|          CsvHelper |  23.445 ms | 0.4663 ms | 0.8527 ms |  23.238 ms |  1.00 |    0.00 |  6000.0000 |        - |        - |  27258.73 KB |
| CsvTextFieldParser |  11.993 ms | 0.2216 ms | 0.2073 ms |  11.866 ms |  0.51 |    0.02 |  5437.5000 |        - |        - |  22235.21 KB |
|      FastCsvParser |   9.180 ms | 0.0366 ms | 0.0343 ms |   9.171 ms |  0.39 |    0.01 |  1828.1250 | 125.0000 |  46.8750 |   7548.89 KB |
|         CsvBySteve | 127.528 ms | 2.4387 ms | 6.3386 ms | 125.299 ms |  5.43 |    0.33 | 22000.0000 |        - |        - |  90289.51 KB |
|         Lumenworks |  18.440 ms | 0.3656 ms | 0.5359 ms |  18.212 ms |  0.78 |    0.04 | 10468.7500 |        - |        - |  42798.25 KB |
|        NaiveBroken |   4.929 ms | 0.0810 ms | 0.1081 ms |   4.898 ms |  0.21 |    0.01 |  2757.8125 |        - |        - |  11266.87 KB |
|          NLightCsv |  13.941 ms | 0.0676 ms | 0.0632 ms |  13.939 ms |  0.59 |    0.02 |  1750.0000 |        - |        - |   7323.02 KB |
|        VisualBasic | 109.613 ms | 0.6261 ms | 0.4888 ms | 109.550 ms |  4.68 |    0.17 | 45600.0000 |        - |        - | 187058.47 KB |
|           OleDbCsv | 197.860 ms | 3.8904 ms | 3.8209 ms | 198.721 ms |  8.39 |    0.38 |  1000.0000 |        - |        - |   7812.21 KB |
|       FlatFilesCsv |  37.389 ms | 0.6692 ms | 0.6260 ms |  37.046 ms |  1.59 |    0.07 |  6285.7143 |  71.4286 |        - |  25882.75 KB |
|         FSharpData |  16.001 ms | 0.1000 ms | 0.0935 ms |  15.975 ms |  0.68 |    0.02 | 15406.2500 |  31.2500 |        - |  62950.09 KB |
|              NReco |   6.677 ms | 0.0321 ms | 0.0300 ms |   6.674 ms |  0.28 |    0.01 |  1765.6250 |  15.6250 |        - |   7214.94 KB |
|             Sylvan |   5.789 ms | 0.0238 ms | 0.0198 ms |   5.791 ms |  0.25 |    0.01 |  1757.8125 |        - |        - |   7197.21 KB |
|       SylvanSchema |   5.376 ms | 0.0192 ms | 0.0160 ms |   5.375 ms |  0.23 |    0.01 |   203.1250 |  31.2500 |        - |    858.49 KB |
|        NRecoSelect |   2.918 ms | 0.0581 ms | 0.1032 ms |   2.899 ms |  0.12 |    0.01 |   113.2813 |  15.6250 |        - |    471.01 KB |
|       SylvanSelect |   2.436 ms | 0.0149 ms | 0.0116 ms |   2.439 ms |  0.10 |    0.00 |    66.4063 |   7.8125 |        - |    282.22 KB |

## CSV Writing

These benchmarks write a 100k sequence of object data containing several typed columns as well as a "grid" of 20 doubles, to a `TextWriter.Null`.

|          Method |       Mean |     Error |    StdDev | Ratio | RatioSD |       Gen 0 | Gen 1 | Gen 2 |    Allocated |
|---------------- |-----------:|----------:|----------:|------:|--------:|------------:|------:|------:|-------------:|
|   CsvHelperSync |   693.1 ms |  11.96 ms |  10.60 ms |  1.00 |    0.00 |  39000.0000 |     - |     - | 161719.31 KB |
|  CsvHelperAsync | 6,737.3 ms | 127.27 ms | 209.10 ms |  9.81 |    0.40 | 163000.0000 |     - |     - | 662502.94 KB |
|     NaiveBroken |   289.4 ms |   1.67 ms |   1.56 ms |  0.42 |    0.01 |  21000.0000 |     - |     - |  89054.87 KB |
|       NLightCsv |   474.5 ms |   2.11 ms |   1.87 ms |  0.68 |    0.01 |  34000.0000 |     - |     - | 140778.31 KB |
|           NReco |   367.0 ms |   2.58 ms |   2.16 ms |  0.53 |    0.01 |  21000.0000 |     - |     - |  86711.21 KB |
|      SylvanSync |   298.6 ms |   3.19 ms |   2.98 ms |  0.43 |    0.01 |           - |     - |     - |     34.77 KB |
| SylvanDataAsync |   470.0 ms |   4.01 ms |   3.75 ms |  0.68 |    0.01 |           - |     - |     - |    849.66 KB |
|  SylvanDataSync |   466.7 ms |   6.38 ms |   5.96 ms |  0.67 |    0.01 |           - |     - |     - |    849.55 KB |

### CsvHelper
Josh Close's [CsvHelper](https://github.com/joshclose/CsvHelper) appears to be the go-to CSV parser for dotNET in 2020. It is a full feature library that does a lot more than just parsing CSV. I've used it as the baseline for benchmarks, since it is the most used CSV library on nuget.

The performance of using the CSV writer's async APIs was slow enough that I'm assuming I'm using it incorrectly.
I will update benchmarks if I figure out what's wrong.

### Naive Broken
This measures the naive approach of using `TextReader.ReadLine` and `string.Split` to process CSV data. It is fast, but doesn't handle the edge cases of quoted fields, embedded commas, etc; and so isn't [RFC 4180](https://tools.ietf.org/html/rfc4180) compliant.

Likewise, the writing test is performed by writing commas and newlines, but ignoring escaping.

### FSharp.Data
The FSharp.Data library works perfectly well with C# of course, it also happens to be pretty fast.

### VisualBasic
This is `TextFieldParser` included in the Microsoft.VisualBasic library that ships with dotNET. I include this, because it is the only option included as part of the framework libraries.

### OleDbCsv
This benchmark uses the MS Access database driver via OleDb (Windows only, requires a separate install). 
It does do something that no other parser appears to support: it will try to detect the data types of the columns in the CSV file. 
My understanding is that this is done by analyzing the first N rows of the CSV. That comes at the cost of being one of the slowest CSV parsers tested. 
I've had negative experiences with this feature mis-detecting a column type, when the errant values appear later in a file; the result is usually an exception being thrown.
I suspect the memory metric is misrepresented, because it uses an unmanaged driver so it might not be detectable by the BenchmarkDotNet memory analyzer.

### FastCsvParser
As the name suggests, it is pretty fast.

### CsvBySteve
This is the `Csv` nuget package, by "Steve".

### FlatFilesCsv
The csv parser from the `FlatFiles` nuget package.

### NReco
Vitaliy Fedorchenko's [NReco.Csv](https://github.com/nreco/csv) is an extremely fast CSV parser. 
It uses a very similar technique to Sylvan to attain the performance it does.

### Sylvan
The Sylvan.Data.Csv library, is currently the fastest available CSV parser for dotNET that I'm aware of.

Sylvan offers two CSV writing APIs: `CsvWriter` which offers raw writing capabilities similar to other libraries, and `CsvDataWriter` which writes `DbDataReader` data to a `CsvWriter`.

### SylvanSchema
This measures using the Sylvan CsvDataReader with a provided schema. 
The schema allows parsing primitive values directly out of the text buffer.
This adds a slight amount of time to parse the primitive values, but this is time which would be spent in `Parse` methods anyway if consuming only strings.
This also reduces allocations, since it avoid producing intermediate strings.

### *Select
The approach that Sylvan and NReco use for processing CSV make them even more efficient when reading only a subset of the columns in a file. These benchmarks measures reading only 3 of the 85 columns.
