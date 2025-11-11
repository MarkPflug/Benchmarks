# XBase Reader Benchmarks

XBase is a very old data format used by dBase, FoxPro, Clipper, and other old database systems.
Notably, it is currently still used by ESRI ShapeFiles to store metadata about shapes.
This benchmark measures reading the contents of a [large shape file](https://prd-tnm.s3.amazonaws.com/StagedProducts/GovtUnit/Shape/GOVTUNIT_Oregon_State_Shape.zip)

| Method       | Mean     | Error     | Ratio | Allocated  | Alloc Ratio |
|------------- |---------:|----------:|------:|-----------:|------------:|
| Sylvan       | 2.032 ms | 0.3235 ms |  1.00 |  753.07 KB |        1.00 |
| SylvanAsync  | 2.306 ms | 0.0691 ms |  1.14 | 1291.57 KB |        1.72 |
| SylvanPooled | 2.649 ms | 0.0454 ms |  1.31 |  696.88 KB |        0.93 |
| DbfData      | 5.956 ms | 0.1823 ms |  2.94 | 5955.13 KB |        7.91 |