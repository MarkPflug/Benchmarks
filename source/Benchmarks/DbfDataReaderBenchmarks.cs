using BenchmarkDotNet.Attributes;
using NDbfReader;
using Sylvan;
using Sylvan.Data.XBase;
using System;
using System.Data.Common;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;

namespace Benchmarks;

[MemoryDiagnoser]
public class DbfDataReaderBenchmarks
{
	const string Url = "https://prd-tnm.s3.amazonaws.com/StagedProducts/GovtUnit/Shape/GOVTUNIT_Oregon_State_Shape.zip";
	const string ShapeFileName = "GOVTUNIT_Oregon_State_Shape.zip";
	const string DbfFileName = "GU_PLSSFirstDivision.dbf";
	readonly byte[] dbfData;
	StringPool pool;

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
			var entry = za.GetEntry("Shape/" + DbfFileName);
			entry.ExtractToFile(DbfFileName);
		}
		dbfData = File.ReadAllBytes(DbfFileName);
		this.pool = new StringPool(128);
	}

	[Benchmark]
	public void Sylvan()
	{
		var ms = new MemoryStream(dbfData);
		var dr = XBaseDataReader.Create(ms);
		dr.ProcessGeo();
	}

	[Benchmark]
	public void SylvanPooled()
	{
		var ms = new MemoryStream(dbfData);
		var opts = new XBaseDataReaderOptions { StringFactory = pool.GetString };
		var dr = XBaseDataReader.Create(ms, opts);
		dr.ProcessGeo();
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
		var opts = new DbfDataReader.DbfDataReaderOptions() { 
			SkipDeletedRecords = true 
		};
		var dr = new DbfDataReader.DbfDataReader(ms, opts);
		dr.Process();
	}
}

static class Extensions
{
	public static void ProcessGeo(this DbDataReader reader)
	{
		while (reader.Read())
		{
			reader.GetInt32(0);
			reader.GetGuid(1);
			reader.GetString(2);
			reader.GetGuid(3);
			reader.GetString(4);
			reader.GetString(5);
			reader.GetInt32(6);
			reader.GetString(7);
			reader.GetDateTime(8);
			reader.GetString(9);
			if (reader.IsDBNull(10))
			{
				reader.GetInt32(10);
			}
			reader.GetInt32(11);
			reader.GetString(12);
			reader.GetString(13);
			reader.GetString(14);
			reader.GetString(15);
			reader.GetString(16);
			reader.GetString(17);
			reader.GetGuid(18);
			reader.GetDouble(19);
			reader.GetDouble(20);
		}
	}


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
		// NOTE: DbfDataReader column  schema doesn't seem to agree with GetFieldType

		var cols = reader.GetColumnSchema();
		bool[] allowDbNull = cols.Select(c => c.AllowDBNull != false).ToArray();
		TypeCode[] types = new TypeCode[cols.Count];
		for(int i = 0; i < types.Length; i++)
		{
			var t = reader.GetFieldType(i);
			t = Nullable.GetUnderlyingType(t) ?? t;
			types[i] = Type.GetTypeCode(t);
		}
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
				reader.GetInt32(i);
				break;
			case TypeCode.Int64:
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
