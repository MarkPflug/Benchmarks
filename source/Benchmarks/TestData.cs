using CsvHelper.Configuration.Attributes;
using Sylvan;
using Sylvan.Data;
using Sylvan.Data.Csv;
using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Benchmarks;

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

public static class TestData
{
	const string DataFileName = "Data/65K_Records_Data.csv";

	public static string CachedData;
	static byte[] CachedUtfData;
	static SalesRecord[] salesRecords;
	static Schema salesRecordsSchema;

	static TestData()
	{
		CachedData = File.ReadAllText(DataFileName);
		CachedUtfData = Encoding.UTF8.GetBytes(CachedData);

		salesRecordsSchema =
			new Schema.Builder()
			.Add<string>()
			.Add<string>()
			.Add<string>()
			.Add<string>()
			.Add<string>()
			.Add<DateTime>()
			.Add<int>()
			.Add<DateTime>()
			.Add<int>()
			.Add<decimal>()
			.Add<decimal>()
			.Add<decimal>()
			.Add<decimal>()
			.Add<decimal>()
			.Build();

		var sp = new StringPool();
		var opts = new CsvDataReaderOptions { Schema = new CsvSchema(salesRecordsSchema), StringFactory = sp.GetString };
		using var reader = CsvDataReader.Create(DataFileName, opts);

		salesRecords =
			reader
			.GetRecords<SalesRecord>()
			.ToArray();
	}

	public static Schema GetSchema() => salesRecordsSchema;

	public static SalesRecord[] GetRecords() => salesRecords;

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
		return salesRecords.AsDataReader();
	}
}
