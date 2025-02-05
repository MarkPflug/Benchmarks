using BenchmarkDotNet.Attributes;
using nietras.SeparatedValues;
using RecordParser.Builders.Reader;
using RecordParser.Extensions;
using System.Data;
using System.Globalization;
using System.Text;

namespace Benchmarks;

[MemoryDiagnoser]
[HideColumns("StdDev", "RatioSD", "Gen0", "Gen1", "Gen2")]
public class CsvSum
{
	const int BufferSize = 0x8000;

	readonly char[] buffer = new char[BufferSize];

	[Benchmark(Baseline = true)]
	public decimal SylvanData()
	{
		using var tr = TestData.GetTextReader();
		using var dr = Sylvan.Data.Csv.CsvDataReader.Create(tr, buffer);
		var idx = dr.GetOrdinal("Total Profit");
		decimal a = 0m;
		while (dr.Read())
		{
			a += dr.GetDecimal(idx);
		}
		return a;
	}

	[Benchmark]
	public decimal RecordParserX4()
	{
		return RecordParser(4);
	}
	
	[Benchmark]
	public decimal RecordParser()
	{
		return RecordParser(1);
	}

	decimal RecordParser(int dop)
	{
		var parser = new VariableLengthReaderBuilder<decimal>()
			.Map(x => x, indexColumn: 13)
			.Build(",", CultureInfo.InvariantCulture);

		var options = new VariableLengthReaderOptions
		{
			HasHeader = true,
			ContainsQuotedFields = false,
			ParallelismOptions = new ()
			{
				Enabled = dop > 1,
				MaxDegreeOfParallelism = dop,
				EnsureOriginalOrdering = false
			}
		};
		
		var a = 0m;
		using var tr = TestData.GetTextReader();
		foreach (var profit in tr.ReadRecords(parser, options))
		{
			a += profit;
		}

		return a;
	}

	[Benchmark]
	public decimal SepCsv()
	{
		using var t = TestData.GetTextReader();
		using var r = Sep.Reader().From(t);
		var a = 0m;
		foreach (var record in r)
		{
			var span = record["Total Profit"];
			a += span.Parse<decimal>();
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
		for (int i = 0; i < r.Count; i++)
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
}
