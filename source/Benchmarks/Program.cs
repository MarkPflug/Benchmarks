using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using System.Text;

namespace Benchmarks;

static class Program
{
	public static void Main(string[] args)
	{
		// needed for xbase and .xls formats.
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		BenchmarkSwitcher
		.FromAssembly(typeof(Program).Assembly)
		.Run(args, new MyConfig());
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