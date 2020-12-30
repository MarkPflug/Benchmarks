using BenchmarkDotNet.Attributes;
using NDbfReader;
using Sylvan.Data.XBase;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;

namespace CsvBenchmark
{
	[MemoryDiagnoser]
	public class DbfDataReaderBenchmarks
	{
		const string Url = "https://www2.census.gov/geo/tiger/GENZ2018/shp/cb_2018_us_county_20m.zip";
		const string ShapeFileName = "cb_2018_us_county_20m.zip";
		const string DbfFileName = "cb_2018_us_county_20m.dbf";
		readonly byte[] dbfData;

		public DbfDataReaderBenchmarks()
		{
			if (!File.Exists(ShapeFileName))
			{
				using var oStream = File.Create(ShapeFileName);
				using var iStream = new HttpClient().GetStreamAsync(Url).Result;
				iStream.CopyTo(oStream);
			}

			if (!File.Exists(DbfFileName))
			{
				var za = ZipFile.OpenRead(ShapeFileName);
				var entry = za.GetEntry(DbfFileName);
				entry.ExtractToFile(DbfFileName);
			}
			dbfData = File.ReadAllBytes(DbfFileName);
		}

		[Benchmark]
		public void Sylvan()
		{
			var ms = new MemoryStream(dbfData);
			var dr = XBaseDataReader.Create(ms);
			dr.Process();
		}

		[Benchmark(Baseline = true)]
		public void NDbf()
		{
			var ms = new MemoryStream(dbfData);
			var t = Table.Open(ms);
			var dr = t.OpenReader();
			dr.Process();
		}

		[Benchmark]
		public void DbfData()
		{
			var ms = new MemoryStream(dbfData);
			var dr = new DbfDataReader.DbfDataReader(ms, new DbfDataReader.DbfDataReaderOptions() { SkipDeletedRecords = true });
			dr.Process();
		}
	}

	static class Extensions
	{
		public static void Process(this Reader reader)
		{
			while (reader.Read())
			{
				reader.ProcessRecord();
			}
		}

		public static void ProcessRecord(this Reader record)
		{
			foreach(var col in record.Table.Columns)
			{
				switch (Type.GetTypeCode(col.Type))
				{
					case TypeCode.Boolean:
						record.GetBoolean(col);
						break;
					case TypeCode.Int32:
						record.GetInt32(col);
						break;
					case TypeCode.DateTime:
						record.GetDateTime(col);
						break;
					case TypeCode.Decimal:
						record.GetDecimal(col);
						break;
					case TypeCode.String:
						record.GetString(col);
						break;
					default:
						continue;
				}
			}
		}
	}
}
