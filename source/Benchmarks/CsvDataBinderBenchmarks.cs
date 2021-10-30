using BenchmarkDotNet.Attributes;
using Benchmarks.Support;
using Cesil;
using CsvHelper.Configuration;
using Dapper;
using RecordParser.Builders.Reader;
using Sylvan.Data;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using static Dapper.SqlMapper;

namespace Benchmarks
{
	[MemoryDiagnoser]
	public class CsvDataBinderBenchmarks
	{
		Sylvan.StringPool pool;
		public CsvDataBinderBenchmarks()
		{
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

		[Benchmark]
		public async Task RecordParserAsync()
		{
			var parser = new VariableLengthReaderBuilder<SalesRecord>()
				.Map(x => x.Region, 0)
				.Map(x => x.Country, 1)
				.Map(x => x.ItemType, 2)
				.Map(x => x.SalesChannel, 3)
				.Map(x => x.OrderPriority, 4)
				.Map(x => x.OrderDate, 5)
				.Map(x => x.OrderId, 6)
				.Map(x => x.ShipDate, 7)
				.Map(x => x.UnitsSold, 8)
				.Map(x => x.UnitPrice, 9)
				.Map(x => x.UnitCost, 10)
				.Map(x => x.TotalRevenue, 11)
				.Map(x => x.TotalCost, 12)
				.Map(x => x.TotalProfit, 13)
				.Build(",", CultureInfo.InvariantCulture);

			using var stream = TestData.GetUtf8Stream();
			var records = RecordParserSupport.ProcessFile(stream, parser.Parse);
			await foreach(var record in records) {

			}
		}

		[Benchmark]
		public void SylvanData()
		{
			var dr = TestData.GetDataWithSchema();
			foreach(var record in dr.GetRecords<SalesRecord>())
			{
			}
		}

		[Benchmark]
		public async Task SylvanDataAsync()
		{
			var dr = TestData.GetDataWithSchema();
			var binder = DataBinder.Create<SalesRecord>(dr);
			while (await dr.ReadAsync())
			{
				var record = new SalesRecord();
				binder.Bind(dr, record);
			}
		}

		[Benchmark]
		public void SylvanDataPooled()
		{
			var dr = TestData.GetDataWithSchema(o => o.StringFactory = pool.GetString);
			foreach(var record in dr.GetRecords<SalesRecord>())
			{
			}
		}

		[Benchmark]
		public void SylvanManual()
		{
			var dr = TestData.GetDataWithSchema();
			var binder = new ManualBinder();
			while (dr.Read())
			{
				var record = binder.Bind(dr);
			}
		}

		[Benchmark]
		public void SylvanDapper()
		{
			SetTypeMap(typeof(SalesRecord), new SalesRecordMap());
			var dr = TestData.GetDataWithSchema();
			var parser = dr.GetRowParser<SalesRecord>();
			while (dr.Read())
			{
				var record = parser(dr);
			}
		}
	}

	sealed class SalesRecordMap : ITypeMap
	{
		public ConstructorInfo FindConstructor(string[] names, Type[] types)
		{
			return null;
		}

		public ConstructorInfo FindExplicitConstructor()
		{
			return typeof(SalesRecord).GetConstructor(Array.Empty<Type>());
		}

		public IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
		{
			return null;
		}

		public IMemberMap GetMember(string columnName)
		{
			var propName = columnName.Replace(" ", "");
			var sr = typeof(SalesRecord);
			var prop = sr.GetProperties().Single(p => StringComparer.OrdinalIgnoreCase.Equals(p.Name, propName));
			if (prop == null) throw new Exception();
			return new PropMap(columnName, prop);
		}

		class PropMap : IMemberMap
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

	class SalesRecordMapping : CsvMapping<SalesRecord>
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

	// manually binds a SalesRecord. Used as a baseline for comparison.
	class ManualBinder
	{
		public SalesRecord Bind(IDataRecord dr)
		{
			var record = new SalesRecord();
			record.Region = dr.GetString(0);
			record.Country = dr.GetString(1);
			record.ItemType = dr.GetString(2);
			record.SalesChannel = dr.GetString(3);
			record.OrderPriority = dr.GetString(4);
			record.OrderDate = dr.GetDateTime(5);
			record.OrderId = dr.GetInt32(6);
			record.ShipDate = dr.GetDateTime(7);
			record.UnitsSold = dr.GetInt32(8);
			record.UnitPrice = dr.GetDecimal(9);
			record.UnitCost = dr.GetDecimal(10);
			record.TotalRevenue = dr.GetDecimal(11);
			record.TotalCost = dr.GetDecimal(12);
			record.TotalProfit = dr.GetDecimal(13);
			return record;
		}
	}
}
