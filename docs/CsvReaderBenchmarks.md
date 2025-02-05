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
| RecordParserManualX4 |  28.02 ms |   1.012 ms |  0.77 |     23 MB |        1.00 |
| SylvanManual         |  36.18 ms |   0.445 ms |  1.00 |  22.92 MB |        1.00 |
| SylvanAuto           |  36.98 ms |   0.318 ms |  1.02 |  22.92 MB |        1.00 |
| SylvanAutoAsync      |  40.33 ms |   0.261 ms |  1.11 |  22.96 MB |        1.00 |
| RecordParserManualX2 |  41.53 ms |   0.923 ms |  1.15 |  22.97 MB |        1.00 |
| SepManual            |  48.81 ms |   0.670 ms |  1.35 |  21.39 MB |        0.93 |
| SylvanDapperAuto     |  65.19 ms |   0.700 ms |  1.80 |  61.75 MB |        2.69 |
| RecordParserManual   |  67.04 ms |   1.033 ms |  1.85 |  22.94 MB |        1.00 |
| CsvHelperManual      | 105.49 ms |   1.142 ms |  2.92 |  63.32 MB |        2.76 |
| SoftCircuitsAuto     | 107.08 ms |   1.034 ms |  2.96 |  93.23 MB |        4.07 |
| CsvHelperAuto        | 109.96 ms |   9.651 ms |  3.04 |  63.33 MB |        2.76 |
| CesilAuto            | 130.87 ms |   1.263 ms |  3.62 |  23.14 MB |        1.01 |
| TinyCsvManual        | 221.35 ms | 149.364 ms |  6.12 | 229.49 MB |       10.01 |

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
| Sep                  |  12.36 ms |  0.198 ms |  0.89 |  34.21 MB |        0.96 |
| Sylvan               |  13.89 ms |  0.252 ms |  1.00 |  35.72 MB |        1.00 |
| RecordParserX4       |  18.16 ms |  0.142 ms |  1.31 |  33.25 MB |        0.93 |
| NaiveBroken          |  18.36 ms |  0.805 ms |  1.32 |  61.24 MB |        1.71 |
| SoftCircuits         |  18.53 ms |  1.151 ms |  1.33 |  35.72 MB |        1.00 |
| Fluent               |  20.74 ms |  0.279 ms |  1.49 |  61.24 MB |        1.71 |
| CursivelyCsv         |  25.92 ms |  0.256 ms |  1.87 |  35.71 MB |        1.00 |
| CtlData              |  27.44 ms |  0.519 ms |  1.97 |  81.84 MB |        2.29 |
| RecordParser         |  29.35 ms |  0.378 ms |  2.11 |  33.21 MB |        0.93 |
| FastCsvParser        |  34.52 ms |  0.093 ms |  2.49 |  36.05 MB |        1.01 |
| MgholamFastCSV       |  35.11 ms |  0.260 ms |  2.53 |  37.41 MB |        1.05 |
| NReco                |  35.15 ms |  0.149 ms |  2.53 |  35.86 MB |        1.00 |
| NLightCsv            |  43.54 ms |  0.596 ms |  3.13 |  35.84 MB |        1.00 |
| CsvHelper            |  46.82 ms |  0.316 ms |  3.37 |  35.85 MB |        1.00 |
| Lumenworks           |  48.37 ms |  0.054 ms |  3.48 | 166.79 MB |        4.67 |
| NotVBTextFieldParser |  50.95 ms |  0.680 ms |  3.67 |    133 MB |        3.72 |
| FSharpData           |  60.82 ms |  1.071 ms |  4.38 | 230.31 MB |        6.45 |
| FlatFilesCsv         | 116.93 ms |  1.704 ms |  8.42 | 139.52 MB |        3.91 |
| CsvBySteve           | 165.32 ms |  3.103 ms | 11.90 | 334.26 MB |        9.36 |
| VisualBasic          | 435.72 ms | 72.219 ms | 31.37 | 1153.4 MB |       32.29 |

## CsvSum

This benchmark measures summing the values of a single column (`Total Profit`) from the CSV file.
It is meant to highlight an optimization employed by some libraries where you only "pay" for the
columns that you access.

| Method         | Mean      | Error     | Ratio | Allocated    | Alloc Ratio |
|--------------- |----------:|----------:|------:|-------------:|------------:|
| SepCsv         |  8.399 ms | 0.0010 ms |  1.00 |      5.86 KB |        0.64 |
| SylvanData     |  8.401 ms | 0.0997 ms |  1.00 |       9.1 KB |        1.00 |
| RecordParserX4 |  8.825 ms | 0.1030 ms |  1.05 |    114.44 KB |       12.58 |
| RecordParser   | 20.045 ms | 0.1530 ms |  2.39 |     14.17 KB |        1.56 |
| NReco          | 26.793 ms | 0.2217 ms |  3.19 |   2677.27 KB |      294.25 |
| CsvHelper      | 36.786 ms | 0.4531 ms |  4.38 |   2671.52 KB |      293.62 |
| Lumenworks     | 41.080 ms | 0.7549 ms |  4.89 | 136754.65 KB |   15,030.24 |