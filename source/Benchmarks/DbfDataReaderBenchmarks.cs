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
	[SimpleJob(1, 2, 4, 1)]
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
			var cols = reader.Table.Columns;
			TypeCode[] code = new TypeCode[cols.Count];

			int colIdx = 0;
			foreach (var col in cols)
			{
				var type = col.Type;
				type = Nullable.GetUnderlyingType(type) ?? type;
				code[colIdx++] = Type.GetTypeCode(type);
			}

			while (reader.Read())
			{
				colIdx = 0;
				foreach (var col in cols)
				{
					var type = code[colIdx++];
				
					switch (type)
					{
						case TypeCode.Boolean:
							reader.GetBoolean(col);
							break;
						case TypeCode.Int32:
							reader.GetInt32(col);
							break;
						case TypeCode.DateTime:
							reader.GetDateTime(col);
							break;
						case TypeCode.Decimal:
							reader.GetDecimal(col);
							break;
						case TypeCode.String:
							reader.GetString(col);
							break;
						default:
							// shouldn't get here if we do, our benchmarks are bogus anyway.
							throw new NotSupportedException(type.ToString());
					}
				}
			}
		}
	}
}
