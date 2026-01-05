# CSV Reader Benchmarks

The CSV benchmarks measure processing the contents of a 65k record CSV file containing
"SalesRecord" data.
This data set does not include any quoted fields, and thus does not test correctness of any implementation.

## Strongly-Typed Access

This first benchmark set measures accessing CSV data in a strongly-typed way. Some libraries provide
optimized access to typed field values, while other only provide access to fields as strings and require the
user to parse as appropriate. Strongly typed access can avoid allocating an intermediary string and
potentially parse the value out of an internal buffer.


| Method                | Mean     | Error     | Median   | Ratio | Allocated | Alloc Ratio |
|---------------------- |---------:|----------:|---------:|------:|----------:|------------:|
| RecordParserManualX4  | 12.61 ms |  0.178 ms | 12.62 ms |  0.63 |  23.05 MB |        1.01 |
| SylvanManual          | 19.89 ms |  0.353 ms | 19.87 ms |  1.00 |  22.92 MB |        1.00 |
| SylvanAuto            | 20.20 ms |  0.162 ms | 20.19 ms |  1.02 |  22.92 MB |        1.00 |
| SylvanAutoAsync       | 21.60 ms |  0.328 ms | 21.57 ms |  1.09 |  22.96 MB |        1.00 |
| RecordParserManualX2  | 22.87 ms |  0.278 ms | 22.87 ms |  1.15 |  22.97 MB |        1.00 |
| SepManual             | 25.87 ms |  0.450 ms | 25.88 ms |  1.30 |  21.39 MB |        0.93 |
| FourLambdaUtf8Manual  | 29.04 ms |  0.382 ms | 29.03 ms |  1.46 |  22.89 MB |        1.00 |
| FlameCsvBinder        | 30.17 ms |  2.183 ms | 30.26 ms |  1.52 |  22.88 MB |        1.00 |
| SylvanDapperAuto      | 34.91 ms |  0.627 ms | 34.86 ms |  1.76 |  61.75 MB |        2.69 |
| RecordParserManual    | 38.99 ms |  0.348 ms | 38.97 ms |  1.96 |  22.95 MB |        1.00 |
| FourLambdaUtf16Manual | 45.50 ms |  0.640 ms | 45.43 ms |  2.29 |  22.89 MB |        1.00 |
| CsvHelperManual       | 59.05 ms |  4.096 ms | 58.60 ms |  2.97 |  61.83 MB |        2.70 |
| CsvHelperAuto         | 60.55 ms |  7.215 ms | 59.39 ms |  3.04 |  61.83 MB |        2.70 |
| SoftCircuitsAuto      | 63.01 ms |  0.856 ms | 63.00 ms |  3.17 |  93.23 MB |        4.07 |
| TinyCsvManual         | 75.86 ms | 87.130 ms | 57.58 ms |  3.81 | 229.77 MB |       10.02 |
| CesilAuto             | 81.93 ms |  8.657 ms | 81.62 ms |  4.12 |  23.13 MB |        1.01 |

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

| Method               | Mean       | Error     | Ratio | Allocated | Alloc Ratio |
|--------------------- |-----------:|----------:|------:|----------:|------------:|
| Sep                  |   7.728 ms | 0.1135 ms |  0.94 |  34.21 MB |        0.96 |
| RecordParserX4       |   8.013 ms | 0.5079 ms |  0.98 |   33.3 MB |        0.93 |
| Sylvan               |   8.178 ms | 0.1546 ms |  1.00 |  35.72 MB |        1.00 |
| FourLambdaUtf16      |   9.208 ms | 0.1736 ms |  1.13 |  35.71 MB |        1.00 |
| NaiveBroken          |  10.456 ms | 0.2017 ms |  1.28 |  61.24 MB |        1.71 |
| FourLambdaUtf8       |  11.998 ms | 0.1515 ms |  1.47 |  35.71 MB |        1.00 |
| SoftCircuits         |  13.121 ms | 0.1227 ms |  1.60 |  35.72 MB |        1.00 |
| Fluent               |  15.158 ms | 0.2758 ms |  1.85 |  61.24 MB |        1.71 |
| FlameCsvText         |  16.757 ms | 0.2441 ms |  2.05 |  35.71 MB |        1.00 |
| CtlData              |  16.828 ms | 0.3297 ms |  2.06 |  81.84 MB |        2.29 |
| MgholamFastCSV       |  17.247 ms | 0.2633 ms |  2.11 |  37.41 MB |        1.05 |
| CursivelyCsv         |  17.271 ms | 0.1742 ms |  2.11 |  35.71 MB |        1.00 |
| FastCsvParser        |  17.976 ms | 0.3327 ms |  2.20 |  36.05 MB |        1.01 |
| NReco                |  19.059 ms | 0.3797 ms |  2.33 |  35.86 MB |        1.00 |
| FlameCsvStream       |  20.879 ms | 0.2326 ms |  2.55 |  35.71 MB |        1.00 |
| RecordParser         |  21.445 ms | 0.3054 ms |  2.62 |  33.21 MB |        0.93 |
| NLightCsv            |  24.907 ms | 0.1853 ms |  3.05 |  35.84 MB |        1.00 |
| CsvBySteve           |  25.356 ms | 0.5995 ms |  3.10 | 107.24 MB |        3.00 |
| CsvHelper            |  28.058 ms | 0.2849 ms |  3.43 |  35.85 MB |        1.00 |
| Lumenworks           |  29.341 ms | 0.3856 ms |  3.59 | 166.79 MB |        4.67 |
| NotVBTextFieldParser |  31.374 ms | 0.4125 ms |  3.84 |    133 MB |        3.72 |
| FSharpData           |  34.669 ms | 0.6386 ms |  4.24 | 230.31 MB |        6.45 |
| FlatFilesCsv         |  64.697 ms | 1.7705 ms |  7.91 | 139.52 MB |        3.91 |
| VisualBasic          | 191.554 ms | 2.8246 ms | 23.42 | 1153.4 MB |       32.29 |

## CsvSum

This benchmark measures summing the values of a single column (`Total Profit`) from the CSV file.
It is meant to highlight an optimization employed by some libraries where you only "pay" for the
columns that you access.

| Method             | Mean      | Error     | Ratio | Allocated    | Alloc Ratio |
|------------------- |----------:|----------:|------:|-------------:|------------:|
| SepCsv             |  4.273 ms | 0.0610 ms |  0.92 |      5.84 KB |        0.64 |
| FourLambdaUtf8     |  4.392 ms | 0.0798 ms |  0.95 |      5.51 KB |        0.61 |
| SylvanData         |  4.640 ms | 0.0454 ms |  1.00 |      9.06 KB |        1.00 |
| RecordParserX4     |  5.290 ms | 0.0812 ms |  1.14 |    459.31 KB |       50.72 |
| FlameCsvSumByIndex |  6.434 ms | 0.0903 ms |  1.39 |      4.33 KB |        0.48 |
| FourLambdaUtf16    |  6.769 ms | 0.1350 ms |  1.46 |      8.71 KB |        0.96 |
| FlameCsvSumByName  |  7.761 ms | 0.1012 ms |  1.67 |      4.34 KB |        0.48 |
| NReco              | 13.881 ms | 0.1126 ms |  2.99 |   2677.22 KB |      295.61 |
| RecordParser       | 14.136 ms | 0.2378 ms |  3.05 |     14.05 KB |        1.55 |
| CsvHelper          | 19.167 ms | 0.2933 ms |  4.13 |   2671.38 KB |      294.96 |
| Lumenworks         | 24.407 ms | 0.4384 ms |  5.26 | 136754.47 KB |   15,099.91 |
