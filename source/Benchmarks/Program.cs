using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
	static class Program
	{
		public static void Main(string[] args)
		{

			new XlsBenchmarks().NpoiXls();

			// NPOI is not optimized, so we disable this validation
			var config = DefaultConfig.Instance.WithOption(ConfigOptions.DisableOptimizationsValidator, true);
			BenchmarkSwitcher
			.FromAssembly(typeof(Program).Assembly).Run(args,config);
		}
	}
}