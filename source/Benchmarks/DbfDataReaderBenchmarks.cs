using BenchmarkDotNet.Attributes;
using Sylvan;
using Sylvan.Data.XBase;
using System.IO;
using System.Threading.Tasks;

namespace Benchmarks;

[MemoryDiagnoser]
[HideColumns("StdDev", "RatioSD", "Gen0", "Gen1", "Gen2")]
public class DbfDataReaderBenchmarks
{
	const string DbfFileName = "Data/DevArea.dbf";
	readonly byte[] dbfData;
	StringPool pool;

	public DbfDataReaderBenchmarks()
	{
		dbfData = File.ReadAllBytes(DbfFileName);
	}

	[Benchmark(Baseline = true)]
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
