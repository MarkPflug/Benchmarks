# XBase Reader Benchmarks

XBase is a very old data format used by dBase, FoxPro, Clipper, and other old database systems.
Notably, it is currently still used by ESRI ShapeFiles to store metadata about shapes.
This benchmark measures reading the contents of a [large shape file](https://prd-tnm.s3.amazonaws.com/StagedProducts/GovtUnit/Shape/GOVTUNIT_Oregon_State_Shape.zip)

|       Method |      Mean |    Error |   StdDev |       Gen0 |    Allocated |
|------------- |----------:|---------:|---------:|-----------:|-------------:|
|       Sylvan |  87.28 ms | 1.139 ms | 0.062 ms | 15500.0000 |  63909.75 KB |
| SylvanPooled | 109.82 ms | 5.670 ms | 2.022 ms |          - |      9.42 KB |
|         NDbf | 198.86 ms | 6.319 ms | 2.253 ms | 25333.3333 | 103883.23 KB |
|      DbfData | 287.16 ms | 8.176 ms | 2.123 ms | 83500.0000 | 341377.63 KB |