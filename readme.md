# CSV Benchmarks

This repository contains benchmarks for various .NET CSV libraries. These benchmarks were created to validate the performance of my own CSV library [Sylvan.Data.Csv](https://github.com/MarkPflug/Sylvan) and Sylvan.Data.

The benchmarks are authored using [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet). The various libraries provide different APIs, so the while each benchmark has slightly different construction, I believe they provide a fair comparison. If any of the benchmarks are found to be incorrectly setup, I'd happily welcome a pull request with a fix.

The benchmarks reported here were run with the following configuration

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.685 (2004/?/20H1)
Intel Core i7-7700K CPU 4.20GHz (Kaby Lake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.101
  [Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
  DefaultJob : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
```

## Running benchmarks

`benchmark.cmd`

This will compile and run the benchmark project. Then select the benchmark set to run. If your results differ wildly from mine, I'd like to hear about it.

## Libraries



## [CSV Reader Benchmarks](Docs/CsvReaderBenchmarks.md)

Benchmarks for libraries raw CSV reading capabilities.

## [CSV Data Binder Benchmarks](Docs/CsvDataBinderBenchmarks.md)

Libraries that provide object data binding capabilities are reported here.

