# XBase Reader Benchmarks

XBase is a very old data format used by dBase, FoxPro, Clipper, and other old database systems.
Notably, it is currently still used by ESRI ShapeFiles to store metadata about shapes.
This benchmark measures reading the contents of a [large shape file](https://prd-tnm.s3.amazonaws.com/StagedProducts/GovtUnit/Shape/GOVTUNIT_Oregon_State_Shape.zip)

| Method       | Mean     | Error     | Ratio | Allocated  | Alloc Ratio |
|------------- |---------:|----------:|------:|-----------:|------------:|
| Sylvan       | 1.086 ms | 0.0231 ms |  1.00 |  752.83 KB |        1.00 |
| SylvanAsync  | 1.298 ms | 0.0186 ms |  1.20 | 1291.17 KB |        1.72 |
| SylvanPooled | 1.305 ms | 0.0212 ms |  1.20 |  696.75 KB |        0.93 |
| DbfData      | 3.236 ms | 0.0588 ms |  2.98 | 5954.57 KB |        7.91 |