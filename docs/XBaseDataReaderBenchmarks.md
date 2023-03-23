# XBase Reader Benchmarks

XBase is a very old data format used by dBase, FoxPro, Clipper, and other old database systems.
Notably, it is currently still used by ESRI ShapeFiles to store metadata about shapes.
This benchmark measures reading the contents of a [large shape file](https://prd-tnm.s3.amazonaws.com/StagedProducts/GovtUnit/Shape/GOVTUNIT_Oregon_State_Shape.zip)

|       Method |     Mean |   Error |  StdDev |       Gen0 |      Gen1 |     Gen2 | Allocated |
|------------- |---------:|--------:|--------:|-----------:|----------:|---------:|----------:|
|       Sylvan | 160.1 ms | 1.50 ms | 0.08 ms | 21000.0000 |         - |        - |  83.85 MB |
|  SylvanAsync | 168.9 ms | 3.18 ms | 0.83 ms | 22666.6667 |         - |        - |  90.84 MB |
|         NDbf | 178.4 ms | 3.67 ms | 0.95 ms | 21666.6667 |         - |        - |  87.73 MB |
| SylvanPooled | 225.3 ms | 0.65 ms | 0.10 ms |  2666.6667 | 1333.3333 | 333.3333 |   29.5 MB |
|      DbfData | 367.7 ms | 5.09 ms | 0.79 ms | 76000.0000 |         - |        - | 305.29 MB |