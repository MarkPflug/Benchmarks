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
| RecordParserManualX4 | 12.99 ms |  0.068 ms | 12.99 ms |  0.64 |  23.01 MB |        1.00 |
| SylvanAuto           | 20.12 ms |  0.141 ms | 20.12 ms |  0.99 |  22.92 MB |        1.00 |
| SylvanManual         | 20.35 ms |  0.299 ms | 20.35 ms |  1.00 |  22.92 MB |        1.00 |
| SylvanAutoAsync      | 21.20 ms |  0.320 ms | 21.18 ms |  1.04 |  22.96 MB |        1.00 |
| RecordParserManualX2 | 23.05 ms |  0.336 ms | 23.02 ms |  1.13 |  22.97 MB |        1.00 |
| SepManual            | 25.24 ms |  0.430 ms | 25.24 ms |  1.24 |  21.39 MB |        0.93 |
| FlameCsvBinder       | 28.21 ms |  0.536 ms | 28.15 ms |  1.39 |  22.88 MB |        1.00 |
| FourLambdaManual     | 30.07 ms |  0.357 ms | 30.06 ms |  1.48 |  22.88 MB |        1.00 |
| SylvanDapperAuto     | 35.34 ms |  0.423 ms | 35.37 ms |  1.74 |  61.75 MB |        2.69 |
| RecordParserManual   | 38.24 ms |  0.501 ms | 38.20 ms |  1.88 |  22.95 MB |        1.00 |
| CsvHelperManual      | 55.54 ms |  0.911 ms | 55.44 ms |  2.73 |  61.83 MB |        2.70 |
| CsvHelperAuto        | 55.82 ms |  0.989 ms | 55.72 ms |  2.74 |  61.84 MB |        2.70 |
| SoftCircuitsAuto     | 61.98 ms |  1.329 ms | 62.04 ms |  3.05 |  93.23 MB |        4.07 |
| TinyCsvManual        | 74.33 ms | 90.445 ms | 56.41 ms |  3.65 | 229.75 MB |       10.02 |
| CesilAuto            | 76.34 ms |  1.303 ms | 76.42 ms |  3.75 |  23.13 MB |        1.01 |

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
| Sep                  |   7.675 ms | 0.2069 ms |  0.93 |   34.21 MB |        0.96 |
| RecordParserX4       |   8.071 ms | 0.3159 ms |  0.98 |   33.26 MB |        0.93 |
| Sylvan               |   8.277 ms | 0.2785 ms |  1.00 |   35.72 MB |        1.00 |
| NaiveBroken          |  10.433 ms | 0.3587 ms |  1.26 |   61.24 MB |        1.71 |
| SoftCircuits         |  12.853 ms | 0.1913 ms |  1.55 |   35.72 MB |        1.00 |
| Fluent               |  15.071 ms | 0.2505 ms |  1.82 |   61.24 MB |        1.71 |
| FourLambda           |  15.077 ms | 0.2071 ms |  1.82 |   35.71 MB |        1.00 |
| MgholamFastCSV       |  16.533 ms | 0.3145 ms |  2.00 |   37.41 MB |        1.05 |
| CtlData              |  16.566 ms | 0.3521 ms |  2.00 |   81.84 MB |        2.29 |
| FlameCsvText         |  16.727 ms | 0.2772 ms |  2.02 |   35.71 MB |        1.00 |
| CursivelyCsv         |  16.947 ms | 0.2877 ms |  2.05 |   35.71 MB |        1.00 |
| FastCsvParser        |  17.641 ms | 0.3890 ms |  2.13 |   36.05 MB |        1.01 |
| NReco                |  19.011 ms | 0.3759 ms |  2.30 |   35.86 MB |        1.00 |
| FlameCsvStream       |  20.954 ms | 0.4742 ms |  2.53 |   35.71 MB |        1.00 |
| RecordParser         |  21.482 ms | 0.2870 ms |  2.60 |   33.21 MB |        0.93 |
| NLightCsv            |  24.721 ms | 0.2083 ms |  2.99 |   35.84 MB |        1.00 |
| CsvBySteve           |  25.080 ms | 0.4543 ms |  3.03 |  107.24 MB |        3.00 |
| CsvHelper            |  26.075 ms | 0.4617 ms |  3.15 |   35.85 MB |        1.00 |
| Lumenworks           |  29.631 ms | 0.5295 ms |  3.58 |  166.79 MB |        4.67 |
| NotVBTextFieldParser |  29.757 ms | 0.5816 ms |  3.60 |  130.18 MB |        3.64 |
| FSharpData           |  34.634 ms | 0.7864 ms |  4.18 |  230.31 MB |        6.45 |
| FlatFilesCsv         |  64.022 ms | 2.5198 ms |  7.74 |   139.4 MB |        3.90 |
| VisualBasic          | 186.569 ms | 3.1203 ms | 22.54 | 1112.46 MB |       31.15 |
## CsvSum

This benchmark measures summing the values of a single column (`Total Profit`) from the CSV file.
It is meant to highlight an optimization employed by some libraries where you only "pay" for the
columns that you access.

| Method             | Mean      | Error     | Ratio | Allocated    | Alloc Ratio |
|------------------- |----------:|----------:|------:|-------------:|------------:|
| SepCsv             |  4.229 ms | 0.0607 ms |  0.92 |      5.84 KB |        0.64 |
| FourLambdaCsv      |  4.400 ms | 0.0893 ms |  0.95 |      4.28 KB |        0.47 |
| SylvanData         |  4.620 ms | 0.0658 ms |  1.00 |      9.06 KB |        1.00 |
| RecordParserX4     |  5.201 ms | 0.0673 ms |  1.13 |    327.91 KB |       36.21 |
| FlameCsvSumByIndex |  6.394 ms | 0.0637 ms |  1.38 |      4.35 KB |        0.48 |
| FlameCsvSumByName  |  7.812 ms | 0.1283 ms |  1.69 |      4.35 KB |        0.48 |
| RecordParser       | 14.101 ms | 0.2005 ms |  3.05 |     14.13 KB |        1.56 |
| NReco              | 14.210 ms | 0.2419 ms |  3.08 |   2677.35 KB |      295.62 |
| CsvHelper          | 18.866 ms | 0.2999 ms |  4.08 |   2671.37 KB |      294.96 |
| Lumenworks         | 24.726 ms | 0.4806 ms |  5.35 | 136754.61 KB |   15,099.93 |