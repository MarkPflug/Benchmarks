using BenchmarkDotNet.Attributes;
using nietras.SeparatedValues;
using RecordParser.Builders.Reader;
using RecordParser.Extensions;
using System.Data;
using System.Globalization;

namespace Benchmarks;

[MemoryDiagnoser]
[HideColumns("StdDev", "RatioSD", "Gen0", "Gen1", "Gen2")]
public class CsvSum
{
	const int BufferSize = 0x8000;

	// Any test using this is cheating...
	const int TotalProfitColumnIdx = 13;

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
			.Map(x => x, indexColumn: TotalProfitColumnIdx)
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
	public decimal FlameCsvSumByIndex()
	{
		using var t = TestData.GetTextReader();
		var data = FlameCsv.CsvReader.Enumerate(t);
		var a = 0m;
		// I can't figure out how to get the index from the column name
		// so this benchmark currently "cheats" with the hard-coded index
		foreach (var record in data)
		{
			a += record.ParseField<decimal>(TotalProfitColumnIdx);
		}
		return a;
	}

	[Benchmark]
	public decimal FlameCsvSumByName()
	{
		using var t = TestData.GetTextReader();
		var data = FlameCsv.CsvReader.Enumerate(t);
		var a = 0m;
		
		foreach (var record in data)
		{
			// using the name here appears to slow things down quite a bit
			// but this benchmark doesn't "cheat".
			a += record.ParseField<decimal>("Total Profit");
		}
		return a;
	}

	[Benchmark]
	public decimal SepCsv()
	{
		using var t = TestData.GetTextReader();
		using var data = Sep.Reader().From(t);
		var a = 0m;
		foreach (var record in data)
		{
			var col = record["Total Profit"];
			a += col.Parse<decimal>();
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
