﻿using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;

namespace Benchmarks;

static class Program
{
	public static void Main(string[] args)
	{
		// needed for Excel .xls readers and dbf
		//Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

		BenchmarkRunner.Run<PerformanceCounterBench>(new MyConfig(), args);
	}
}

class MyConfig : ManualConfig
{
	public MyConfig() : base()
	{
		AddJob(Job.InProcess.WithMinIterationCount(1).WithWarmupCount(2).WithMaxIterationCount(6));
		AddLogger(ConsoleLogger.Default)
			.WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest));
		AddColumnProvider(DefaultColumnProviders.Instance);

	}
}