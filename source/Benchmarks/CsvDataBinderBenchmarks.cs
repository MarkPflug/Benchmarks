using Ben.Collections.Specialized;
using BenchmarkDotNet.Attributes;
using Cesil;
using CsvHelper.Configuration;
using Dapper;
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
public class CsvDataBinderBenchmarks
{
	Sylvan.StringPool pool;

	public CsvDataBinderBenchmarks()
	{

		Dapper.SqlMapper.SetTypeMap(typeof(SalesRecord), new SalesRecordMap());
		pool = new Sylvan.StringPool(64);
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
	public void CsvHelperAutoPooled()
	{
		var tr = TestData.GetTextReader();
		var config = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			CacheFields = true,
		};
		var csv = new CsvHelper.CsvReader(tr, config);
		var data = csv.GetRecords<SalesRecord>();

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

	static IVariableLengthReader<SalesRecord> BuildReader(bool pooled)
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

		if (pooled)
			builder.DefaultTypeConvert(new InternPool().Intern);

		return builder.Build(",", CultureInfo.InvariantCulture);
	}

	[Benchmark]
	public void RecordParser()
	{
		RecordParserImpl(false);
	}

	[Benchmark]
	public void RecordParserPooled()
	{
		RecordParserImpl(true);
	}

	void RecordParserImpl(bool pooled)
	{
		var tr = TestData.GetTextReader();
		var parser = BuildReader(pooled);
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
	public void RecordParserParallelX4()
	{
		var tr = TestData.GetTextReader();
		var parser = BuildReader(pooled: false);
		var options = new VariableLengthReaderOptions
		{
			HasHeader = true,
			ParallelismOptions = new()
			{
				Enabled = true,
				MaxDegreeOfParallelism = 4,
			}
		};

		var records = tr.ReadRecords(parser, options);
		foreach (var record in records)
		{
		}
	}

	[Benchmark]
	public void SylvanData()
	{
		var tr = TestData.GetTextReader();
		var dr = Sylvan.Data.Csv.CsvDataReader.Create(tr);
		foreach (var record in dr.GetRecords<SalesRecord>())
		{
		}
	}

	[Benchmark]
	public async Task SylvanDataAsync()
	{
		var tr = TestData.GetTextReader();
		var dr = Sylvan.Data.Csv.CsvDataReader.Create(tr);
		await foreach (var record in dr.GetRecordsAsync<SalesRecord>())
		{
		}
	}

	[Benchmark]
	public void SylvanDataPooled()
	{
		var tr = TestData.GetTextReader();
		var opts = new Sylvan.Data.Csv.CsvDataReaderOptions { StringFactory = pool.GetString };
		var dr = Sylvan.Data.Csv.CsvDataReader.Create(tr, opts);
		foreach (var record in dr.GetRecords<SalesRecord>())
		{
		}
	}

	[Benchmark]
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

	[Benchmark]
	public void SylvanDapper()
	{
		var tr = TestData.GetTextReader();
		var dr = Sylvan.Data.Csv.CsvDataReader.Create(tr);
		var parser = dr.GetRowParser<SalesRecord>();
		while (dr.Read())
		{
			var record = parser(dr);
		}
	}

	[Benchmark]
	public void SoftCircuits()
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

#region CSVHelper support

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
