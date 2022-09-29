using BenchmarkDotNet.Attributes;
using CsvHelper.Configuration;
using Sylvan.Data;
using Sylvan.Data.Csv;
using System;
using System.Buffers;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks;

[CpuDiagnoser]
[MemoryDiagnoser]
public class CsvWriterBenchmarks
{
	MemoryStream ms;

	public CsvWriterBenchmarks()
	{
		this.ms = new MemoryStream(10 * 0x100000); // 10mb
	}

	Stream GetStream([CallerMemberName] string name = null)
	{
		//this.ms.Position = 0;
		//this.ms.SetLength(0);
		//return new NoCloseStream(this.ms);
		return File.Create(name + ".csv");
	}

	TextWriter GetWriter([CallerMemberName] string name = null)
	{
		var s = GetStream(name);
		return new StreamWriter(s, Encoding.UTF8, 0x10000);
	}

	DbDataReader GetData()
	{
		return TestData.GetData();
	}

	SalesRecord[] GetRecords()
	{
		return TestData.GetRecords();
	}

	[Benchmark]
	public void NaiveBroken()
	{
		using var tw = GetWriter();

		var data = GetData();

		var count = data.FieldCount;
		while (data.Read())
		{
			for (int i = 0; i < count; i++)
			{
				if (i > 0)
					tw.Write(',');
				tw.Write(data.GetValue(i)?.ToString());
			}
			tw.WriteLine();
		}
	}

	[Benchmark]
	public void RecordParserParallel()
	{
		using var tw = GetWriter();
		// I don't see a way to use this library without a `T`, so can't use DbDataReader directly.
		var items = GetRecords();

		var builder = new RecordParser.Builders.Writer.VariableLengthWriterSequentialBuilder<SalesRecord>();
		builder.Map(x => x.Region);
		builder.Map(x => x.Country);
		builder.Map(x => x.ItemType);
		builder.Map(x => x.SalesChannel);
		builder.Map(x => x.OrderPriority);
		builder.Map(x => x.OrderDate);
		builder.Map(x => x.OrderId);
		builder.Map(x => x.ShipDate);
		builder.Map(x => x.UnitsSold);
		builder.Map(x => x.UnitPrice);
		builder.Map(x => x.UnitCost);
		builder.Map(x => x.TotalRevenue);
		builder.Map(x => x.TotalCost);
		builder.Map(x => x.TotalProfit);

		var csv = builder.Build(",");

		var parallelism = 4;
		var buffers = Enumerable
			.Range(0, parallelism)
			.Select(_ => (pow: 10,
						  buffer: ArrayPool<char>.Shared.Rent((int)Math.Pow(2, 10)),
						  lockObj: new object()))
			.ToArray();

		var textWriterLock = new object();

		Parallel.ForEach(items, new ParallelOptions { MaxDegreeOfParallelism = parallelism }, (item, _, i) =>
		{
			var x = buffers[i % parallelism];

			lock (x.lockObj)
			{
				x = buffers[i % parallelism];

			retry:

				if (csv.TryFormat(item, x.buffer, out var charsWritten))
				{
					lock (textWriterLock)
					{
						tw.WriteLine(x.buffer, 0, charsWritten);
					}
				}
				else
				{
					ArrayPool<char>.Shared.Return(x.buffer);
					x.pow++;
					x.buffer = ArrayPool<char>.Shared.Rent((int)Math.Pow(2, x.pow));

					buffers[i % parallelism] = x;
					goto retry;
				}
			}
		});

		foreach (var x in buffers)
			ArrayPool<char>.Shared.Return(x.buffer);
	}

