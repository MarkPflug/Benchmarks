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
| RecordParserManualX4 |  27.67 ms |   0.511 ms |  0.76 |     23 MB |        1.00 |
| SylvanManual         |  36.30 ms |   0.626 ms |  1.00 |  22.92 MB |        1.00 |
| SylvanAuto           |  37.29 ms |   0.854 ms |  1.03 |  22.92 MB |        1.00 |
| SylvanAutoAsync      |  40.50 ms |   0.459 ms |  1.12 |  22.96 MB |        1.00 |
| RecordParserManualX2 |  40.95 ms |   0.615 ms |  1.13 |  22.96 MB |        1.00 |
| SepManual            |  48.48 ms |   0.070 ms |  1.34 |  21.39 MB |        0.93 |
| FlameCsvBinder       |  52.17 ms |   0.539 ms |  1.44 |  22.93 MB |        1.00 |
| SylvanDapperAuto     |  66.03 ms |   0.577 ms |  1.82 |  61.75 MB |        2.69 |
| RecordParserManual   |  67.51 ms |   1.305 ms |  1.86 |  22.94 MB |        1.00 |
| SoftCircuitsAuto     | 105.38 ms |   1.709 ms |  2.90 |  93.23 MB |        4.07 |
| CsvHelperManual      | 106.22 ms |   0.593 ms |  2.93 |  63.33 MB |        2.76 |
| CsvHelperAuto        | 107.85 ms |   0.927 ms |  2.97 |  63.33 MB |        2.76 |
| CesilAuto            | 128.57 ms |   2.427 ms |  3.54 |  23.14 MB |        1.01 |
| TinyCsvManual        | 218.65 ms | 146.797 ms |  6.02 | 229.51 MB |       10.01 |

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

| Method               | Mean      | Error     | Ratio | Allocated | Alloc Ratio |
|--------------------- |----------:|----------:|------:|----------:|------------:|
| Sep                  |  12.02 ms |  0.159 ms |  0.89 |  34.21 MB |        0.96 |
| Sylvan               |  13.57 ms |  0.201 ms |  1.00 |  35.72 MB |        1.00 |
| SoftCircuits         |  17.47 ms |  0.111 ms |  1.29 |  35.72 MB |        1.00 |
| RecordParserX4       |  17.83 ms |  0.307 ms |  1.31 |  33.25 MB |        0.93 |
| Fluent               |  20.45 ms |  0.220 ms |  1.51 |  61.24 MB |        1.71 |
| NaiveBroken          |  23.16 ms | 16.807 ms |  1.71 |  61.24 MB |        1.71 |
| CursivelyCsv         |  25.69 ms |  0.409 ms |  1.89 |  35.71 MB |        1.00 |
| CtlData              |  26.65 ms |  0.440 ms |  1.96 |  81.84 MB |        2.29 |
| RecordParser         |  28.77 ms |  0.308 ms |  2.12 |  33.21 MB |        0.93 |
| FlameCsvText         |  30.02 ms |  0.571 ms |  2.21 |  35.76 MB |        1.00 |
| MgholamFastCSV       |  33.53 ms |  0.429 ms |  2.47 |  37.41 MB |        1.05 |
| FastCsvParser        |  33.54 ms |  0.602 ms |  2.47 |  36.05 MB |        1.01 |
| NReco                |  33.81 ms |  0.581 ms |  2.49 |  35.86 MB |        1.00 |
| FlameCsvStream       |  40.59 ms |  0.695 ms |  2.99 |  35.75 MB |        1.00 |
| NLightCsv            |  43.02 ms |  0.387 ms |  3.17 |  35.84 MB |        1.00 |
| CsvHelper            |  46.53 ms |  0.897 ms |  3.43 |  35.85 MB |        1.00 |
| Lumenworks           |  48.62 ms |  1.091 ms |  3.58 | 166.79 MB |        4.67 |
| NotVBTextFieldParser |  50.49 ms |  0.854 ms |  3.72 |    133 MB |        3.72 |
| FSharpData           |  60.89 ms |  3.023 ms |  4.49 | 230.31 MB |        6.45 |
| FlatFilesCsv         | 114.97 ms |  1.759 ms |  8.47 | 139.52 MB |        3.91 |
| CsvBySteve           | 167.01 ms |  0.881 ms | 12.31 | 334.26 MB |        9.36 |
| VisualBasic          | 391.23 ms |  7.716 ms | 28.83 | 1153.4 MB |       32.29 |

## CsvSum

This benchmark measures summing the values of a single column (`Total Profit`) from the CSV file.
It is meant to highlight an optimization employed by some libraries where you only "pay" for the
columns that you access.

| Method             | Mean      | Error     | Ratio | Allocated    | Alloc Ratio |
|------------------- |----------:|----------:|------:|-------------:|------------:|
| SepCsv             |  8.121 ms | 0.0475 ms |  0.98 |      5.88 KB |        0.64 |
| SylvanData         |  8.271 ms | 0.0410 ms |  1.00 |      9.12 KB |        1.00 |
| RecordParserX4     |  8.416 ms | 0.2568 ms |  1.02 |    108.63 KB |       11.91 |
| FlameCsvSumByIndex | 11.253 ms | 0.0480 ms |  1.36 |     51.55 KB |        5.65 |
| FlameCsvSumByName  | 13.454 ms | 0.1564 ms |  1.63 |     51.55 KB |        5.65 |
| RecordParser       | 19.770 ms | 0.1429 ms |  2.39 |      14.2 KB |        1.56 |
| NReco              | 26.166 ms | 0.3210 ms |  3.16 |    2677.3 KB |      293.56 |
| CsvHelper          | 36.434 ms | 0.4320 ms |  4.40 |   2671.59 KB |      292.93 |
| Lumenworks         | 40.722 ms | 0.6793 ms |  4.92 | 136754.73 KB |   14,994.84 |