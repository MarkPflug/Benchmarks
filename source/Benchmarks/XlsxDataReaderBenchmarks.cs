using BenchmarkDotNet.Attributes;
using CommandLine;
using MiniExcelLibs;
using NanoXLSX.LowLevel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Xml;

namespace Benchmarks;

[MemoryDiagnoser]
[HideColumns("StdDev", "RatioSD", "Gen0", "Gen1", "Gen2")]
public class XlsxReaderBenchmarks
{
	const string file = @"Data/65K_Records_Data.xlsx";

	public XlsxReaderBenchmarks()
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	}

	// Measures a baseline of decompressing and reading xml.
	// Nothing can go faster than this without implementing
	// a custom decompressor or xml reader.
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
	public void SylvanXlsx()
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

	// For some reason the ACE driver leaves some thread spinning in the process
	// which alone is terrible, but also affects the results of subsequent benchmarks
	[Benchmark]
	[SupportedOSPlatform("windows")]
	public void AceOleDbXls()
	{
		AceOleDb.ProcessFile(file);
	}

	[Benchmark]
	public void ExcelDataReaderXlsx()
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

	[Benchmark]
	public void XlsxHelperXlsx ()
	{
		using var stream = File.OpenRead(file);
		using (var book = XlsxHelper.XlsxReader.OpenWorkbook(stream))
		{
			foreach (var sheet in book.Worksheets) {
				using var reader = sheet.WorksheetReader;
				int i = 0;
				foreach (var row in reader)
				{
					i++;
					// skip header row
					if (i == 1) continue;
					ProcessXlsxHelperRecord(row);
				}
			}
		}
	}

	static void ProcessXlsxHelperRecord(XlsxHelper.Row r)
	{
		var row = r.Cells;
		var region = row[0].CellValue;
		var country = row[1].CellValue;
		var type = row[2].CellValue;
		var channel = row[3].CellValue;
		var priority = row[4].CellValue;
		var orderDate = row[5].GetDateTime();
		var id = row[6].GetInt32();
		var shipDate = row[7].GetDateTime();
		var unitsSold = row[8].GetInt32();
		var unitPrice = row[9].GetDecimal();
		var unitCost = row[10].GetDecimal();
		var totalRevenue = row[11].GetDecimal();
		var totalCost = row[12].GetDecimal();
		var totalProfit = row[13].GetDecimal();
	}

	[Benchmark]
	public void FastExcelXlsx()
	{
		using var fastExcel = new FastExcel.FastExcel(new FileInfo(file), true);
		// Read the rows using worksheet name
		var worksheet = fastExcel.Worksheets.First();
		worksheet.Read();
		foreach (var row in worksheet.Rows)
		{
			foreach (var cell in row.Cells)
			{
				var val = cell.Value;
			}
		}
	}

	[Benchmark]
	public void NpoiXlsx()
	{
		var wb = new XSSFWorkbook(file);
		var sheet = wb.GetSheetAt(0);
		IRow headerRow = sheet.GetRow(0);
		int cellCount = headerRow.LastCellNum;
		for (int j = 0; j < cellCount; j++)
		{
			ICell cell = headerRow.GetCell(j);
			var str = cell.ToString();
		}
		for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
		{
			IRow row = sheet.GetRow(i);
			if (row == null) continue;
			if (row.Cells.All(d => d.CellType == NPOI.SS.UserModel.CellType.Blank)) continue;


			for (int j = row.FirstCellNum; j < cellCount; j++)
			{
				var cell = row.GetCell(j);
				if (cell == null || cell.CellType == NPOI.SS.UserModel.CellType.Blank)
					continue;
				try
				{
					var str = cell?.ToString();
				}
				catch (Exception) { }
			}
		}
	}

	[Benchmark]
	public void EPPlusXlsx()
	{
		var pkg = new ExcelPackage(new FileInfo(file));
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		var workbook = pkg.Workbook;
		foreach (var worksheet in workbook.Worksheets)
		{
			var r = 0;
			var dim = worksheet.Dimension;
			if (dim == null) continue;
			var data = worksheet.Cells;
			var rows = dim.Rows;
			var cols = dim.Columns;
			for (r = 1; r < rows; r++)
			{
				for (int c = 1; c < cols; c++)
				{
					var val = data[r, c].Value;
				}
			}
		}
	}

	[Benchmark]
	public void ClosedXmlXlsx()
	{
		var pkg = new ClosedXML.Excel.XLWorkbook(file);
		var ws = pkg.Worksheets.First();

		var rc = ushort.MaxValue;
		var cc = 14;
		for (int r = 1; r <= rc; r++)
		{
			var row = ws.Row(r);
			for (int i = 1; i <= cc; i++)
			{
				var cell = row.Cell(i);
				var value = cell.Value;
			}
		}
	}

	static void ProcessAsposeRecord(Aspose.Cells.Row row)
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
		var unitPrice = (decimal)row[9].DoubleValue;
		var unitCost = (decimal)row[10].DoubleValue;
		var totalRevenue = (decimal)row[11].DoubleValue;
		var totalCost = (decimal)row[12].DoubleValue;
		var totalProfit = (decimal)row[13].DoubleValue;
	}

	[Benchmark]
	public void AsposeXlsx()
	{
		var wb = new Aspose.Cells.Workbook(file);
		foreach (var ws in wb.Worksheets)
		{
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
				ProcessAsposeRecord(row);
			}
		}
	}

	[Benchmark]
	public void OpenXmlXlsx()
	{
		var s = File.OpenRead(file);
		var pkg = Package.Open(s);
		var doc = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Open(pkg);
		foreach (var wsp in doc.WorkbookPart.WorksheetParts)
		{
			var sd = wsp.Worksheet.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.SheetData>();
			foreach (DocumentFormat.OpenXml.Spreadsheet.Row r in sd)
			{
				foreach (DocumentFormat.OpenXml.Spreadsheet.Cell c in r)
				{
					var v = c.CellValue;
				}
			}
		}
	}

	[Benchmark]
	public void MiniExcelXlsx()
	{
		using var s = File.OpenRead(file);
		var rows = s.Query(true);
		foreach (ExpandoObject row in rows)
		{
			var d = (IDictionary<string, object>)row;
			var region = (string)d["Region"];
			var country = (string)d["Country"];
			var type = (string)d["Item Type"];
			var channel = (string)d["Sales Channel"];
			var priority = (string)d["Order Priority"];
			var orderDate = (DateTime)d["Order Date"];
			var orderId = (int)(double)d["Order ID"];
			var shipDate = (DateTime)d["Ship Date"];
			var unitsSold = (int)(double)d["Units Sold"];
			var unitPrice = (decimal)(double)d["Unit Price"];
			var unitCost = (decimal)(double)d["Unit Cost"];
			var totalRevenue = (decimal)(double)d["Total Revenue"];
			var totalCost = (decimal)(double)d["Total Cost"];
			var totalProfit = (decimal)(double)d["Total Profit"];
		}
	}
}
