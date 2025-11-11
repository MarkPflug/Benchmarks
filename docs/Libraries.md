## Libraries Tested

I've created benchmarks that cover many of the most common or highest performance .NET libraries.
These are primarily third-party open source libraries, 
but also includes some implementations from the BCL such as the Visual Basic TextFieldParser.

All libraries are using the latest version as of 2025-11-11.

- Naive, Broken

These measure the naive approach of using `TextReader.ReadLine` and `String.Split` to process CSV data. 
Likewise, writing is performed by writing commas and newlines, but ignoring escaping.
These approaches are fast, but don't handle the edge cases of quoted fields, embedded commas, etc; and thus are not [RFC 4180](https://tools.ietf.org/html/rfc4180) compliant.

## CSV Libraries

- [Cesil](https://github.com/kevin-montrose/Cesil)
- [CsvBySteve](https://github.com/stevehansen/csv/)
- [CsvHelper](https://github.com/JoshClose/CsvHelper)
- [Ctl.Data](https://github.com/ctl-global/ctl-data/)
- [Cursively](https://github.com/airbreather/Cursively)
- [FastCsvParser](https://github.com/bopohaa/CsvParser)
- [FlatFilesCsv](https://github.com/jehugaleahsa/FlatFiles)
- [FluentCSV](https://github.com/aboudoux/FluentCSV)
- [FSharp.Data](https://github.com/fsharp/FSharp.Data)
- [Lumenworks](https://www.codeproject.com/Articles/9258/A-Fast-CSV-Reader) now maintained as [LumenWorksCsvReader](https://github.com/phatcher/CsvReader).
- [mhgolam.fastCSV](https://github.com/mgholam/fastCSV)
- [NLight](https://github.com/slorion/nlight)
- [NReco.Csv](https://github.com/nreco/csv)
- [OleDbCsv](https://www.microsoft.com/en-us/download/details.aspx?id=54920)
- [RecordParser](https://github.com/leandromoh/RecordParser)
- [Sylvan](https://github.com/MarkPflug/Sylvan/blob/master/docs/csv/Sylvan.Data.Csv.md)
- [TinyCsvParser](https://github.com/bytefish/TinyCsvParser)
- [VisualBasic](https://github.com/dotnet/runtime/blob/master/src/libraries/Microsoft.VisualBasic.Core/src/Microsoft/VisualBasic/FileIO/TextFieldParser.vb)


## Excel Libraries


## XBase/DBF Libraries

- [DbfDataReader](https://github.com/yellowfeather/DbfDataReader)
- [NDbfDataReader](https://github.com/eXavera/NDbfReader)
- [Sylvan.Data.XBase](https://github.com/MarkPflug/Sylvan)

