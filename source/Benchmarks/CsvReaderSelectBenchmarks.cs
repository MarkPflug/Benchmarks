using BenchmarkDotNet.Attributes;
using Cursively;
using Sylvan.Data.Csv;
using System.Globalization;
using static fastCSV;

namespace CsvBenchmark
{
	[MemoryDiagnoser]
	public class CsvReaderSelectBenchmarks
	{
		// buffer size for libraries that allow configuration
		const int BufferSize = 0x10000;

		[Benchmark(Baseline = true)]
		public void NaiveBroken()
		{
			var tr = TestData.GetTextReader();
			string line;
			while ((line = tr.ReadLine()) != null)
			{
				var cols = line.Split(',');
				for (int i = 0; i < cols.Length; i++)
				{
					var s = cols[i];
				}
			}
		}

		[Benchmark]
		public void MgholamFastCsvSelect()
		{
			int id;
			string name;
			int val;
			var rows =
				fastCSV.ReadStream<object>(
					TestData.GetTextReader(),
					',',
					(object obj, COLUMNS cols) =>
					{
						id = int.Parse(cols[0]);
						name = cols[10];
						val = int.Parse(cols[20]);
						return false;
					}
				);
		}

		[Benchmark]
		public void NRecoSelect()
		{
			using var tr = TestData.GetTextReader();
			var dr = new NReco.Csv.CsvReader(tr);
			dr.BufferSize = BufferSize;
			dr.Read(); // read the headers
			while (dr.Read())
			{
				var id = int.Parse(dr[0]);
				var name = dr[10];
				var val = int.Parse(dr[20]);
			}
		}

		[Benchmark]
		public void CursivelyCsvSelect()
		{
			var d = TestData.GetUtf8Array();
			var proc = new CursivelySelectVisitor();
			CsvSyncInput
				.ForMemory(d)
				.Process(proc);
		}

		[Benchmark]
		public void SylvanSelect()
		{
			using var tr = TestData.GetTextReader();
			using var dr = CsvDataReader.Create(tr);
			while (dr.Read())
			{
				var id = dr.GetInt32(0);
				var name = dr.GetString(10);
				var val = dr.GetInt32(20);
			}
		}

		//[Benchmark]
		//public void CsvHelperSelect()
		//{
		//	var tr = TestData.GetTextReader();
		//	var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture)
		//	{
		//		BufferSize = BufferSize
		//	};

		//	var r = new CsvHelper.CsvReader(tr, config);
		//	var dr = new CsvHelper.CsvDataReader(r);
		//	while (dr.Read())
		//	{
		//		var id = dr.GetInt32(0);
		//		var name = dr.GetString(10);
		//		var value = dr.GetInt32(20);
		//	}
		//}
	}
}
