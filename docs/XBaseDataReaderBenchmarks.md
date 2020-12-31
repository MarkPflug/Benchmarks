# XBase Reader Benchmarks

XBase is a very old data format used by dBase, FoxPro, Clipper, and other old database systems.
Notably, it is currently still used by ESRI ShapeFiles to store metadata about shapes.


|  Method |     Mean |     Error |    StdDev | Ratio | RatioSD |     Gen 0 | Gen 1 | Gen 2 |  Allocated |
|-------- |---------:|----------:|----------:|------:|--------:|----------:|------:|------:|-----------:|
|    NDbf | 3.050 ms | 0.0253 ms | 0.0225 ms |  1.00 |    0.00 |  320.3125 |     - |     - | 1309.18 KB |
| DbfData | 5.423 ms | 0.0871 ms | 0.0815 ms |  1.78 |    0.03 | 1031.2500 |     - |     - | 4227.94 KB |
|  Sylvan | 1.353 ms | 0.0096 ms | 0.0090 ms |  0.44 |    0.00 |  183.5938 |     - |     - |  750.44 KB |

### [NDbfDataReader](https://github.com/eXavera/NDbfReader)

This is the most-downloaded `.dbf` nuget package, so used as the baseline for benchmarks.
Doesn't implement IDataReader, and so requires adapting to .NET standard data access APIs.
Implements only a minimal set of field types, and doesn't support any memo data.

### [DbfDataReader](https://github.com/yellowfeather/DbfDataReader)

Implements I/DbDataReader.
Supports a variety of memo formats.
Incorrectly exposes currency and other numeric values as `float`, when `decimal` should be used.

### [Sylvan.Data.XBase](https://github.com/MarkPflug/Sylvan)

Implements DbDataReader, supports FoxPro-style memo data.
Supports FoxPro null flags, double type, and varchar/binary fields.
Does __not__ support all memo formats.
Pre-release, probably lots of bugs yet to discover.