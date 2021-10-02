# XBase Reader Benchmarks

XBase is a very old data format used by dBase, FoxPro, Clipper, and other old database systems.
Notably, it is currently still used by ESRI ShapeFiles to store metadata about shapes.

|  Method |     Mean |     Error |    StdDev |    Gen 0 | Allocated |
|-------- |---------:|----------:|----------:|---------:|----------:|
|  Sylvan | 1.140 ms | 0.0103 ms | 0.0006 ms | 203.1250 |    833 KB |
|    NDbf | 3.200 ms | 0.0565 ms | 0.0087 ms | 320.3125 |  1,309 KB |
| DbfData | 3.200 ms | 0.2706 ms | 0.0965 ms | 738.2813 |  3,021 KB |

### [NDbfDataReader](https://github.com/eXavera/NDbfReader)

This is the most-downloaded `.dbf` nuget package.
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