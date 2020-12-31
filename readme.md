# CSV Benchmarks

This repository contains benchmarks for various .NET CSV libraries. These benchmarks were created to validate the performance of my own CSV library [Sylvan.Data.Csv](https://github.com/MarkPflug/Sylvan) and Sylvan.Data.

The benchmarks are authored using [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). The various libraries provide different APIs, so the while each benchmark has slightly different construction, I believe they provide a fair comparison. If any of the benchmarks are found to be incorrectly setup, I'd happily welcome a pull request with a fix.

## Running benchmarks

`benchmark.cmd`

This will compile and run the benchmark project, which will then present a selection for which benchmark set to run.

## Benchmark Environment

The benchmark results reported here were run with the following configuration:
```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.685 (2004/?/20H1)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.101
  [Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
  DefaultJob : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
```

## [CSV Reader Benchmarks](docs/CsvReaderBenchmarks.md)

Benchmarks for libraries that support raw CSV reading capabilities.

## [CSV Writer Benchmarks](docs/CsvWriterBenchmarks.md)

Benchmarks for libraries that supporting writing CSV data.

## [CSV Data Binder Benchmarks](docs/CsvDataBinderBenchmarks.md)

Libraries that provide object data binding capabilities are reported here.

## [XBase Reader Benchmarks](docs/XBaseDataREaderBenchmarks.md)

Benchmarks for .dbf data reader libraries.

## Libraries Tested

I've created benchmarks that cover many of the most common open-source .NET CSV libraries. 
I'd welcome any additions to this list.

### [CsvHelper](https://github.com/JoshClose/CsvHelper)
Josh Close's CsvHelper is the most popular CSV parsing library for dotNET, as indicated by nuget package downloads. 
It is a full-featured library that supports raw data access, and also binding data to objects. 
I've used it as the baseline for benchmarks, since it is the most popular library.

### [Cesil](https://github.com/kevin-montrose/Cesil)
Kevin Montrose's Cesil library is still in pre-release development. 
As far as I know it only allows directly binding CSV data to objects and doesn't provide any raw data access mechanism.

### Naive, Broken
This measures the naive approach of using `TextReader.ReadLine` and `string.Split` to process CSV data. 

Likewise, writing is performed by writing commas and newlines, but ignoring escaping.

These approaches are fast, but don't handle the edge cases of quoted fields, embedded commas, etc; and thus are not [RFC 4180](https://tools.ietf.org/html/rfc4180) compliant.

### [Lumenworks](https://www.codeproject.com/Articles/9258/A-Fast-CSV-Reader)
Sebastien Lorion's Lumenworks CSV reader is possibly the oldest mature CSV parser for .NET. 
I remember using this library back in 2005/6.
It is now maintained as [LumenWorksCsvReader](https://github.com/phatcher/CsvReader) by Paul Hatcher.

### [NLight](https://github.com/slorion/nlight)
Sometimes one isn't enough, as this is Sebastien Lorion second CSV parser.
This library also contains a variety of other APIs unrelated to CSV.

### [FSharp.Data](https://github.com/fsharp/FSharp.Data)
The FSharp.Data library works perfectly well with C# of course, it also happens to be pretty fast.

### [VisualBasic](https://github.com/dotnet/runtime/blob/master/src/libraries/Microsoft.VisualBasic.Core/src/Microsoft/VisualBasic/FileIO/TextFieldParser.vb)
The `Microsoft.VisualBasic.FileIO.TextFieldParser` class included in the `Microsoft.VisualBasic` library that ships with dotNET.
Of course, even though it is in the VisualBasic namespace, it works just fine from any .NET language.
This type is notably only because it ships as part of the framework libraries.

### OleDbCsv
This uses the MS Access database driver via OleDb (Windows only, requires a separate install). 
It will try to automatically detect the data types of the columns in the CSV file. 
It appears that this schema detection comes at the cost of being one of the slowest CSV parsers I've tested. 
I've had negative experiences with this feature mis-detecting a column type, when the errant values appear late in a file; the result is usually an exception being thrown.
I suspect the memory metric is misrepresented, because it uses an unmanaged driver so it might not be detectable by the BenchmarkDotNet memory analyzer.

### [FastCsvParser](https://github.com/bopohaa/CsvParser)
Nikolay Vorobev's FastCsvParser is, as the name suggests, pretty fast.

### [CsvBySteve](https://github.com/stevehansen/csv/)
Steve Hansen's "Csv" library.

### [FlatFilesCsv](https://github.com/jehugaleahsa/FlatFiles)
Travis Parks' FlatFiles nuget package.

### [TinyCsvParser](https://github.com/bytefish/TinyCsvParser)
Philipp Wagner's TinyCsvParser allows binding CSV data to objects, but no raw data access mechanism.

### [NReco.Csv](https://github.com/nreco/csv)
Vitaliy Fedorchenko's NReco.Csv is an extremely fast CSV parser. 
It uses a very similar technique to Sylvan to attain the performance it does.

### [Sylvan](https://github.com/MarkPflug/Sylvan/blob/master/docs/Sylvan.Data.Csv.md)
My own Sylvan.Data.Csv library, is currently the fastest available CSV parser for dotNET that I'm aware of.
It does not offer data binding capabilities, but can be used by general purpose data binders 
such as the [Dapper](https://github.com/StackExchange/Dapper) library.

Sylvan CSV supports defining a schema for the CSV data so that the `CsvDataReader` (`DbDataReader`)
can be consumed by APIs that support schemas, such as `DataTable.Load`, `SqlBulkCopy.WriteToServer`
or using the Dapper's `GetRowParser<T>()` method.
