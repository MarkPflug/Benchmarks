using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ExcelPRIME;
using System.IO;
using System.Runtime.Versioning;
using System.Text;

namespace Benchmarks;

[MemoryDiagnoser]
[HideColumns("StdDev", "RatioSD", "Gen0", "Gen1", "Gen2")]
public class XlsbReaderBenchmarks
{
	const string file = @"Data/65K_Records_Data.xlsb";

	public XlsbReaderBenchmarks()
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	}

	[Benchmark(Baseline = true)]
	public void SylvanXlsb()
	{
		var reader = Sylvan.Data.Excel.ExcelDataReader.Create(file);
		do
		{
			while (reader.Read())
			{
				reader.ProcessSalesRecord();
			}

		} while (reader.NextResult());
	}

	[Benchmark]
	[SupportedOSPlatform("windows")]
	public void AceOleDbXlsb()
	{
		AceOleDb.ProcessFile(file);
	}

	[Benchmark]
	public void ExcelDataReaderXlsb()
	{
		using var stream = File.OpenRead(file);
		using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
		{
			do
			{
				reader.Read();//skip header
				while (reader.Read())
				{
					reader.ProcessSalesRecordEDR();
				}
			} while (reader.NextResult());
		}
	}

	static void ProcessAsposeRecord(Row row)
	{
		var region = row[0].StringValue;
		var country = row[1].StringValue;
		var type = row[2].StringValue;
		var channel = row[3].StringValue;
		var priority = row[4].StringValue;
		var orderDate = row[5].DateTimeValue;
		var id = row[6].IntValue;
		var shipDate = row[7].DateTimeValue;
		var unitsSold = row[8].IntValue;
		var unitPrice = row[9].DoubleValue;
		var unitCost = row[10].DoubleValue;
		var totalRevenue = row[11].DoubleValue;
		var totalCost = row[12].DoubleValue;
		var totalProfit = row[13].DoubleValue;
	}

	[Benchmark]
	public void AsposeXlsb()
	{
		var wb = new Workbook(file);
		foreach (var ws in wb.Worksheets)
		{
			var cells = ws.Cells;
			var rowCount = cells.GetLastDataRow(0);
			bool header = true;
			foreach (Row row in cells.Rows)
			{
				if (header)
				{
					header = false;
					continue;
				}
				ProcessAsposeRecord(row);
			}
		}
	}

	[Benchmark]
	public void PrimeXlsb()
	{
		using Excel_PRIMEXlsb workbook = new();
		workbook.Open(file);
		foreach (string sheetName in workbook.SheetNames())
		{
			using var worksheet = workbook.GetSheet(sheetName);
			foreach (var row in worksheet!.GetRowData(1, RowCellGet.PreGet))// skip header row
			{
				if (row == null)
				{   // Because this returns upto the dimension of the sheet Height
					break;
				}

				var cells = row.GetAllCells();
				var region = cells[0].CellValue.ToString();
				var country = cells[1].CellValue.ToString();
				var type = cells[2].CellValue.ToString();
				var channel = cells[3].CellValue.ToString();
				var priority = cells[4].CellValue.ToString();
				var orderDate = cells[5].CellValue.AsDateTime;
				var id = cells[6].CellValue.AsInt32;
				var shipDate = cells[7].CellValue.AsDateTime;
				var unitsSold = cells[8].CellValue.AsInt32;
				var unitPrice = cells[9].CellValue.AsDecimal;
				var unitCost = cells[10].CellValue.AsDecimal;
				var totalRevenue = cells[11].CellValue.AsDecimal;
				var totalCost = cells[12].CellValue.AsDecimal;
				var totalProfit = cells[13].CellValue.AsDecimal;
				row.Dispose();
			}
		}
	}

	[Benchmark]
	public void PrimeXlsbObj()
	{
		using Excel_PRIMEXlsb workbook = new();
		workbook.Open(file);
		foreach (string sheetName in workbook.SheetNames())
		{
			using var worksheet = workbook.GetSheet(sheetName);
			var values = new object?[worksheet.SheetDimensions.Width];
			foreach (var row in worksheet!.GetRowData(1, RowCellGet.PreGet)) // skip header row
			{
				if (row == null)
				{
					// Because this returns upto the dimension of the sheet Height
					break;
				}

				row.CopyBoxedToArray(values);
				row.Dispose();
			}
		}
	}
}
