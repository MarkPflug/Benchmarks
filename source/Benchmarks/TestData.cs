using CsvHelper.Configuration.Attributes;
using Sylvan.Data;
using Sylvan.Data.Csv;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;

namespace Benchmarks
{
	public sealed class TestRecord
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public bool IsActive { get; set; }
		public double[] DataSet { get; set; }
	}

	public class SalesRecord
	{
		[Name("Region")]
		[DataMember(Name = "Region")]
		public string Region { get; set; }
		[Name("Country")]
		[DataMember(Name = "Country")]
		public string Country { get; set; }
		[Name("Item Type")]
		[DataMember(Name = "Item Type")]
		public string ItemType { get; set; }
		[Name("Sales Channel")]
		[DataMember(Name = "Sales Channel")]
		public string SalesChannel { get; set; }
		[Name("Order Priority")]
		[DataMember(Name = "Order Priority")]
		public string OrderPriority { get; set; }
		[Name("Order Date")]
		[DataMember(Name = "Order Date")]
		public DateTime OrderDate { get; set; }
		[Name("Order ID")]
		[DataMember(Name = "Order ID")]
		public int OrderId { get; set; }
		[Name("Ship Date")]
		[DataMember(Name = "Ship Date")]
		public DateTime ShipDate { get; set; }
		[Name("Units Sold")]
		[DataMember(Name = "Units Sold")]
		public int UnitsSold { get; set; }
		[Name("Unit Price")]
		[DataMember(Name = "Unit Price")]
		public decimal UnitPrice { get; set; }
		[Name("Unit Cost")]
		[DataMember(Name = "Unit Cost")]
		public decimal UnitCost { get; set; }
		[Name("Total Revenue")]
		[DataMember(Name = "Total Revenue")]
		public decimal TotalRevenue { get; set; }
		[Name("Total Cost")]
		[DataMember(Name = "Total Cost")]
		public decimal TotalCost { get; set; }
		[Name("Total Profit")]
		[DataMember(Name = "Total Profit")]
		public decimal TotalProfit { get; set; }
	}

	public class SalesRecordStrings
	{
		public string Region { get; set; }
		public string Country { get; set; }
		public string ItemType { get; set; }
		public string SalesChannel { get; set; }
		public string OrderPriority { get; set; }
		public string OrderDate { get; set; }
		public string OrderId { get; set; }
		public string ShipDate { get; set; }
		public string UnitsSold { get; set; }
		public string UnitPrice { get; set; }
		public string UnitCost { get; set; }
		public string TotalRevenue { get; set; }
		public string TotalCost { get; set; }
		public string TotalProfit { get; set; }
	}

	public static class TestData
	{
		const string DataSetUrl = "http://eforexcel.com/wp/wp-content/uploads/2017/07/1000000%20Sales%20Records.zip";
		const string ZipFileName = "SalesData.zip";
		const string DataFileName = "SalesData.csv";

		const string DataSetSchema = @"
Region,
Country,
Item Type,
Sales Channel,
Order Priority,
Order Date:DateTime,
Order ID:int,
Ship Date:DateTime,
Units Sold:int,
Unit Price:decimal,
Unit Cost:decimal,
Total Revenue:decimal,
Total Cost:decimal,
Total Profit:decimal
";
		static CsvSchema Schema = new CsvSchema(Sylvan.Data.Schema.Parse(DataSetSchema).GetColumnSchema());

		public static string CachedData;
		static byte[] CachedUtfData;

		static void CacheData()
		{
			if (!File.Exists(DataFileName))
			{
				using (var oStream = File.OpenWrite(ZipFileName))
				using (var iStream = new HttpClient().GetStreamAsync(DataSetUrl).Result)
				{
					iStream.CopyTo(oStream);
				}

				var s = File.OpenRead(ZipFileName);
				var a = new ZipArchive(s);
				using var ds = a.Entries.First().Open();
				using var os = File.Create(DataFileName);
				ds.CopyTo(os);
			}
			CachedData = File.ReadAllText(DataFileName);
			CachedUtfData = Encoding.UTF8.GetBytes(CachedData);
		}

		static TestData()
		{
			// is it a bad idea to do this in a static constructor?
			// probably, but this is only used in test/benchmarks.
			CacheData();
		}

		

		public static string DataFile
		{
			get
			{
				return DataFileName;
			}
		}

		public static TextReader GetTextReader()
		{
			return new StreamReader(GetUtf8Stream(), Encoding.UTF8);
		}

		public static Stream GetUtf8Stream()
		{
			return new MemoryStream(CachedUtfData);
		}

		public static ReadOnlyMemory<byte> GetUtf8Array()
		{
			return CachedUtfData;
		}

		public static DbDataReader GetData()
		{

			return CsvDataReader.Create(GetTextReader());
		}

		public static DbDataReader GetDataWithSchema(Action<CsvDataReaderOptions> opts = null)
		{
			
			var options = new CsvDataReaderOptions { 
				Schema = Schema 
			};
			opts?.Invoke(options);
			return CsvDataReader.Create(GetTextReader(), options);
		}

		public static DbDataReader GetTypedData()
		{
			var reader = File.OpenText("Data\\Schema.csv");
			return CsvDataReader.Create(reader, new CsvDataReaderOptions() { Schema = DataSchema.Instance });
		}

		public static ICsvSchemaProvider TestDataSchema => DataSchema.Instance;


		class DataSchema : ICsvSchemaProvider
		{
			public static DataSchema Instance = new DataSchema();
			Type[] types;
			bool[] nullable;

			private DataSchema()
			{
				Type i = typeof(int);
				Type s = typeof(string);
				Type d = typeof(DateTime);
				Type m = typeof(decimal);
				types = new Type[] { s, s, s, s, s, d, i, d, i, m, m, m, m, m };
			}

			public DbColumn GetColumn(string name, int ordinal)
			{
				return new TypedCsvColumn(types[ordinal], false);
			}
		}

		class TypedCsvColumn : DbColumn
		{
			public TypedCsvColumn(Type type, bool allowNull)
			{
				this.DataType = type;
				this.AllowDBNull = allowNull;
			}
		}

		static ObjectDataReader.Factory<TestRecord> Factory =
			ObjectDataReader
				.BuildFactory<TestRecord>()
				.AddColumn("Id", i => i.Id)
				.AddColumn("Name", i => i.Name)
				.AddColumn("Date", i => i.Date)
				.AddColumn("IsActive", i => i.IsActive)
				.Repeat((b, i) => b.AddColumn("Data" + i, r => r.DataSet[i]), 10)
				.Build();

		public static T Repeat<T>(this T obj, Func<T, int, T> a, int count)
		{
			var item = obj;

			for (int i = 0; i < count; i++)
			{
				item = a(item, i);
			}

			return item;
		}

		public static DbDataReader GetTestData(int count = 10)
		{
			return Factory.Create(GetTestObjects(count, 10));
		}

		public const int DefaultRecordCount = 100000;
		public const int DefaultDataValueCount = 20;

		public static IEnumerable<TestRecord> GetTestObjects(int recordCount = DefaultRecordCount, int valueCount = DefaultDataValueCount)
		{
			// We'll reuse the single instance of TestRecord. 
			// We do this so memory usage in benchmarks is a better indicator
			// of the library, and not just overwhelmed by TestRecord allocations.
			var row = new TestRecord();
			DateTime startDate = new DateTime(2020, 3, 23, 0, 0, 0, DateTimeKind.Utc);
			row.DataSet = new double[valueCount];
			var counter = 1;

			return
				Enumerable
				.Range(0, recordCount)
				.Select(
					i =>
					{
						row.Id = i;
						row.Name = "Model Number: 1337";
						row.Date = startDate.AddDays(i);
						row.IsActive = i % 2 == 1;
						for (int idx = 0; idx < row.DataSet.Length; idx++)
						{
							row.DataSet[idx] = .25 * counter++;
						}
						return row;
					}
				);
		}

		static ObjectDataReader.Factory<BinaryData> BinaryFactory =
			ObjectDataReader
				.BuildFactory<BinaryData>()
				.AddColumn("Id", d => d.Id)
				.AddColumn("Data", d => d.Data)
				.Build();

		public static DbDataReader GetBinaryData()
		{
			return BinaryFactory.Create(GetTestBinary());
		}

		public class BinaryData
		{
			public int Id { get; set; }
			public byte[] Data { get; set; }
		}

		public static IEnumerable<BinaryData> GetTestBinary()
		{
			yield return new BinaryData { Id = 1, Data = new byte[] { 1, 2, 3, 4, 5 } };
			yield return new BinaryData { Id = 2, Data = new byte[] { 5, 4, 3, 2, 1 } };

		}

		public static DbDataReader GetTestDataReader(int recordCount = DefaultRecordCount, int valueCount = DefaultDataValueCount)
		{
			var items = GetTestObjects(recordCount, valueCount);
			return
				ObjectDataReader
				.BuildFactory<TestRecord>()
				.AddColumn("Id", i => i.Id)
				.AddColumn("Name", i => i.Name)
				.AddColumn("Date", i => i.Date)
				.AddColumn("IsActive", i => i.IsActive)
				.Repeat((b, i) => b.AddColumn("Data" + i, r => r.DataSet[i]), valueCount)
				.Build()
				.Create(items);
		}
	}
}
