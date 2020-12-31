using BenchmarkDotNet.Running;
using System.Text;

namespace CsvBenchmark
{
	static class Program
	{
		public static void Main(string[] args)
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			var n = new DbfDataReaderBenchmarks();
		//n.NDbf();
			n.Sylvan();
			var summary = BenchmarkSwitcher
				.FromAssembly(typeof(Program).Assembly).Run(args);
		}
	}
}