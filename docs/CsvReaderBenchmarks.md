# CSV Reader Benchmarks

The CSV benchmarks measure processing the contents of a 65k record CSV file containing
"SalesRecord" data.
This data set does not include any quoted fields, and thus does not test correctness of any implementation.

## Strongly-Typed Access

This first benchmark set measures accessing CSV data in a strongly-typed way. Some libraries provide
optimized access to typed field values, while other only provide access to fields as strings and require the
user to parse as appropriate. Strongly typed access can avoid allocating an intermediary string and
potentially parse the value out of an internal buffer.

| Method               | Mean      | Error      | Ratio | Allocated | Alloc Ratio |
|--------------------- |----------:|-----------:|------:|----------:|------------:|
| RecordParserManualX4 |  27.46 ms |   0.422 ms |  0.78 |  23.02 MB |        1.00 |
| SylvanManual         |  35.05 ms |   0.686 ms |  1.00 |  22.92 MB |        1.00 |
| SylvanAuto           |  35.50 ms |   0.473 ms |  1.01 |  22.92 MB |        1.00 |
| SylvanAutoAsync      |  39.04 ms |   0.328 ms |  1.11 |  22.96 MB |        1.00 |
| RecordParserManualX2 |  41.13 ms |   1.823 ms |  1.17 |  22.98 MB |        1.00 |
| SepManual            |  46.14 ms |   0.314 ms |  1.32 |  21.39 MB |        0.93 |
| FlameCsvBinder       |  48.51 ms |   0.732 ms |  1.38 |  22.88 MB |        1.00 |
| SylvanDapperAuto     |  64.02 ms |   1.099 ms |  1.83 |  61.75 MB |        2.69 |
| RecordParserManual   |  65.70 ms |   2.375 ms |  1.87 |  22.95 MB |        1.00 |
| CsvHelperManual      | 102.68 ms |   3.041 ms |  2.93 |  61.83 MB |        2.70 |
| CsvHelperAuto        | 103.38 ms |   2.641 ms |  2.95 |  61.84 MB |        2.70 |
| SoftCircuitsAuto     | 110.59 ms |  12.292 ms |  3.16 |  93.23 MB |        4.07 |
| CesilAuto            | 128.49 ms |   3.888 ms |  3.67 |  23.13 MB |        1.01 |
| TinyCsvManual        | 206.25 ms | 152.362 ms |  5.89 | 229.54 MB |       10.01 |

The "X2" and "X4" suffix indicate the degree of paralleization used. 
The RecordParser library employs parallelization in the processing of CSV records, so
while RecordParser's X4 processing is the fastest, it comes at the cost of higher CPU utilization.

Each benchmark contains an "Auto" or "Manual" suffix, indicating whether the library's automatic binder
was used, or whether the binder was written manually. The `SylvanDapperAuto` benchmark uses the generic 
`DbDataReader` binder provided by the popular Dapper library and is meant to compare the performance of
the Sylvan automatic generic binder.

## String-Only Access

This benchmark set measures CSV readers processing every field in the dataset
as a string, which is the most basic functionality common to all CSV parsers. 
Typically, string values would subsequently be parsed into a strongly-typed value, so
this benchmark might not be representative of a real-world use case, since in many cases
using strongly-typed access would be more convenient and also faster.

| Method               | Mean      | Error    | Ratio | Allocated | Alloc Ratio |
|--------------------- |----------:|---------:|------:|----------:|------------:|
| Sep                  |  12.28 ms | 0.326 ms |  0.88 |  34.21 MB |        0.96 |
| Sylvan               |  13.95 ms | 0.221 ms |  1.00 |  35.72 MB |        1.00 |
| SoftCircuits         |  17.84 ms | 0.242 ms |  1.28 |  35.72 MB |        1.00 |
| RecordParserX4       |  18.48 ms | 0.415 ms |  1.32 |  33.27 MB |        0.93 |
| NaiveBroken          |  18.83 ms | 3.350 ms |  1.35 |  61.24 MB |        1.71 |
| Fluent               |  20.47 ms | 0.364 ms |  1.47 |  61.24 MB |        1.71 |
| FlameCsvText         |  25.16 ms | 0.316 ms |  1.80 |  35.71 MB |        1.00 |
| CursivelyCsv         |  25.58 ms | 0.382 ms |  1.83 |  35.71 MB |        1.00 |
| CtlData              |  27.08 ms | 0.372 ms |  1.94 |  81.84 MB |        2.29 |
| RecordParser         |  30.31 ms | 1.274 ms |  2.17 |  33.21 MB |        0.93 |
| FlameCsvStream       |  30.93 ms | 0.163 ms |  2.22 |  35.71 MB |        1.00 |
| MgholamFastCSV       |  32.60 ms | 0.262 ms |  2.34 |  37.41 MB |        1.05 |
| NReco                |  34.66 ms | 0.547 ms |  2.48 |  35.86 MB |        1.00 |
| FastCsvParser        |  34.87 ms | 0.881 ms |  2.50 |  36.05 MB |        1.01 |
| NLightCsv            |  42.27 ms | 0.704 ms |  3.03 |  35.84 MB |        1.00 |
| CsvBySteve           |  43.41 ms | 0.761 ms |  3.11 | 107.24 MB |        3.00 |
| CsvHelper            |  48.29 ms | 2.729 ms |  3.46 |  35.85 MB |        1.00 |
| Lumenworks           |  50.23 ms | 0.655 ms |  3.60 | 166.79 MB |        4.67 |
| NotVBTextFieldParser |  50.95 ms | 1.741 ms |  3.65 |    133 MB |        3.72 |
| FSharpData           |  62.25 ms | 5.546 ms |  4.46 | 230.31 MB |        6.45 |
| FlatFilesCsv         | 114.03 ms | 1.165 ms |  8.17 | 139.52 MB |        3.91 |
| VisualBasic          | 325.01 ms | 6.123 ms | 23.30 | 1153.4 MB |       32.29 |

## CsvSum

This benchmark measures summing the values of a single column (`Total Profit`) from the CSV file.
It is meant to highlight an optimization employed by some libraries where you only "pay" for the
columns that you access.

| Method             | Mean      | Error     | Ratio | Allocated   | Alloc Ratio |
|------------------- |----------:|----------:|------:|------------:|------------:|
| SepCsv             |  7.530 ms | 0.0750 ms |  0.96 |     5.83 KB |        0.64 |
| SylvanData         |  7.848 ms | 0.0462 ms |  1.00 |     9.06 KB |        1.00 |
| RecordParserX4     |  9.781 ms | 0.2173 ms |  1.25 |   161.67 KB |       17.85 |
| FlameCsvSumByIndex | 10.249 ms | 0.0717 ms |  1.31 |     4.37 KB |        0.48 |
| FlameCsvSumByName  | 12.975 ms | 0.1785 ms |  1.65 |     4.38 KB |        0.48 |
| RecordParser       | 20.783 ms | 0.0955 ms |  2.65 |    13.98 KB |        1.54 |
| NReco              | 26.220 ms | 0.2013 ms |  3.34 |  2677.29 KB |      295.58 |
| CsvHelper          | 36.026 ms | 0.4277 ms |  4.59 |  2671.55 KB |      294.95 |
| Lumenworks         | 40.821 ms | 0.7965 ms |  5.20 | 136754.7 KB |   15,098.31 |