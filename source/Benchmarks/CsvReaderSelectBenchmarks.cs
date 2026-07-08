using BenchmarkDotNet.Attributes;
using ExcelReader.Core.Reader;
using Sylvan.Data.Csv;
using System;
using System.Globalization;
using static fastCSV;

namespace Benchmarks;

[MemoryDiagnoser]
public class CsvReaderSelectBenchmarks
{
	// buffer size for libraries that allow configuration
	const int BufferSize = 0x10000;

	[Benchmark]
	public void NaiveBroken()
	{
		var tr = TestData.GetTextReader();
		string line;
		while ((line = tr.ReadLine()) != null)
		{
			var cols = line.Split(',');
			for (int i = 0; i < cols.Length; i++)
			{
				var s = cols[i];
			}
		}
	}

	[Benchmark]
	public void MgholamFastCsvSelect()
	{
		int id;
		DateTime orderDate;
		string type;
		decimal profit;
		var rows =
			fastCSV.ReadStream<object>(
				TestData.GetTextReader(),
				',',
				(object obj, COLUMNS cols) =>
				{
					type = cols[2];
					orderDate = DateTime.Parse(cols[5]);
					id = int.Parse(cols[6]);
					profit = decimal.Parse(cols[13]);
					return false;
				}
			);
	}

	[Benchmark]
	public void NRecoSelect()
	{
		using var tr = TestData.GetTextReader();
		var dr = new NReco.Csv.CsvReader(tr);
		dr.BufferSize = BufferSize;
		dr.Read(); // read the headers
		while (dr.Read())
		{
			var type = dr[2];
			var orderDate = DateTime.Parse(dr[5]);
			var id = int.Parse(dr[6]);
			var profit = decimal.Parse(dr[13]);
		}
	}

	//[Benchmark]
	//public void CursivelyCsvSelect()
	//{
	//	var d = TestData.GetUtf8Array();
	//	var proc = new CursivelySelectVisitor();
	//	CsvSyncInput
	//		.ForMemory(d)
	//		.Process(proc);
	//}

	[Benchmark]
	public void SylvanSelect()
	{
		using var tr = TestData.GetTextReader();
		using var dr = CsvDataReader.Create(tr);
		while (dr.Read())
		{
			var type = dr.GetString(2);
			var orderDate = dr.GetDateTime(5);
			var id = dr.GetInt32(6);
			var profit = dr.GetDecimal(13);
		}
	}

	[Benchmark]
	public void ExcelReaderNetSelect()
	{
		var stream = TestData.GetUtf8Stream();
		using var reader = Excel.FromCsv(stream, true);
		using var enumerator = reader.GetEnumerator();
		enumerator.MoveNext(); // skip headers
		while (enumerator.MoveNext())
		{
			var row = enumerator.Current;
			var type = row[2].ToString();
			var orderDate = row[5].TryGetDateTime(out var dt) ? dt : default;
			var id = row[6].TryParse<int>(CultureInfo.InvariantCulture, out var i) ? i : default;
			var profit = row[13].TryParse<decimal>(CultureInfo.InvariantCulture, out var p) ? p : default;
		}
	}
}