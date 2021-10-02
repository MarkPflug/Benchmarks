using BenchmarkDotNet.Attributes;
using NDbfReader;
using Sylvan.Data.XBase;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;

namespace Benchmarks
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

		[Benchmark]
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
							// shouldn't get here. If we do, our benchmarks are bogus anyway.
							throw new NotSupportedException(type.ToString());
					}
				}
			}
		}

		public static void Process(this DbfDataReader.DbfDataReader reader)
		{
			var cols = reader.GetColumnSchema();
			bool[] allowDbNull = cols.Select(c => c.AllowDBNull != false).ToArray();
			TypeCode[] types = cols.Select(c => Type.GetTypeCode(c.DataType)).ToArray();
			while (reader.Read())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					if (allowDbNull[i] && reader.IsDBNull(i))
						continue;
					ProcessField(reader, i, types[i]);
				}
			}
		}

		static void ProcessField(this DbfDataReader.DbfDataReader reader, int i, TypeCode typeCode)
		{
			switch (typeCode)
			{
				case TypeCode.Boolean:
					reader.GetBoolean(i);
					break;
				case TypeCode.Int32:
					// ??? for some reason this fails if accessed
					// via GetInt32, throws a cast exception.
					reader.GetInt64(i);
					break;
				case TypeCode.DateTime:
					reader.GetDateTime(i);
					break;
				case TypeCode.Single:
					reader.GetFloat(i);
					break;
				case TypeCode.Double:
					reader.GetDouble(i);
					break;
				case TypeCode.Decimal:
					reader.GetDecimal(i);
					break;
				case TypeCode.String:
					reader.GetString(i);
					break;
				default:
					// no cheating
					throw new NotSupportedException("" + typeCode);
			}
		}
	}
}
