using BenchmarkDotNet.Attributes;
using NDbfReader;
using Sylvan;
using Sylvan.Data.XBase;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

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
	}

	[Benchmark]
	public void Sylvan()
	{
		var ms = new MemoryStream(dbfData);
		var dr = XBaseDataReader.Create(ms);
		dr.Process();
	}

	[Benchmark]
	public async Task SylvanAsync()
	{
		var ms = new MemoryStream(dbfData);
		var dr = XBaseDataReader.Create(ms);
		await dr.ProcessAsync();
	}

	[Benchmark]
	public void SylvanPooled()
	{
		var ms = new MemoryStream(dbfData);
		pool = new StringPool(128);
		var opts = new XBaseDataReaderOptions { StringFactory = pool.GetString };
		var dr = XBaseDataReader.Create(ms, opts);
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
		var opts = new DbfDataReader.DbfDataReaderOptions() { 
			SkipDeletedRecords = true 
		};
		var dr = new DbfDataReader.DbfDataReader(ms, opts);
		dr.Process();
	}
}

static class Extensions
{	
	//ndbf
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
}
