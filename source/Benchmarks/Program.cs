using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using System;

namespace Benchmarks
{
	static class Program
	{
		public static void Main(string[] args)
		{
			//var b = new CsvDataBinderBenchmarks();
			//b.RecordParserBind().Wait();
			//b.SylvanCompiledReuse();
			//b.SylvanCompiled();
			//b.SpreadSheetTasksXlsx();
			var config = new MyConfig();
				
			BenchmarkSwitcher
			.FromAssembly(typeof(Program).Assembly)
			.Run(args,config);
		}
	}

	class MyConfig : ManualConfig
	{
		public MyConfig(): base()
		{
			AddJob(Job.InProcess.WithMinIterationCount(1).WithWarmupCount(2).WithMaxIterationCount(6));
			// NPOI is not optimized...
			WithOption(ConfigOptions.DisableOptimizationsValidator, true);
			AddLogger(ConsoleLogger.Default);
			AddColumnProvider(DefaultColumnProviders.Instance);
		}
	}
}