using BenchmarkDotNet.Attributes;
using MiniExcelLibs;
using System.IO;

namespace Benchmarks;

[MemoryDiagnoser]
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
	public void MiniExcelXlsx()
	{
		using var s = File.OpenRead(file);
		foreach(var rec in s.Query<SalesRecord>())
		{
		}
	}
}
