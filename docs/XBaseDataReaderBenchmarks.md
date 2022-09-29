# XBase Reader Benchmarks

XBase is a very old data format used by dBase, FoxPro, Clipper, and other old database systems.
Notably, it is currently still used by ESRI ShapeFiles to store metadata about shapes.
This benchmark measures reading the contents of a [large shape file](https://prd-tnm.s3.amazonaws.com/StagedProducts/GovtUnit/Shape/GOVTUNIT_Oregon_State_Shape.zip)

|       Method |      Mean |    Error |   StdDev |      Gen 0 | Gen 1 | Gen 2 |    Allocated |
|------------- |----------:|---------:|---------:|-----------:|------:|------:|-------------:|
|       Sylvan |  94.92 ms | 4.312 ms | 1.538 ms | 15500.0000 |     - |     - |  63902.84 KB |
| SylvanPooled | 114.56 ms | 1.524 ms | 0.543 ms |          - |     - |     - |      10.6 KB |
|         NDbf | 211.15 ms | 2.651 ms | 0.688 ms | 25333.3333 |     - |     - | 103884.76 KB |
|      DbfData | 301.05 ms | 9.615 ms | 3.429 ms | 83000.0000 |     - |     - | 341378.91 KB |