using BenchmarkDotNet.Running;
using Dapper;
using System.Globalization;
using System.IO;

namespace CsvBenchmark
{
	static class Program
	{
		public static void Main(string[] args)
		{
			Experiments.DapperBinderTest();
			new CsvDataBinderBenchmarks().SylvanDataBinder();
			var summary = BenchmarkSwitcher
				.FromAssembly(typeof(Program).Assembly).Run(args);
		}

	}
}