using BenchmarkDotNet.Attributes;
using Cesil;
using CsvHelper.Configuration;
using Dapper;
using Sylvan.Data;
using Sylvan.Data.Csv;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using static Dapper.SqlMapper;

namespace Benchmarks
{
	[MemoryDiagnoser]
	[SimpleJob(1, 2, 4, 1)]
	public class CsvDataBinderBenchmarks
	{

		Sylvan.StringPool pool;
		public CsvDataBinderBenchmarks()
		{
			pool = new Sylvan.StringPool(64);
		}

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

		[Benchmark(Baseline = true)]
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
		public void MgholamCsvManual()
		{
			var data = fastCSV.ReadStream<SalesRecord>(TestData.GetTextReader(), ',',
				(obj, cols) =>
				{
					obj.Region = cols[0];
					obj.Country = cols[1];
					obj.ItemType = cols[2];
					obj.SalesChannel = cols[3];
					obj.OrderPriority = cols[4];
					obj.OrderDate = DateTime.Parse(cols[5]);
					obj.OrderId = int.Parse(cols[6]);
					obj.ShipDate = DateTime.Parse(cols[7]);
					obj.UnitsSold = int.Parse(cols[8]);
					obj.UnitPrice = decimal.Parse(cols[9]);
					obj.UnitCost = decimal.Parse(cols[10]);
					obj.TotalRevenue = decimal.Parse(cols[11]);
					obj.TotalCost = decimal.Parse(cols[12]);
					obj.TotalProfit = decimal.Parse(cols[13]);
					return true;
				}
			);
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
		public void SylvanData()
		{
			var dr = (CsvDataReader)TestData.GetDataWithSchema();
			var binder = DataBinder.Create<SalesRecord>(dr);
			while (dr.Read())
			{
				var record = new SalesRecord();
				binder.Bind(dr, record);
			}
		}

		[Benchmark]
		public void SylvanDataPooled()
		{
			var dr = (CsvDataReader)TestData.GetDataWithSchema(o => o.StringFactory = pool.GetString);
			var binder = DataBinder.Create<SalesRecord>(dr);
			while (dr.Read())
			{
				var record = new SalesRecord();
				binder.Bind(dr, record);
			}
		}

		[Benchmark]
		public void SylvanManual()
		{
			var dr = (CsvDataReader)TestData.GetDataWithSchema();
			var binder = new ManualBinder();
			while (dr.Read())
			{
				var record = binder.Bind(dr);
			}
		}

		[Benchmark]
		public void SylvanDapper()
		{
			SqlMapper.SetTypeMap(typeof(SalesRecord), new SalesRecordMap());
			var dr = (CsvDataReader)TestData.GetDataWithSchema();
			var parser = dr.GetRowParser<SalesRecord>();
			while (dr.Read())
			{
				var record = parser(dr);
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
}
