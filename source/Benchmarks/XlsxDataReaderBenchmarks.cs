using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using MiniExcelLibs;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;

namespace Benchmarks
{
	[MemoryDiagnoser]
	public class XlsxBenchmarks
	{
		const string file = @"Data/65K_Records_Data.xlsx";

		public XlsxBenchmarks()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}

		static void ProcessRecord(IDataReader reader)
		{
			var region = reader.GetString(0);
			var country = reader.GetString(1);
			var type = reader.GetString(2);
			var channel = reader.GetString(3);
			var priority = reader.GetString(4);
			var orderDate = reader.GetDateTime(5);
			var id = reader.GetInt32(6);
			var shipDate = reader.GetDateTime(7);
			var unitsSold = reader.GetInt32(8);
			var unitPrice = reader.GetDouble(9);
			var unitCost = reader.GetDouble(10);
			var totalRevenue = reader.GetDouble(11);
			var totalCost = reader.GetDouble(12);
			var totalProfit = reader.GetDouble(13);
		}

		static void ProcessRecordEDR(IDataReader reader)
		{
			var region = reader.GetString(0);
			var country = reader.GetString(1);
			var type = reader.GetString(2);
			var channel = reader.GetString(3);
			var priority = reader.GetString(4);
			var orderDate = reader.GetDateTime(5);
			// ExcelDataReader doesn't allow reading as integers
			var id = reader.GetDouble(6);
			var shipDate = reader.GetDateTime(7);
			var unitsSold = reader.GetDouble(8);
			var unitPrice = reader.GetDouble(9);
			var unitCost = reader.GetDouble(10);
			var totalRevenue = reader.GetDouble(11);
			var totalCost = reader.GetDouble(12);
			var totalProfit = reader.GetDouble(13);		
		}

		[Benchmark]
		public void SylvanXlsx()
		{
			var reader = Sylvan.Data.Excel.ExcelDataReader.Create(file);

			do
			{
				while (reader.Read())
				{
					ProcessRecord(reader);
				}

			} while (reader.NextResult());
		}

		//[Benchmark]
		[SupportedOSPlatform("windows")]
		public void AceOleDbXls()
		{
			AceOleDb.ProcessFile(file);
		}

		//[Benchmark]
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
						ProcessRecordEDR(reader);
					}
				} while (reader.NextResult());
			}
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
				if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
				for (int j = row.FirstCellNum; j < cellCount; j++)
				{
					var cell = row.GetCell(j);
					if (cell == null || cell.CellType == CellType.Blank)
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
		public void AsposeXlsx()
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
				var orderId = (double)d["Order ID"];
				var shipDate = (DateTime)d["Ship Date"];
				var unitsSold = (double)d["Units Sold"];
				var unitPrice = (double)d["Unit Price"];
				var unitCost = (double)d["Unit Cost"];
				var totalRevenue = (double)d["Total Revenue"];
				var totalCost = (double)d["Total Cost"];
				var totalProfit = (double)d["Total Profit"];
			}
		}
	}
}
