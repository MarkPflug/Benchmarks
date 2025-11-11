# Sylvan Benchmarks

This repository contains benchmarks for my collection of [Sylvan](https://github.com/MarkPflug/Sylvan) .NET libraries, 
comparing them with other libraries in the .NET ecosystem.
The benchmarks are written using [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet).
The various libraries provide different APIs, so the while each benchmark has slightly 
different construction, I believe they provide a fair comparison.  
If any of the benchmarks are found to be incorrectly setup, 
I'd happily welcome a pull request with a fix.

Many of the benchmarks in this collection use sample "Sales Record" data files from
[excelbianalytics.com](https://excelbianalytics.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/).
This files contains 14 columns of multiple data types including string, integer, date, and decimal.

## Running benchmarks

`benchmark.cmd`

This will compile and run the benchmark project, which will then present 
a selection for which benchmark set to run.

## Benchmark Environment

Benchmark results were updated with latest package versions as of Nov 11, 2025, as well as updating to .NET 10.0.

The benchmark results reported here were run with the following configuration, unless otherwise noted:
```
BenchmarkDotNet v0.15.6, Windows 10 (10.0.19045.6456/22H2/2022Update)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK 10.0.100
  [Host] : .NET 10.0.0 (10.0.0, 10.0.25.52411), X64 RyuJIT x86-64-v3
```

## [CSV Reader Benchmarks](docs/CsvReaderBenchmarks.md)

Benchmarks for libraries that implement CSV readers.

## [CSV Writer Benchmarks](docs/CsvWriterBenchmarks.md)

Benchmarks for libraries that implement CSV writers.

## [Excel Reader Benchmarks](docs/ExcelReaderBenchmarks.md)

Benchmarks for Excel data reader libraries processing .xlsx, .xlsb, and xls files.

## [Excel Writer Benchmarks](docs/ExcelWriterBenchmarks.md)

Benchmarks for Excel libraries writing .xlsx files.

## [XBase Reader Benchmarks](docs/XBaseDataReaderBenchmarks.md)

Benchmarks for .dbf data reader libraries.
