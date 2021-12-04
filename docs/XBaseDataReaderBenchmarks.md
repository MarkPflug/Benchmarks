# XBase Reader Benchmarks

XBase is a very old data format used by dBase, FoxPro, Clipper, and other old database systems.
Notably, it is currently still used by ESRI ShapeFiles to store metadata about shapes.
This benchmark measures reading the contents of a [large shape file](https://prd-tnm.s3.amazonaws.com/StagedProducts/GovtUnit/Shape/GOVTUNIT_Oregon_State_Shape.zip)

|  Method |      Mean |     Error |   StdDev |      Gen 0 | Allocated |
|-------- |----------:|----------:|---------:|-----------:|----------:|
|  Sylvan |  88.62 ms |  3.779 ms | 1.348 ms | 24333.3333 |     98 MB |
|    NDbf | 213.55 ms |  4.459 ms | 1.158 ms | 25333.3333 |    101 MB |
| DbfData | 291.12 ms | 24.683 ms | 8.802 ms | 83000.0000 |    333 MB |