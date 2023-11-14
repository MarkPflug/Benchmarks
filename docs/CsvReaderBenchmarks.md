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
| RecordParserManualX4 |  30.61 ms |   0.664 ms |  0.80 |     23 MB |        1.00 |
| SylvanManual         |  38.43 ms |   0.705 ms |  1.00 |  22.92 MB |        1.00 |
| SylvanAuto           |  39.29 ms |   0.426 ms |  1.02 |  22.92 MB |        1.00 |
| SylvanAutoAsync      |  42.85 ms |   0.724 ms |  1.11 |  22.96 MB |        1.00 |
| RecordParserManualX2 |  44.15 ms |   1.382 ms |  1.15 |  22.96 MB |        1.00 |
| SepManual            |  50.59 ms |   0.961 ms |  1.31 |  21.39 MB |        0.93 |
| SylvanDapperAuto     |  70.91 ms |   0.732 ms |  1.84 |  61.75 MB |        2.69 |
| RecordParserManual   |  71.60 ms |   1.388 ms |  1.86 |  22.94 MB |        1.00 |
| SoftCircuitsAuto     | 116.21 ms |   1.177 ms |  3.02 | 106.23 MB |        4.63 |
| CsvHelperManual      | 117.08 ms |   2.115 ms |  3.05 |  63.32 MB |        2.76 |
| CsvHelperAuto        | 119.05 ms |   5.555 ms |  3.11 |  63.32 MB |        2.76 |
| CesilAuto            | 141.47 ms |   2.529 ms |  3.68 |  23.14 MB |        1.01 |
| TinyCsvManual        | 215.01 ms | 138.455 ms |  5.44 | 229.51 MB |       10.01 |

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

| Method               | Mean      | Error     | StdDev    | Gen0        | Gen1    | Allocated |
|--------------------- |----------:|----------:|----------:|------------:|--------:|----------:|
| Sep                  |  12.64 ms |  0.303 ms |  0.108 ms |   8562.5000 |       - |  34.21 MB |
| Sylvan               |  15.47 ms |  0.238 ms |  0.062 ms |   8953.1250 |       - |  35.72 MB |
| SoftCircuits         |  18.97 ms |  1.406 ms |  0.501 ms |   8937.5000 |       - |  35.72 MB |
| NaiveBroken          |  19.88 ms |  1.873 ms |  0.668 ms |  15343.7500 |       - |  61.24 MB |
| Fluent               |  21.66 ms |  0.444 ms |  0.158 ms |  15343.7500 |       - |  61.24 MB |
| CursivelyCsv         |  26.34 ms |  0.372 ms |  0.097 ms |   8937.5000 |       - |  35.71 MB |
| CtlData              |  29.11 ms |  0.844 ms |  0.301 ms |  20468.7500 |       - |  81.84 MB |
| NReco                |  34.49 ms |  0.510 ms |  0.079 ms |   8933.3333 |       - |  35.86 MB |
| FastCsvParser        |  36.66 ms |  1.466 ms |  0.523 ms |   8933.3333 | 66.6667 |  36.05 MB |
| MgholamFastCSV       |  37.15 ms |  1.051 ms |  0.375 ms |   9285.7143 |       - |  37.41 MB |
| NLightCsv            |  43.45 ms |  2.018 ms |  0.720 ms |   8916.6667 |       - |  35.84 MB |
| Lumenworks           |  53.58 ms |  3.025 ms |  1.079 ms |  41700.0000 |       - | 166.79 MB |
| CsvHelper            |  53.63 ms |  1.063 ms |  0.276 ms |   8900.0000 |       - |  35.85 MB |
| NotVBTextFieldParser |  54.12 ms |  1.831 ms |  0.653 ms |  33300.0000 |       - |    133 MB |
| FSharpData           |  70.75 ms |  6.882 ms |  2.454 ms |  60000.0000 |       - | 239.31 MB |
| FlatFilesCsv         | 132.54 ms |  3.923 ms |  1.399 ms |  34750.0000 |       - | 139.52 MB |
| CsvBySteve           | 179.72 ms | 14.072 ms |  5.018 ms |  83666.6667 |       - | 334.26 MB |
| VisualBasic          | 448.76 ms | 37.005 ms | 13.196 ms | 289000.0000 |       - | 1153.4 MB |

## CsvSum

This benchmark measures summing the values of a single column (`Total Profit`) from the CSV file.
It is meant to highlight an optimization employed by some libraries where you only "pay" for the
columns that you access.

| Method         | Mean      | Error     | Allocated    |
|--------------- |----------:|----------:|-------------:|
| SylvanData     |  8.911 ms | 0.3933 ms |      8.79 KB |
| SepCsv         |  9.020 ms | 0.9497 ms |      5.86 KB |
| RecordParserX4 |  9.527 ms | 0.8254 ms |    114.37 KB |
| RecordParser   | 21.709 ms | 0.2744 ms |     14.43 KB |
| NReco          | 26.815 ms | 0.3454 ms |   2677.17 KB |
| CsvHelper      | 37.195 ms | 0.4231 ms |   2671.88 KB |
| Lumenworks     | 47.992 ms | 5.3740 ms | 136754.41 KB |