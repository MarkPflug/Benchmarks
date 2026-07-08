using BenchmarkDotNet.Attributes;
using MiniExcelLibs;
using System.IO;
using Ganss.Excel;
using Npoi.Mapper;

namespace Benchmarks;

[MemoryDiagnoser]
[HideColumns("StdDev", "RatioSD", "Gen0", "Gen1", "Gen2")]
public class ExcelBinderBenchmarks
{
	public const string file = @"Data/65K_Records_Data.xlsx";

	[Benchmark]
	public void SylvanXlsx()
	{
		var reader = Sylvan.Data.Excel.ExcelDataReader.Create(file);
		foreach(var rec in reader.GetRecords<SalesRecord>())
		{
		}
	}
	
	[Benchmark]
	public void ExcelReaderNetXlsx()
	{
		using var reader = ExcelReader.Core.Reader.Excel.FromFile(file);
		var parser = new ExcelReader.Core.Parser.ExcelParser<SalesRecord>();
		foreach (var rec in parser.Parse(reader))
		{
		}
	}

	[Benchmark]
	public void MiniExcelXlsx()
	{
		using var s = File.OpenRead(file);
		foreach(var rec in s.Query<SalesRecord>())
		{
		}
	}

	[Benchmark(Description =  "ExcelMapperXlsx (based on NPOI)")]
	public void ExcelMapperXlsx()
	{
		var excel = new ExcelMapper(file);
		var salesRecords = excel.Fetch<SalesRecord>();
		foreach(var rec in salesRecords)
		{
		}
	}
	[Benchmark(Description =  "NPOI.MapperXlsx (based on NPOI)")]
	public void NPOIMapperXlsx()
	{
		var mapper = new Mapper(file);
		var salesRecords = mapper.Take<SalesRecord>();
		foreach(var rec in salesRecords)
		{
		}
	}
}
