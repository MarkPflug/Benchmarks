using BenchmarkDotNet.Attributes;
using Cesil;
using CsvHelper.Configuration;
using Dapper;
using nietras.SeparatedValues;
using RecordParser.Builders.Reader;
using RecordParser.Extensions;
using RecordParser.Parsers;
using System;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace Benchmarks;

[MemoryDiagnoser]
// hide some less interesting columns.
[HideColumns("StdDev", "RatioSD", "Gen0", "Gen1", "Gen2")]
public class CsvDataBinderBenchmarks
{
	public CsvDataBinderBenchmarks()
	{
		Dapper.SqlMapper.SetTypeMap(typeof(SalesRecord), new SalesRecordMap());
	}

	[Benchmark]
	public void CsvHelperAuto()
	{
		var tr = TestData.GetTextReader();
		var csv = new CsvHelper.CsvReader(tr, new CsvConfiguration(CultureInfo.InvariantCulture));
		var data = csv.GetRecords<SalesRecord>();

		foreach (var record in data)
		{
		}
	}

	[Benchmark]
	public void CsvHelperManual()
	{
		var tr = TestData.GetTextReader();
		var csv = new CsvHelper.CsvReader(tr, new CsvConfiguration(CultureInfo.InvariantCulture));
		var data = csv.GetRecords<SalesRecord>();

		foreach (var record in data)
		{
		}
	}

	[Benchmark]
	public void FlameCsvBinder()
	{
		var tr = TestData.GetTextReader();
		var data = FlameCsv.CsvReader.Read<SalesRecord>(tr);
		foreach (var record in data)
		{
		}
	}

	[Benchmark]
	public void CesilAuto()
	{
		var tr = TestData.GetTextReader();
		var data = CesilUtils.Enumerate<SalesRecord>(tr, Options.Default);
		foreach (var record in data)
		{
		}
	}

	[Benchmark]
	public void TinyCsvManual()
	{
		var csvP = new CsvParser<SalesRecord>(new CsvParserOptions(true, ','), new SalesRecordMapping());
		var dr = csvP.ReadFromString(new CsvReaderOptions(new[] { "\r\n", "\n", "\r" }), TestData.CachedData);
		foreach (var record in dr)
		{
		}
	}

	static IVariableLengthReader<SalesRecord> BuildReader()
	{
		var builder = new VariableLengthReaderSequentialBuilder<SalesRecord>()
			.Map(x => x.Region)
			.Map(x => x.Country)
			.Map(x => x.ItemType)
			.Map(x => x.SalesChannel)
			.Map(x => x.OrderPriority)
			.Map(x => x.OrderDate)
			.Map(x => x.OrderId)
			.Map(x => x.ShipDate)
			.Map(x => x.UnitsSold)
			.Map(x => x.UnitPrice)
			.Map(x => x.UnitCost)
			.Map(x => x.TotalRevenue)
			.Map(x => x.TotalCost)
			.Map(x => x.TotalProfit);

		return builder.Build(",", CultureInfo.InvariantCulture);
	}

	[Benchmark]
	public void RecordParserManual()
	{
		var tr = TestData.GetTextReader();
		var parser = BuildReader();
		var options = new VariableLengthReaderOptions
		{
			HasHeader = true,
			ParallelismOptions = new()
			{
				Enabled = false,
			}
		};

		var records = tr.ReadRecords(parser, options);
		foreach (var record in records)
		{
		}
	}

	[Benchmark]
	public void RecordParserManualX2()
	{
		RecordParserParallel(2);
	}

	[Benchmark]
	public void RecordParserManualX4()
	{
		// at least on my machine, there doesn't seem to be any benefit beyond 4x.
		RecordParserParallel(4);
	}

	void RecordParserParallel(int dop)
	{
		var tr = TestData.GetTextReader();
		var parser = BuildReader();
		var options = new VariableLengthReaderOptions
		{
			HasHeader = true,
			ParallelismOptions = new()
			{
				Enabled = true,
				MaxDegreeOfParallelism = dop,
			}
		};

		var records = tr.ReadRecords(parser, options);
		foreach (var record in records)
		{
		}
	}

	SalesRecord SepBind(SepReader.Row row)
	{
		return new SalesRecord
		{
			Region = row[0].ToString(),
			Country = row[1].ToString(),
			ItemType = row[2].ToString(),
			SalesChannel = row[3].ToString(),
			OrderPriority = row[4].ToString(),
			OrderDate = row[5].Parse<DateTime>(),
			OrderId = row[6].Parse<int>(),
			ShipDate = row[7].Parse<DateTime>(),
			UnitsSold = row[8].Parse<int>(),
			UnitPrice = row[9].Parse<decimal>(),
			UnitCost = row[10].Parse<decimal>(),
			TotalRevenue = row[11].Parse<decimal>(),
			TotalCost = row[12].Parse<decimal>(),
			TotalProfit = row[13].Parse<decimal>()
		};
	}

