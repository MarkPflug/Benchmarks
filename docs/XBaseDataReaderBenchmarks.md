# XBase Reader Benchmarks

XBase is a very old data format used by dBase, FoxPro, Clipper, and other old database systems.
Notably, it is currently still used by ESRI ShapeFiles to store metadata about shapes.
This benchmark measures reading the contents of a [US Census data shape file](https://www2.census.gov/geo/tiger/GENZ2018/shp/cb_2018_us_county_20m.zip)

|  Method |     Mean |     Error |    StdDev |    Gen 0 | Allocated |
|-------- |---------:|----------:|----------:|---------:|----------:|
|  Sylvan | 1.151 ms | 0.0210 ms | 0.0032 ms | 203.1250 |    833 KB |
|    NDbf | 2.983 ms | 0.0458 ms | 0.0071 ms | 230.4688 |    957 KB |
| DbfData | 3.314 ms | 0.1765 ms | 0.0629 ms | 738.2813 |  3,021 KB |