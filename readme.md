# Sylvan Benchmarks

This repository contains benchmarks for [Sylvan](https://github.com/MarkPflug/Sylvan) .NET libraries, comparing them with other libraries in the .NET ecosystem, using [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). The various libraries provide different APIs, so the while each benchmark has slightly different construction, I believe they provide a fair comparison.  If any of the benchmarks are found to be incorrectly setup, I'd happily welcome a pull request with a fix.

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

## [XBase Reader Benchmarks](docs/XBaseDataReaderBenchmarks.md)

Benchmarks for .dbf data reader libraries.

## Libraries Tested

I've created benchmarks that cover many of the most common open-source .NET CSV libraries.
These are primarily third-party open source libraries, but also tested are the Visual Basic TextFieldParser which included as
part of the .NET framework libraries, and OleDb text file driver, which is a windows-only and maintained by Microsoft.
I'd welcome any additions to this list as a pull request.

All libraries are using the latest version as of 2021-03-22.

- Naive, Broken

	This measures the naive approach of using `TextReader.ReadLine` and `String.Split` to process CSV data. 
	Likewise, writing is performed by writing commas and newlines, but ignoring escaping.
	These approaches are fast, but don't handle the edge cases of quoted fields, embedded commas, etc; and thus are not [RFC 4180](https://tools.ietf.org/html/rfc4180) compliant.
	This is used as the benchmark baseline, since its performance is unlikely to change much.

- [Cesil](https://github.com/kevin-montrose/Cesil)
- [CsvHelper](https://github.com/JoshClose/CsvHelper)
- [CsvBySteve](https://github.com/stevehansen/csv/)
- [Ctl.Data](https://github.com/ctl-global/ctl-data/)
- [Cursively](https://github.com/airbreather/Cursively)
- [FastCsvParser](https://github.com/bopohaa/CsvParser)
- [FlatFilesCsv](https://github.com/jehugaleahsa/FlatFiles)
- [FluentCSV](https://github.com/aboudoux/FluentCSV)
- [FSharp.Data](https://github.com/fsharp/FSharp.Data)
- [Lumenworks](https://www.codeproject.com/Articles/9258/A-Fast-CSV-Reader) now maintained as [LumenWorksCsvReader](https://github.com/phatcher/CsvReader).
- [mhgolam.fastCSV](https://github.com/mgholam/fastCSV)
- [NLight](https://github.com/slorion/nlight)
- [NReco.Csv](https://github.com/nreco/csv)
- [OleDbCsv](https://www.microsoft.com/en-us/download/details.aspx?id=54920)
- [Sylvan](https://github.com/MarkPflug/Sylvan/blob/master/docs/Sylvan.Data.Csv.md)
- [TinyCsvParser](https://github.com/bytefish/TinyCsvParser)
- [VisualBasic](https://github.com/dotnet/runtime/blob/master/src/libraries/Microsoft.VisualBasic.Core/src/Microsoft/VisualBasic/FileIO/TextFieldParser.vb)