using BenchmarkDotNet.Running;

namespace CsvBenchmark
{
	static class Program
	{
		public static void Main(string[] args)
		{
			//new DbfDataReaderBenchmarks().DbfData();
			BenchmarkSwitcher
			.FromAssembly(typeof(Program).Assembly).Run(args);
		}
	}
}