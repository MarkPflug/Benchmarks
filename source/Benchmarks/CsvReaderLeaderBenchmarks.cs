using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
	[MemoryDiagnoser]
	[SimpleJob(1, 2, 4, 1)]
	public class CsvReaderLeaderBenchmarks
	{
		CsvReaderBenchmarks bench;

		public CsvReaderLeaderBenchmarks()
		{
			this.bench = new CsvReaderBenchmarks();
		}

		[Benchmark(Baseline = true)]
		public void NaiveBroken()
		{
			bench.NaiveBroken();
		}

		[Benchmark]
		public void CsvHelperCsv()
		{
			bench.CsvHelperCsv();
		}

		[Benchmark]
		public void SoftCircuits()
		{
			bench.SoftCircuits();
		}

		[Benchmark]
		public void FastCsvParser()
		{
			bench.FastCsvParser();
		}

		[Benchmark]
		public void MgholamFastCSV()
		{
			bench.MgholamFastCSV();
		}

		[Benchmark]
		public void CursivelyCsv()
		{
			bench.CursivelyCsv();
		}

		[Benchmark]
		public void CtlData()
		{
			bench.CtlData();
		}

		[Benchmark]
		public void NReco()
		{
			bench.NReco();
		}

		[Benchmark]
		public void Sylvan()
		{
			bench.Sylvan();
		}
	}
}
