# CSV Reader Benchmarks

The CSV benchmarks measure processing the contents of a 65k record CSV file containing
"SalesRecord" data.
This data set does not include any quoted fields, and thus does not test correctness of any implementation.

## Strongly-Typed Access

This first benchmark set measures accessing CSV data in a strongly-typed way. Some libraries provide
optimized access to typed field values, while other only provide access to fields as strings and require the
user to parse as appropriate. Strongly typed access can avoid allocating an intermediary string and
potentially parse the value out of an internal buffer.

| Method               | Mean     | Error     | Median   | Ratio | Allocated | Alloc Ratio |
|--------------------- |---------:|----------:|---------:|------:|----------:|------------:|
| RecordParserManualX4 | 13.15 ms |  0.244 ms | 13.15 ms |  0.67 |  23.03 MB |        1.00 |
| SylvanManual         | 19.60 ms |  0.249 ms | 19.61 ms |  1.00 |  22.92 MB |        1.00 |
| SylvanAuto           | 19.81 ms |  0.390 ms | 19.80 ms |  1.01 |  22.92 MB |        1.00 |
| SylvanAutoAsync      | 21.34 ms |  0.273 ms | 21.33 ms |  1.09 |  22.96 MB |        1.00 |
| RecordParserManualX2 | 23.05 ms |  0.289 ms | 23.05 ms |  1.18 |  22.97 MB |        1.00 |
| SepManual            | 25.03 ms |  0.500 ms | 25.01 ms |  1.28 |  21.39 MB |        0.93 |
| FlameCsvBinder       | 28.20 ms |  0.384 ms | 28.25 ms |  1.44 |  22.88 MB |        1.00 |
| SylvanDapperAuto     | 35.34 ms |  0.684 ms | 35.34 ms |  1.80 |  61.75 MB |        2.69 |
| RecordParserManual   | 38.46 ms |  0.397 ms | 38.44 ms |  1.96 |  22.95 MB |        1.00 |
| CsvHelperManual      | 55.31 ms |  0.945 ms | 55.26 ms |  2.82 |  61.84 MB |        2.70 |
| CsvHelperAuto        | 55.87 ms |  1.226 ms | 55.92 ms |  2.85 |  61.84 MB |        2.70 |
| SoftCircuitsAuto     | 61.24 ms |  1.390 ms | 61.12 ms |  3.12 |  93.23 MB |        4.07 |
| TinyCsvManual        | 75.69 ms | 90.947 ms | 55.88 ms |  3.86 | 229.68 MB |       10.02 |
| CesilAuto            | 77.31 ms |  1.108 ms | 77.32 ms |  3.94 |  23.13 MB |        1.01 |

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

| Method               | Mean       | Error     | Ratio | Allocated  | Alloc Ratio |
|--------------------- |-----------:|----------:|------:|-----------:|------------:|
| Sep                  |   7.743 ms | 0.1211 ms |  0.96 |   34.21 MB |        0.96 |
| Sylvan               |   8.105 ms | 0.1549 ms |  1.00 |   35.72 MB |        1.00 |
| RecordParserX4       |   8.206 ms | 0.2606 ms |  1.01 |    33.3 MB |        0.93 |
| NaiveBroken          |  10.405 ms | 0.1662 ms |  1.28 |   61.24 MB |        1.71 |
| SoftCircuits         |  12.681 ms | 0.2248 ms |  1.56 |   35.72 MB |        1.00 |
| Fluent               |  14.992 ms | 0.1618 ms |  1.85 |   61.24 MB |        1.71 |
| CtlData              |  16.611 ms | 0.3309 ms |  2.05 |   81.84 MB |        2.29 |
| MgholamFastCSV       |  16.676 ms | 0.1186 ms |  2.06 |   37.41 MB |        1.05 |
| FlameCsvText         |  16.685 ms | 0.1834 ms |  2.06 |   35.71 MB |        1.00 |
| CursivelyCsv         |  16.838 ms | 0.2796 ms |  2.08 |   35.71 MB |        1.00 |
| FastCsvParser        |  17.502 ms | 0.2582 ms |  2.16 |   36.05 MB |        1.01 |
| NReco                |  19.014 ms | 0.3748 ms |  2.35 |   35.86 MB |        1.00 |
| FlameCsvStream       |  20.806 ms | 0.3013 ms |  2.57 |   35.71 MB |        1.00 |
| RecordParser         |  21.479 ms | 0.3170 ms |  2.65 |   33.21 MB |        0.93 |
| NLightCsv            |  24.635 ms | 0.3848 ms |  3.04 |   35.84 MB |        1.00 |
| CsvBySteve           |  24.887 ms | 0.1839 ms |  3.07 |  107.24 MB |        3.00 |
| CsvHelper            |  25.429 ms | 0.2741 ms |  3.14 |   35.85 MB |        1.00 |
| Lumenworks           |  29.369 ms | 0.2557 ms |  3.62 |  166.79 MB |        4.67 |
| NotVBTextFieldParser |  29.760 ms | 0.2860 ms |  3.67 |  130.18 MB |        3.64 |
| FSharpData           |  34.187 ms | 0.2576 ms |  4.22 |  230.31 MB |        6.45 |
| FlatFilesCsv         |  63.908 ms | 1.4948 ms |  7.89 |   139.4 MB |        3.90 |
| VisualBasic          | 185.812 ms | 1.4951 ms | 22.93 | 1112.46 MB |       31.15 |


## CsvSum

This benchmark measures summing the values of a single column (`Total Profit`) from the CSV file.
It is meant to highlight an optimization employed by some libraries where you only "pay" for the
columns that you access.

| Method             | Mean      | Error     | Ratio | Allocated    | Alloc Ratio |
|------------------- |----------:|----------:|------:|-------------:|------------:|
| SepCsv             |  4.200 ms | 0.0926 ms |  0.91 |      5.84 KB |        0.64 |
| SylvanData         |  4.635 ms | 0.0543 ms |  1.00 |      9.06 KB |        1.00 |
| RecordParserX4     |  5.247 ms | 0.0843 ms |  1.13 |       563 KB |       62.16 |
| FlameCsvSumByIndex |  6.426 ms | 0.0433 ms |  1.39 |      4.35 KB |        0.48 |
| FlameCsvSumByName  |  7.695 ms | 0.0860 ms |  1.66 |      4.35 KB |        0.48 |
| RecordParser       | 14.118 ms | 0.1412 ms |  3.05 |     14.07 KB |        1.55 |
| NReco              | 14.150 ms | 0.3217 ms |  3.05 |   2677.34 KB |      295.62 |
| CsvHelper          | 18.892 ms | 0.3285 ms |  4.08 |   2671.38 KB |      294.96 |
| Lumenworks         | 24.542 ms | 0.3392 ms |  5.30 | 136754.59 KB |   15,099.92 |