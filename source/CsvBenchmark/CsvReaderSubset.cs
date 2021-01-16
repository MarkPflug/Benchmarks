using BenchmarkDotNet.Attributes;

namespace CsvBenchmark
{
	// used to bench an interesting subset of the parsers.
	[MemoryDiagnoser]
	class CsvReaderSubset
	{
		CsvReaderBenchmarks bench;
		public CsvReaderSubset()
		{
			this.bench = new CsvReaderBenchmarks();
		}

		[Benchmark(Baseline = true)]
		public void Naive()
		{
			bench.NaiveBroken();
		}

		[Benchmark]
		public void Fluent()
		{
			bench.Fluent();
		}

		//[Benchmark]
		//public void SylvanSelect()
		//{
		//	bench.SylvanSelect();
		//}

		//[Benchmark]
		//public void NrecoSelect()
		//{
		//	bench.NRecoSelect();
		//}

		//[Benchmark]
		//public void CursivelySelect()
		//{
		//	bench.CursivelyCsvSelect();
		//}

		//[Benchmark]
		//public void MgholamSelect()
		//{
		//	bench.MgholamFastCsvSelect();
		//}
	}
}
