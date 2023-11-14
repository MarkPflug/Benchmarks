# XBase Reader Benchmarks

XBase is a very old data format used by dBase, FoxPro, Clipper, and other old database systems.
Notably, it is currently still used by ESRI ShapeFiles to store metadata about shapes.
This benchmark measures reading the contents of a [large shape file](https://prd-tnm.s3.amazonaws.com/StagedProducts/GovtUnit/Shape/GOVTUNIT_Oregon_State_Shape.zip)

| Method       | Mean     | Error     | Ratio | Allocated  | Alloc Ratio |
|------------- |---------:|----------:|------:|-----------:|------------:|
| Sylvan       | 2.204 ms | 0.0474 ms |  1.00 |  753.12 KB |        1.00 |
| SylvanAsync  | 2.648 ms | 0.0479 ms |  1.20 |  1291.6 KB |        1.71 |
| SylvanPooled | 3.046 ms | 0.0800 ms |  1.38 |  696.85 KB |        0.93 |
| DbfData      | 8.228 ms | 0.2195 ms |  3.73 | 5955.14 KB |        7.91 |