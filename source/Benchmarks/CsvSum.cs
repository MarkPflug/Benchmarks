using BenchmarkDotNet.Attributes;
using System.Globalization;

namespace Benchmarks;

[MemoryDiagnoser]
class CsvSum
{
	const int BufferSize = 0x10000;

	[Benchmark]
	public decimal SylvanData()
	{
		var tr = TestData.GetTextReader();
		var o = new Sylvan.Data.Csv.CsvDataReaderOptions { BufferSize = BufferSize };
		var dr = Sylvan.Data.Csv.CsvDataReader.Create(tr, o);
		var idx = dr.GetOrdinal("Total Profit");
		decimal a = 0m;
		while (dr.Read())
		{
			a += dr.GetDecimal(idx);
		}
		return a;
	}

	[Benchmark]
	public decimal CsvHelper()
	{

		var tr = TestData.GetTextReader();
		var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture)
		{
			BufferSize = BufferSize
		};

		var r = new CsvHelper.CsvParser(tr, config);
		r.Read();//headers
		decimal a = 0m;
		while (r.Read())
		{
			var dec = decimal.Parse(r[13]);
			a += dec;
		}
		return a;
	}
}