	[Benchmark]
	public async Task RecordParserAsync()
	{
		using var tw = GetWriter();
		// I don't see a way to use this library without a `T`, so can't use DbDataReader directly.
		var items = GetRecords();

		var builder = new RecordParser.Builders.Writer.VariableLengthWriterSequentialBuilder<SalesRecord>();
		builder.Map(x => x.Region);
		builder.Map(x => x.Country);
		builder.Map(x => x.ItemType);
		builder.Map(x => x.SalesChannel);
		builder.Map(x => x.OrderPriority);
		builder.Map(x => x.OrderDate);
		builder.Map(x => x.OrderId);
		builder.Map(x => x.ShipDate);
		builder.Map(x => x.UnitsSold);
		builder.Map(x => x.UnitPrice);
		builder.Map(x => x.UnitCost);
		builder.Map(x => x.TotalRevenue);
		builder.Map(x => x.TotalCost);
		builder.Map(x => x.TotalProfit);

		var csv = builder.Build(",");			

		var charsWritten = 0;
		var pow = 8;
		var buffer = ArrayPool<char>.Shared.Rent((int)Math.Pow(2, pow));
		foreach (var item in items)
		{
		retry:

			if (csv.TryFormat(item, buffer, out charsWritten))
			{
				await tw.WriteLineAsync(buffer, 0, charsWritten);
			}
			else
			{
				ArrayPool<char>.Shared.Return(buffer);
				pow++;
				buffer = ArrayPool<char>.Shared.Rent((int)Math.Pow(2, pow));
				goto retry;
			}
		}
	}

	[Benchmark]
	public void CsvHelperSync()
	{
		using var tw = GetWriter();
		var data = GetData();
		var csv = new CsvHelper.CsvWriter(tw, new CsvConfiguration(CultureInfo.InvariantCulture));

		var count = data.FieldCount;
		while (data.Read())
		{
			for (int i = 0; i < count; i++)
			{
				var t = data.GetFieldType(i);
				var c = Type.GetTypeCode(t);
				switch (c)
				{
					case TypeCode.String:
						csv.WriteField(data.GetString(i));
						break;
					case TypeCode.Decimal:
						csv.WriteField(data.GetDecimal(i));
						break;
					case TypeCode.Int32:
						csv.WriteField(data.GetInt32(i));
						break;
					case TypeCode.DateTime:
						csv.WriteField(data.GetDateTime(i));
						break;
					default:
						throw new NotImplementedException();
				}
			}
			csv.NextRecord();
		}		
	}

	[Benchmark]
	public async Task CsvHelperAsync()
	{
		using var tw = GetWriter();
		var data = GetData();
		var csv = new CsvHelper.CsvWriter(tw, new CsvConfiguration(CultureInfo.InvariantCulture));

		var count = data.FieldCount;
		while (await data.ReadAsync())
		{
			for (int i = 0; i < count; i++)
			{
				var t = data.GetFieldType(i);
				var c = Type.GetTypeCode(t);
				switch (c)
				{
					case TypeCode.String:
						csv.WriteField(data.GetString(i));
						break;
					case TypeCode.Decimal:
						csv.WriteField(data.GetDecimal(i));
						break;
					case TypeCode.Int32:
						csv.WriteField(data.GetInt32(i));
						break;
					case TypeCode.DateTime:
						csv.WriteField(data.GetDateTime(i));
						break;
					default:
						throw new NotImplementedException();
				}
			}
			await csv.NextRecordAsync();
		}
	}

	[Benchmark]
	public void NLightCsv()
	{
		using var tw = GetWriter();
		var data = GetData();

		var csv = new NLight.IO.Text.DelimitedRecordWriter(tw);
		var count = data.FieldCount;
		while (data.Read())
		{
			for (int i = 0; i < count; i++)
			{
				csv.WriteField(data.GetValue(i));
			}
			csv.WriteRecordEnd();
		}
	}

	[Benchmark]
	public void NReco()
	{
		using var tw = GetWriter();
		var data = GetData();

		var csv = new NReco.Csv.CsvWriter(tw);

		var c = CultureInfo.InvariantCulture;
		var count = data.FieldCount;
		while(data.Read())
		{
			for (int i = 0; i < count; i++)
			{
				csv.WriteField(data.GetValue(i)?.ToString());
			}
			csv.NextRecord();
		}
	}

	[Benchmark]
	public async Task SylvanDataAsync()
	{
		using var tw = GetWriter();
		var data = GetData();
		var opts = new CsvDataWriterOptions { BufferSize = 0x10000, NewLine = "\n" };
		var csv = CsvDataWriter.Create(tw, opts);
		await csv.WriteAsync(data);
	}

	[Benchmark]
	public void SylvanDataSync()
	{
		using var tw = GetWriter();
		var data = GetData();
		var opts = new CsvDataWriterOptions { BufferSize = 0x10000, NewLine = "\n" };
		var csv = CsvDataWriter.Create(tw, opts);
		csv.Write(data);
	}
}
