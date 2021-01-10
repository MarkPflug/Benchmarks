using BenchmarkDotNet.Running;

namespace CsvBenchmark
{
	static class Program
	{
		public static void Main(string[] args)
		{
			var summary = 
				BenchmarkSwitcher
				.FromAssembly(typeof(Program).Assembly).Run(args);
		}
	}
}