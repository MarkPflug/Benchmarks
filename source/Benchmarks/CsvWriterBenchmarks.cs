using BenchmarkDotNet.Attributes;
using CsvHelper.Configuration;
using Sylvan.Data.Csv;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Benchmarks
{
	[MemoryDiagnoser]
	[SimpleJob(1, 2, 4, 1)]
	public class CsvWriterBenchmarks
	{
		static readonly int ValueCount = TestData.DefaultDataValueCount;

		[Benchmark(Baseline = true)]
		public void NaiveBroken()
		{
			TextWriter tw = TextWriter.Null;

			var items = TestData.GetTestObjects();
			tw.Write("Id");
			tw.Write(',');
			tw.Write("Name");
			tw.Write(',');
			tw.Write("Date");
			tw.Write(',');
			tw.Write("IsActive");
			for (int i = 0; i < ValueCount; i++)
			{
				tw.Write(',');
				tw.Write("Value" + i);
			}
			tw.WriteLine();

			foreach (var item in items)
			{
				tw.Write(item.Id);
				tw.Write(',');
				tw.Write(item.Name);
				tw.Write(',');
				tw.Write(item.Date);
				tw.Write(',');
				tw.Write(item.IsActive);
				for (int i = 0; i < ValueCount; i++)
				{
					tw.Write(',');
					tw.Write(item.DataSet[i]);
				}
				tw.WriteLine();
			}
		}

		[Benchmark]
		public void CsvHelperSync()
		{
			TextWriter tw = TextWriter.Null;
			var items = TestData.GetTestObjects();
			var csv = new CsvHelper.CsvWriter(tw, new CsvConfiguration(CultureInfo.InvariantCulture));
			csv.WriteField("Id");
			csv.WriteField("Name");
			csv.WriteField("Date");
			csv.WriteField("IsActive");
			for (int i = 0; i < ValueCount; i++)
			{
				csv.WriteField("Value" + i);
			}
			csv.NextRecord();

			foreach (var item in items)
			{
				csv.WriteField(item.Id);
				csv.WriteField(item.Name);
				csv.WriteField(item.Date);
				csv.WriteField(item.IsActive);
				for (int i = 0; i < ValueCount; i++)
				{
					csv.WriteField(item.DataSet[i]);
				}
				csv.NextRecord();
			}
			csv.Flush();
		}

		[Benchmark]
		public async Task CsvHelperAsync()
		{
			TextWriter tw = TextWriter.Null;
			var items = TestData.GetTestObjects();
			var csv = new CsvHelper.CsvWriter(tw, new CsvConfiguration(CultureInfo.InvariantCulture));
			csv.WriteField("Id");
			csv.WriteField("Name");
			csv.WriteField("Date");
			csv.WriteField("IsActive");
			for (int i = 0; i < ValueCount; i++)
			{
				csv.WriteField("Value" + i);
			}
			await csv.NextRecordAsync();

			foreach (var item in items)
			{
				csv.WriteField(item.Id);
				csv.WriteField(item.Name);
				csv.WriteField(item.Date);
				csv.WriteField(item.IsActive);
				for (int i = 0; i < ValueCount; i++)
				{
					csv.WriteField(item.DataSet[i]);
				}
				await csv.NextRecordAsync();
				await csv.FlushAsync();
			}
			await csv.FlushAsync();
		}

		[Benchmark]
		public void NLightCsv()
		{
			TextWriter tw = TextWriter.Null;
			var items = TestData.GetTestObjects();
			var csv = new NLight.IO.Text.DelimitedRecordWriter(tw);
			csv.WriteField("Id");
			csv.WriteField("Name");
			csv.WriteField("Date");
			csv.WriteField("IsActive");
			for (int i = 0; i < ValueCount; i++)
			{
				csv.WriteField("Value" + i);
			}
			csv.WriteRecordEnd();

			foreach (var item in items)
			{
				csv.WriteField(item.Id);
				csv.WriteField(item.Name);
				csv.WriteField(item.Date);
				csv.WriteField(item.IsActive);
				for (int i = 0; i < ValueCount; i++)
				{
					csv.WriteField(item.DataSet[i]);
				}
				csv.WriteRecordEnd();
			}
		}

		[Benchmark]
		public void NReco()
		{
			TextWriter tw = TextWriter.Null;
			var items = TestData.GetTestObjects();
			var csv = new NReco.Csv.CsvWriter(tw);
			csv.WriteField("Id");
			csv.WriteField("Name");
			csv.WriteField("Date");
			csv.WriteField("IsActive");
			for (int i = 0; i < ValueCount; i++)
			{
				csv.WriteField("Value" + i);
			}
			csv.NextRecord();
			var c = CultureInfo.InvariantCulture;
			foreach (var item in items)
			{
				csv.WriteField(item.Id.ToString(c));
				csv.WriteField(item.Name);
				csv.WriteField(item.Date.ToString(c));
				csv.WriteField(item.IsActive.ToString(c));
				for (int i = 0; i < ValueCount; i++)
				{
					csv.WriteField(item.DataSet[i].ToString(c));
				}
				csv.NextRecord();
			}
		}

		//[Benchmark]
		//public async Task SylvanDataAsync()
		//{
		//	var tw = TextWriter.Null;
		//	var dr = TestData.GetTestDataReader();
		//	var csv = new CsvDataWriter(tw);
		//	await csv.WriteAsync(dr);
		//}

		//[Benchmark]
		//public void SylvanDataSync()
		//{
		//	var tw = TextWriter.Null;
		//	var dr = TestData.GetTestDataReader();
		//	var csv = new CsvDataWriter(tw);
		//	csv.Write(dr);
		//}
	}
}