	[Benchmark]
	public void SepManual()
	{
		var tr = TestData.GetTextReader();
		using var reader = nietras.SeparatedValues.Sep.Reader().From(tr);
		foreach (var row in reader)
		{
			var rec = SepBind(row);
		}
	}

	[Benchmark]
	public void SylvanAuto()
	{
		var tr = TestData.GetTextReader();
		var dr = Sylvan.Data.Csv.CsvDataReader.Create(tr);
		foreach (var record in dr.GetRecords<SalesRecord>())
		{
		}
	}

	[Benchmark]
	public async Task SylvanAutoAsync()
	{
		var tr = TestData.GetTextReader();
		var dr = Sylvan.Data.Csv.CsvDataReader.Create(tr);
		await foreach (var record in dr.GetRecordsAsync<SalesRecord>())
		{
		}
	}

	[Benchmark(Baseline = true)]
	public void SylvanManual()
	{
		var tr = TestData.GetTextReader();
		var dr = Sylvan.Data.Csv.CsvDataReader.Create(tr);
		var binder = new ManualBinder();
		while (dr.Read())
		{
			var record = binder.Bind(dr);
		}
	}

	Func<DbDataReader, SalesRecord> dapperBinder = null;

	[Benchmark]
	public void SylvanDapperAuto()
	{
		var tr = TestData.GetTextReader();
		var dr = Sylvan.Data.Csv.CsvDataReader.Create(tr);
		if (dapperBinder == null)
			dapperBinder = dr.GetRowParser<SalesRecord>();
		var binder = dapperBinder;
		while (dr.Read())
		{
			var record = binder(dr);
		}
	}

	[Benchmark]
	public void SoftCircuitsAuto()
	{
		var stream = TestData.GetUtf8Stream();
		using (var reader = new SoftCircuits.CsvParser.CsvReader<SalesRecord>(stream, Encoding.UTF8))
		{
			reader.ReadHeaders(true);
			SalesRecord r;
			while ((r = reader.Read()) != null)
			{
			}
		}
	}
}

#region Dapper support

sealed class SalesRecordMap : Dapper.SqlMapper.ITypeMap
{
	public ConstructorInfo FindConstructor(string[] names, Type[] types)
	{
		return null;
	}

	public ConstructorInfo FindExplicitConstructor()
	{
		return typeof(SalesRecord).GetConstructor(Array.Empty<Type>());
	}

	public Dapper.SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
	{
		return null;
	}

	public Dapper.SqlMapper.IMemberMap GetMember(string columnName)
	{
		var propName = columnName.Replace(" ", "");
		var sr = typeof(SalesRecord);
		var prop = sr.GetProperties().Single(p => StringComparer.OrdinalIgnoreCase.Equals(p.Name, propName));
		if (prop == null) throw new Exception();
		return new PropMap(columnName, prop);
	}

	sealed class PropMap : Dapper.SqlMapper.IMemberMap
	{
		public PropMap(string colName, PropertyInfo prop)
		{
			this.ColumnName = colName;
			this.Property = prop;
			this.MemberType = prop.PropertyType;
		}
		public string ColumnName { get; }

		public Type MemberType { get; }

		public PropertyInfo Property { get; }

		public FieldInfo Field => null;

		public ParameterInfo Parameter => null;
	}
}

#endregion

#region TinyCsv support

sealed class SalesRecordMapping : CsvMapping<SalesRecord>
{
	public SalesRecordMapping()
	{
		this.MapProperty(0, r => r.Region);
		this.MapProperty(1, r => r.Country);
		this.MapProperty(2, r => r.ItemType);
		this.MapProperty(3, r => r.SalesChannel);
		this.MapProperty(4, r => r.OrderPriority);
		this.MapProperty(5, r => r.OrderDate);
		this.MapProperty(6, r => r.OrderId);
		this.MapProperty(7, r => r.ShipDate);
		this.MapProperty(8, r => r.UnitsSold);
		this.MapProperty(9, r => r.UnitPrice);
		this.MapProperty(10, r => r.UnitCost);
		this.MapProperty(11, r => r.TotalRevenue);
		this.MapProperty(12, r => r.TotalCost);
		this.MapProperty(13, r => r.TotalProfit);
	}
}

#endregion

// manually binds a SalesRecord. Used as a baseline for comparison.
sealed class ManualBinder
{
	public SalesRecord Bind(DbDataReader dr)
	{
		return new SalesRecord
		{
			Region = dr.GetString(0),
			Country = dr.GetString(1),
			ItemType = dr.GetString(2),
			SalesChannel = dr.GetString(3),
			OrderPriority = dr.GetString(4),
			OrderDate = dr.GetDateTime(5),
			OrderId = dr.GetInt32(6),
			ShipDate = dr.GetDateTime(7),
			UnitsSold = dr.GetInt32(8),
			UnitPrice = dr.GetDecimal(9),
			UnitCost = dr.GetDecimal(10),
			TotalRevenue = dr.GetDecimal(11),
			TotalCost = dr.GetDecimal(12),
			TotalProfit = dr.GetDecimal(13)
		};
	}
}
