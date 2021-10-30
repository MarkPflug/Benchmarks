# Sylvan Benchmarks

This repository contains benchmarks for my collection of [Sylvan](https://github.com/MarkPflug/Sylvan) .NET libraries, 
comparing them with other libraries in the .NET ecosystem.
The benchmarks are written using [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).
The various libraries provide different APIs, so the while each benchmark has slightly 
different construction, I believe they provide a fair comparison.  
If any of the benchmarks are found to be incorrectly setup, 
I'd happily welcome a pull request with a fix.

## Running benchmarks

`benchmark.cmd`

This will compile and run the benchmark project, which will then present 
a selection for which benchmark set to run.

## Benchmark Environment

The benchmark results reported here were run with the following configuration, unless otherwise noted:
```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1320 (21H2)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.100-rc.2.21505.57
  [Host] : .NET 6.0.0 (6.0.21.48005), X64 RyuJIT
```

## [CSV Reader Benchmarks](docs/CsvReaderBenchmarks.md)

Benchmarks for libraries that support raw CSV reading capabilities.

## [CSV Writer Benchmarks](docs/CsvWriterBenchmarks.md)

Benchmarks for libraries that supporting writing CSV data.

## [CSV Data Binder Benchmarks](docs/CsvDataBinderBenchmarks.md)

Libraries that provide object data binding capabilities are reported here.

## [Excel Reader Benchmarks](docs/ExcelBenchmarks.md)

Benchmarks for Excel data reader libraries processing .xls and xlsx files.

## [XBase Reader Benchmarks](docs/XBaseDataReaderBenchmarks.md)

Benchmarks for .dbf data reader libraries.
