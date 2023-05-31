using BenchmarkDotNet.Attributes;
using nietras.SeparatedValues;
using System.Buffers;
using System.Data;
using System.Globalization;


namespace Benchmarks;

[MemoryDiagnoser]
public class CsvSum
{
	const int BufferSize = 0x8000;

	[Benchmark]
	public decimal SylvanData()
	{
		var buf = ArrayPool<char>.Shared.Rent(BufferSize);
		var tr = TestData.GetTextReader();
		
		var dr = Sylvan.Data.Csv.CsvDataReader.Create(tr, buf);
		var idx = dr.GetOrdinal("Total Profit");
		decimal a = 0m;
		while (dr.Read())
		{
			a += dr.GetDecimal(idx);
		}
		ArrayPool<char>.Shared.Return(buf);
		return a;
	}

	[Benchmark]
	public decimal SepCsv()
	{
		var t = TestData.GetTextReader();
		using var r = Sep.Reader().From(t);
		var a = 0m;
		
		foreach(var record in r)
		{
			var dec = record["Total Profit"].Parse<decimal>();
			a += dec;
		}
		return a;
	}

	[Benchmark]
	public decimal NReco()
	{
		var tr = TestData.GetTextReader();
		var dr = new NReco.Csv.CsvReader(tr);
		decimal a = 0;
		dr.BufferSize = BufferSize;
		dr.Read(); // strip the header row
		var idx = -1;
		for (int i = 0; i < dr.FieldsCount; i++)
		{
			if (dr[i] == "Total Profit")
			{
				idx = i;
				break;
			}
		}

		while (dr.Read())
		{
			a += decimal.Parse(dr[idx]);
		}
		return a;
	}

	[Benchmark]
	public decimal Lumenworks()
	{
		var tr = TestData.GetTextReader();
		IDataReader dr = new LumenWorks.Framework.IO.Csv.CsvReader(tr, true, BufferSize);
		decimal a = 0;
		var idx = dr.GetOrdinal("Total Profit");
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
		var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
		{
			BufferSize = BufferSize
		};

		var r = new CsvHelper.CsvParser(tr, config);
		r.Read();//headers

		var idx = -1;
		for(int i = 0; i< r.Count; i++)
		{
			if (r[i] == "Total Profit")
			{
				idx = i;
				break;
			}
		}
		decimal a = 0m;
		while (r.Read())
		{
			var dec = decimal.Parse(r[idx]);
			a += dec;
		}
		return a;
	}

	//[Benchmark]
	//public decimal HyperCsvBench()
	//{
	//	using var r = HyperCsv.CsvDataReader.Create(TestData.GetUtf8Stream());
	//	var a = 0m;
	//	var idx = r.GetOrdinal("Total Profit");
	//	while(r.Read())
	//	{
	//		a += r.GetDecimal(idx);
	//	}
	//	return a;
	//}
}
