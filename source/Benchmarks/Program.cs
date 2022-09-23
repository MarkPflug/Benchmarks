using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using System;

namespace Benchmarks;

static class Program
{
	public static void Main(string[] args)
	{
		var b = new XlsxWriterBenchmarks();
		b.Init();
		b.AsposeXlsx();
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
		AddLogger(ConsoleLogger.Default)
			.WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest));
		AddColumnProvider(DefaultColumnProviders.Instance);
		
	}
}