﻿using BenchmarkDotNet.Analysers;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Benchmarks;

public sealed class CpuDiagnoserAttribute : Attribute, IConfigSource
{
	public IConfig Config { get; }

	public CpuDiagnoserAttribute()
	{
		Config = ManualConfig.CreateEmpty().AddDiagnoser(new CpuDiagnoser());
	}
}

public sealed class CpuDiagnoser : IDiagnoser
{
	Process proc;
	
	public CpuDiagnoser()
	{
		this.proc = Process.GetCurrentProcess();
	}

	public IEnumerable<string> Ids => new[] { "CPU" };

	public IEnumerable<IExporter> Exporters => Array.Empty<IExporter>();

	public IEnumerable<IAnalyser> Analysers => Array.Empty<IAnalyser>();

	public void DisplayResults(ILogger logger)
	{
	}

	public RunMode GetRunMode(BenchmarkCase benchmarkCase)
	{
		return RunMode.NoOverhead;
	}

	long userStart, userEnd;
	long privStart, privEnd;

	public void Handle(HostSignal signal, DiagnoserActionParameters parameters)
	{
		if(signal == HostSignal.BeforeActualRun)
		{
			userStart = proc.UserProcessorTime.Ticks;
			privStart = proc.PrivilegedProcessorTime.Ticks;
		}
		if(signal == HostSignal.AfterActualRun)
		{
			userEnd = proc.UserProcessorTime.Ticks;
			privEnd = proc.PrivilegedProcessorTime.Ticks;
		}
	}

	public IEnumerable<Metric> ProcessResults(DiagnoserResults results)
	{
		yield return new Metric(CpuUserMetricDescriptor.Instance, (userEnd - userStart) * 100d / results.TotalOperations);
		yield return new Metric(CpuKernelMetricDescriptor.Instance, (privEnd - privStart) * 100d / results.TotalOperations);
	}

	public IEnumerable<ValidationError> Validate(ValidationParameters validationParameters)
	{
		yield break;
	}

	public bool RequiresBlockingAcknowledgments(BenchmarkCase benchmarkCase)
	{
		return false;
	}

	class CpuUserMetricDescriptor : IMetricDescriptor
	{
		internal static readonly IMetricDescriptor Instance = new CpuUserMetricDescriptor();

		public string Id => "CPU User Time";
		public string DisplayName => Id;
		public string Legend => Id;
		public string NumberFormat => "0.##";
		public UnitType UnitType => UnitType.Time;
		public string Unit => "ns";
		public bool TheGreaterTheBetter => false;
		public int PriorityInCategory => 1;
	}

	class CpuKernelMetricDescriptor : IMetricDescriptor
	{
		internal static readonly IMetricDescriptor Instance = new CpuKernelMetricDescriptor();

		public string Id => "CPU Kernel Time";
		public string DisplayName => Id;
		public string Legend => Id;
		public string NumberFormat => "0.##";
		public UnitType UnitType => UnitType.Time;
		public string Unit => "ns";
		public bool TheGreaterTheBetter => false;
		public int PriorityInCategory => 1;
	}
}
