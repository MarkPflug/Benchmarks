using BenchmarkDotNet.Attributes;
using Cursively;
using Sylvan.Data.Csv;
using System;
using static fastCSV;

namespace Benchmarks
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
			DateTime orderDate;
			string type;
			decimal profit;
			var rows =
				fastCSV.ReadStream<object>(
					TestData.GetTextReader(),
					',',
					(object obj, COLUMNS cols) =>
					{
						type = cols[2];
						orderDate = DateTime.Parse(cols[5]);
						id = int.Parse(cols[6]);
						profit = decimal.Parse(cols[13]);
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
				var type = dr[2];
				var orderDate = DateTime.Parse(dr[5]);
				var id = int.Parse(dr[6]);
				var profit = decimal.Parse(dr[13]);
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
				var type = dr.GetString(2);
				var orderDate = dr.GetDateTime(5);
				var id = dr.GetInt32(6);
				var profit = dr.GetDecimal(13);
			}
		}
	}
}
