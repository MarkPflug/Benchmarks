using BenchmarkDotNet.Attributes;
using ExcelPRIME;
using System.IO;
using System.Linq;
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
		while (reader.Read())
		{
			reader.ProcessSalesRecord();
		}
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
			reader.Read();//skip header
			while (reader.Read())
			{
				reader.ProcessSalesRecordEDR();
			}
		}
	}


	[Benchmark]
	public void AsposeXlsb()
	{
		var wb = new Aspose.Cells.Workbook(file);
		var ws = wb.Worksheets.First();
		var cells = ws.Cells;
		var rowCount = cells.GetLastDataRow(0);
		bool header = true;
		foreach (Aspose.Cells.Row row in cells.Rows)
		{
			if (header)
			{
				header = false;
				continue;
			}
			row.ProcessAsposeRecord();
		}
	}

	[Benchmark]
	public void PrimeXlsb()
	{
		using Excel_PRIMEXlsb workbook = new();
		workbook.Open(file);
		var sheetName = workbook.SheetNames().First();
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
			// AsDecimal gives correct precision here, were it doesn't with xlsx
			var unitPrice = cells[9].CellValue.AsDecimal;
			var unitCost = cells[10].CellValue.AsDecimal;
			var totalRevenue = cells[11].CellValue.AsDecimal;
			var totalCost = cells[12].CellValue.AsDecimal;
			var totalProfit = cells[13].CellValue.AsDecimal;
			row.Dispose();
		}
	}

	//[Benchmark]
	public void PrimeXlsbObj()
	{
		// this provides the "raw" values from the xlsb, 
		// which don't match what you'd get with the same
		// code from an xlsx file, since xlsb stores values
		// in native binary format and xlsx always as strings.
		// I think Excel_PRIME is still a WIP, so maybe
		// these will be re-enabled at some point.
		using Excel_PRIMEXlsb workbook = new();
		workbook.Open(file);
		var sheetName = workbook.SheetNames().First();
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
