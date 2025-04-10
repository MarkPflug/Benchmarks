# XBase Reader Benchmarks

XBase is a very old data format used by dBase, FoxPro, Clipper, and other old database systems.
Notably, it is currently still used by ESRI ShapeFiles to store metadata about shapes.
This benchmark measures reading the contents of a [large shape file](https://prd-tnm.s3.amazonaws.com/StagedProducts/GovtUnit/Shape/GOVTUNIT_Oregon_State_Shape.zip)

| Method       | Mean     | Error     | Ratio | Allocated  | Alloc Ratio |
|------------- |---------:|----------:|------:|-----------:|------------:|
| Sylvan       | 2.105 ms | 0.0622 ms |  1.00 |  753.03 KB |        1.00 |
| SylvanAsync  | 2.541 ms | 0.0460 ms |  1.21 | 1291.51 KB |        1.72 |
| SylvanPooled | 2.886 ms | 0.0403 ms |  1.37 |  696.82 KB |        0.93 |
| DbfData      | 7.850 ms | 0.1084 ms |  3.73 | 5955.18 KB |        7.91 |