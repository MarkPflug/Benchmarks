using BenchmarkDotNet.Attributes;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;

namespace Benchmarks;

[MemoryDiagnoser]
public class XlsxSumBenchmarks
{
	const string file = @"Data/65K_Records_Data.xlsx";

	const string ColumnName = "Total Profit";

	[Benchmark(Baseline = true)]
	public void Baseline()
	{
		var stream = File.OpenRead(file);
		var za = new ZipArchive(stream, ZipArchiveMode.Read);
		var e = za.GetEntry("xl/worksheets/sheet1.xml");
		var s = e.Open();
		using var x = XmlReader.Create(s);
		while (x.Read()) ;
	}

	[Benchmark]
	public decimal SylvanData()
	{
		decimal total = 0m;

		using var dr = Sylvan.Data.Excel.ExcelDataReader.Create(file);
		var idx = dr.GetOrdinal(ColumnName);
		while (dr.Read())
		{
			total += dr.GetDecimal(idx);
		}
		return total;
	}

	[Benchmark]
	public decimal XlsxHelperXlsx()
	{
		decimal total = 0m;

		using var stream = File.OpenRead(file);
		using (var book = XlsxHelper.XlsxReader.OpenWorkbook(stream))
		{
			var sheet = book.Worksheets.First();
			using var reader = sheet.WorksheetReader;
			bool header = true;
			int idx = 0;
			foreach (var row in reader)
			{
				if (header)
				{
					// find the column ordinal
					var headers = row.Cells;
					for (idx = 0; idx < headers.Length; idx++)
					{
						if (headers[idx].CellValue == ColumnName)
						{
							// found the header
							break;
						}
					}
					header = false;
					continue;
				}
				//total += row.Cells[idx].GetDecimal();
				// Using the above line produces the wrong result 
				total += (decimal) row.Cells[idx].GetDouble();
			}
		}
		return total;
	}

	[Benchmark]
	public decimal ExcelDataReaderXls()
	{
		decimal total = 0m;
		using var stream = File.OpenRead(file);
		using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
		{
			int idx = 0;

			reader.Read(); // read the header row

			for (idx = 0; idx < reader.FieldCount; idx++)
			{
				if (reader.GetString(idx) == ColumnName)
				{
					break;
				}
			}
			if (idx >= reader.FieldCount)
			{
				// couldn't find the column
				throw new System.IndexOutOfRangeException();
			}
			while (reader.Read())
			{
				total += (decimal)reader.GetDouble(idx);
			}
		}
		return total;
	}

	[Benchmark]
	public decimal Prime()
	{
		decimal total = 0m;

		using ExcelPRIME.Excel_PRIME workbook = new();
		workbook.Open(file);
		var sheetName = workbook.SheetNames().First();
		using var worksheet = workbook.GetSheet(sheetName);

		bool header = true;
		var idx = 0;
		foreach (var row in worksheet!.GetRowData(0))
		{
			if (header)
			{
				// find the column
				var headers = row.GetAllCells();
				var c = headers.Count;
				for (idx = 0; idx < c; idx++)
				{
					if (headers[idx].CellValue.ToString() == ColumnName)
					{
						break;
					}
				}
				if (idx >= c)
				{
					// couldn't find the column
					throw new System.IndexOutOfRangeException();
				}
				header = false;
				idx += 1; // this library uses 1-based indexing.
				continue;
			}

			//var val = row.GetCell(idx).CellValue.AsDecimal;
			// Using the above line produces the wrong result 
			var val = (decimal)row.GetCell(idx).CellValue.AsDouble;
			//Console.WriteLine(val);
			total += val;
			row.Dispose();
		}
		return total;
	}
}
