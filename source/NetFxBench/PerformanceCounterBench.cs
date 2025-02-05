using BenchmarkDotNet.Attributes;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Benchmarks;

[MemoryDiagnoser]
public class PerformanceCounterBench
{
	Meter meter;
	Counter<int> counter;
	PerformanceCounter c1;
	PerformanceCounter c2;
	ConcurrentDictionary<string, PerformanceCounter> d;

	string prefix = "store_name";
	string suffix = "myperformancecounter";

	Process proc;

	public PerformanceCounterBench()
	{
		this.meter = new Meter("Sylvan.Bench");
		this.counter = this.meter.CreateCounter<int>("counter");

		if (!PerformanceCounterCategory.Exists("Sylvan"))
		{
			var counterDataCollection = new CounterCreationDataCollection();

			// Add the counter.
			var averageCount64 = new CounterCreationData()
			{
				CounterType = PerformanceCounterType.AverageCount64,
				CounterName = "AverageCounter64Sample",
			};

			counterDataCollection.Add(averageCount64);

			// Add the base counter.
			var averageCount64Base = new CounterCreationData()
			{
				CounterType = PerformanceCounterType.AverageBase,
				CounterName = "AverageCounter64SampleBase",
			};
			counterDataCollection.Add(averageCount64Base);


			PerformanceCounterCategory.Create("Sylvan",
				"Test",
				PerformanceCounterCategoryType.SingleInstance, counterDataCollection);
		}
		d = new ConcurrentDictionary<string, PerformanceCounter>();
		c1 = new PerformanceCounter("Sylvan", "AverageCounter64Sample", false);
		c2 = new PerformanceCounter("Sylvan", "AverageCounter64SampleBase", false);
		d.TryAdd("_store_name_myperformancecounter", c1);

		var id = Process.GetCurrentProcess().Id;

		this.proc = Process.Start(
			new ProcessStartInfo
			{
				FileName = "dotnet-counters",
				Arguments = $"monitor -p {id} --counters Sylvan.Bench"
			}
		);
	}

	[Benchmark(Baseline =true)]
	public void Test1()
	{
		c1.Increment();
		//c2.Increment();
	}

	[Benchmark]
	public void Test2()
	{
		// yes, someone implemented perf counters like this, where they were
		// accessed by a constructed name dictionary lookup. Which ends up
		// being ~10x the cost of just incrementing a counter via static access.
		if (d.TryGetValue(string.Format(prefix + "_{0}", suffix), out var c1))
		{
			c1.Increment();
		}
	}

	[Benchmark]
	public void Meter()
	{
		counter.Add(1);
	}
}
