using BenchmarkDotNet.Attributes;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelPRIME;
using MiniExcelLibs;
using OfficeOpenXml;
using Sylvan.Data.Excel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Xml;
using CellValue = DocumentFormat.OpenXml.Spreadsheet.CellValue;

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
		while (reader.Read())
		{
			reader.ProcessSalesRecord();
		}
	}

	[Benchmark]
	public void SylvanXlsx_BindT()
	{
		var reader = Sylvan.Data.Excel.ExcelDataReader.Create(file);
		// fully bind to the SalesRecord objects
		foreach(var record in reader.GetRecords<SalesRecord>())
		{
			// enumerate the bound records
			// without the enumeration, no work is done.
		}
	}

	[Benchmark]
	public void SylvanXlsxDynamic()
	{
		var o = new ExcelDataReaderOptions { Schema = ExcelSchema.Dynamic };
		using var reader = Sylvan.Data.Excel.ExcelDataReader.Create(file, o);

		var values = new object[reader.FieldCount];
		while (reader.Read())
		{
			// the dynamic schema will cause cells to be read
			// as the most "intuitive" type for their value
			// this means that numeric values might be int or double
			// depending on whether they have fractional components
			// the values array will contain boxed values of the
			// intuited type
			// this is useful when the data has no tabular schema
			// and might vary from row to row
			reader.GetValues(values);
		}
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
			reader.Read();//skip header
			while (reader.Read())
			{
				reader.ProcessSalesRecordEDR();
			}
		}
	}

	[Benchmark]
	public void XlsxHelperXlsx()
	{
		using var stream = File.OpenRead(file);
		using (var book = XlsxHelper.XlsxReader.OpenWorkbook(stream))
		{
			var sheet = book.Worksheets.First();
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

		// can't use GetDecimal(), returns incorrect precision

		var unitPrice = (decimal)row[9].GetDouble();
		var unitCost = (decimal)row[10].GetDouble();
		var totalRevenue = (decimal)row[11].GetDouble();
		var totalCost = (decimal)row[12].GetDouble();
		var totalProfit = (decimal)row[13].GetDouble();
	}

	[Benchmark]
	public void FastExcelXlsx()
	{
		// neither fast, nor easy to use.
		using var fastExcel = new FastExcel.FastExcel(new FileInfo(file), true);
		// Read the rows using worksheet name
		var worksheet = fastExcel.Worksheets.First();
		
		// this accepts an "existingHeadingRows" argument
		// but I don't understand what it does. Enumerating the rows
		// still returns the headers.
		worksheet.Read();
		// skip header
		bool first = true;
		foreach (var row in worksheet.Rows)
		{
			if (first)
			{
				first = false;
				continue;
			}
			
			// this is absurd, having to manually enumerate the cells
			// since there is no indexer.
			using var e = row.Cells.GetEnumerator();
			e.MoveNext();
			var region = e.Current.Value;
			e.MoveNext();
			var country = e.Current.Value;
			e.MoveNext();
			var type = e.Current.Value;
			e.MoveNext();
			var channel = e.Current.Value;
			e.MoveNext();
			var priority = e.Current.Value;
			e.MoveNext();
			// this might be even more absurd. "Value" is of type `object`, but
			// always contains a string. derp
			// IMO, any Excel library that requires the user to understand what an Ole Automation Date is, has failed the user.
			var orderDate = DateTime.FromOADate(double.Parse((string)e.Current.Value));

			e.MoveNext();
			var id = int.Parse((string)e.Current.Value);
			e.MoveNext();
			var shipDate = DateTime.FromOADate(double.Parse((string)e.Current.Value));

			e.MoveNext();
			var unitsSold = int.Parse((string)e.Current.Value);

			e.MoveNext();
			var unitPrice = (decimal)double.Parse((string)e.Current.Value);
			e.MoveNext();
			var unitCost = (decimal)double.Parse((string)e.Current.Value);
			e.MoveNext();
			var totalRevenue = (decimal)double.Parse((string)e.Current.Value);
			e.MoveNext();
			var totalCost = (decimal)double.Parse((string)e.Current.Value);
			e.MoveNext();
			var totalProfit = (decimal)double.Parse((string)e.Current.Value);
		}
	}

	[Benchmark]
	public void NpoiXlsx()
	{
		using var wb = new NPOI.XSSF.UserModel.XSSFWorkbook(file, true);
		wb.ProcessSalesDataNpoi();
	}

	[Benchmark]
	public void EPPlusXlsx()
	{
		var pkg = new ExcelPackage(new FileInfo(file));
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		var workbook = pkg.Workbook;
		var worksheet = workbook.Worksheets.First();
		var r = 0;
		var dim = worksheet.Dimension;
		var data = worksheet.Cells;
		var rows = dim.Rows;
		var cols = dim.Columns;
		// start at 2 to skip header row
		for (r = 2; r < rows; r++)
		{
			var region = (string)data[r, 1].Value;
			var country = (string)data[r, 2].Value;
			var type = (string)data[r, 3].Value;
			var channel = (string)data[r, 4].Value;
			var priority = (string)data[r, 5].Value;

			// this is actually sane, amazing
			var orderDate = (DateTime)data[r, 6].Value;
			var id = (int)(double)data[r, 7].Value;
			var shipDate = (DateTime)data[r, 8].Value;
			var unitsSold = (int)(double)data[r, 9].Value;
			var unitPrice = (decimal)(double)data[r, 10].Value;
			var unitCost = (decimal)(double)data[r, 11].Value;
			var totalRevenue = (decimal)(double)data[r, 12].Value;
			var totalCost = (decimal)(double)data[r, 13].Value;
			var totalProfit = (decimal)(double)data[r, 14].Value;
		}
	}

	[Benchmark]
	public void ClosedXmlXlsx()
	{
		var pkg = new ClosedXML.Excel.XLWorkbook(file);
		var ws = pkg.Worksheets.First();

		var rc = ushort.MaxValue;
		// start at 2 to skip header row
		for (int r = 2; r <= rc; r++)
		{
			var row = ws.Row(r);

			var region = row.Cell(1).Value.GetText();
			var country = row.Cell(2).Value.GetText();
			var type = row.Cell(3).Value.GetText();
			var channel = row.Cell(4).Value.GetText();
			var priority = row.Cell(5).Value.GetText();
			var orderDate = row.Cell(6).Value.GetDateTime();
			var id = (int)row.Cell(7).Value.GetNumber();
			var shipDate = row.Cell(8).Value.GetDateTime();
			var unitsSold = (int)row.Cell(9).Value.GetNumber();
			var unitPrice = (decimal)row.Cell(10).Value.GetNumber();
			var unitCost = (decimal)row.Cell(11).Value.GetNumber();
			var totalRevenue = (decimal)row.Cell(12).Value.GetNumber();
			var totalCost = (decimal)row.Cell(13).Value.GetNumber();
			var totalProfit = (decimal)row.Cell(14).Value.GetNumber();
		}
	}

	

	[Benchmark]
	public void AsposeXlsx()
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
	public void OpenXmlXlsx()
	{
		using SpreadsheetDocument doc = SpreadsheetDocument.Open(file, false);
		WorkbookPart workbookPart = doc.WorkbookPart;
		Sheet firstSheet = workbookPart.Workbook.Sheets.GetFirstChild<Sheet>();

		if (firstSheet == null)
			throw new Exception("No sheets found in Excel file.");

		WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(firstSheet.Id);
		SharedStringTablePart ssp = workbookPart.SharedStringTablePart;
		var sharedStrings = ssp?.SharedStringTable;

		var rows = worksheetPart.Worksheet.Descendants<Row>();

		foreach (var row in rows)
		{
			foreach (var cell in row.Elements<Cell>())
			{ 
				//it would be faster not to calculate column index from cellreference attribute 
				var cellvalue = GetCellValue(cell, sharedStrings);
			}
		}
	}

	static object GetCellValue(Cell cell, SharedStringTable sharedStrings)
	{
		if (cell.DataType?.Value == CellValues.SharedString)
		{
			if (cell.CellValue == null)
				return null;
			if (int.TryParse(cell.CellValue.InnerText, out int index) && sharedStrings != null)
			{
				return sharedStrings.ElementAt(index).InnerText;
			}
		}else if (cell.DataType?.Value == CellValues.Number)
		{
			if (cell.CellValue == null)
				return 0;
			return double.Parse(cell.CellValue.InnerText);
		}else if (cell.DataType?.Value == CellValues.Date)
		{
			if (cell.CellValue == null)
				return default(DateTime);
			return DateTime.FromOADate(double.Parse(cell.CellValue.InnerText));	
			//simplified conversion from Excel date to .NET date. It is simpler than NPOI's DateUtil.GetJavaDate, for example, 1900 leap year compensation, 1904 windowing
		}
		return null;
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


	[Benchmark]
	public void PrimeXlsx()
	{
		using Excel_PRIME workbook = new();
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
			// can't use AsDecimal, it returns the wrong precision
			var unitPrice = (decimal)cells[9].CellValue.AsDouble;
			var unitCost = (decimal)cells[10].CellValue.AsDouble;
			var totalRevenue = (decimal)cells[11].CellValue.AsDouble;
			var totalCost = (decimal)cells[12].CellValue.AsDouble;
			var totalProfit = (decimal)cells[13].CellValue.AsDouble;
			row.Dispose();
		}
	}

	//[Benchmark]
	public void PrimeXlsxObj()
	{
		// this isn't useful. It fills the values array with the raw strings from the xml
		// meaning that dates come through as the ole automation double value as a string
		// just pure trash.
		using Excel_PRIME workbook = new();
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

	[Benchmark]
	public void HypeLabXlsx_SheetData()
	{
		var data = HypeLab.IO.Excel.ExcelReader.ExtractSheetData(file);

		foreach (var row in data.Rows)
		{
			var region = row[0];
			var country = row[1];
			var type = row[2];
			var channel = row[3];
			var priority = row[4];
			var orderDate = DateTime.FromOADate(double.Parse(row[5]));
			var id = int.Parse(row[6]);
			var shipDate = DateTime.FromOADate(double.Parse(row[7]));
			var unitsSold = int.Parse(row[8]);
			// double.parse then cast to decimal is
			// required to get the correct value, otherwise
			// the precision will be wrong. You cannot just decimal.Parse.
			var unitPrice = (decimal)double.Parse(row[9]);
			var unitCost = (decimal)double.Parse(row[10]);
			var totalRevenue = (decimal)double.Parse(row[11]);
			var totalCost = (decimal)double.Parse(row[12]);
			var totalProfit = (decimal)double.Parse(row[13]);
		}
	}

	//[Benchmark]
	public void HypeLabXlsx_BindT()
	{
		var data = HypeLab.IO.Excel.ExcelReader.ExtractSheetData(file);
		// this doesn't work. It returns partially bound objects with only
		// the string properties assigned.
		var records = HypeLab.IO.Excel.ExcelParser.ParseTo<SalesRecord>(data);
	}


	[Benchmark]
	public void Lightweight()
	{
		var reader = new LightWeightExcelReader.ExcelReader(file);
		var sheet = reader[0];

		// skip the header row
		sheet.ReadNext();
		while (sheet.ReadNextInRow()) ;

		// read the data
		while (sheet.ReadNext())
		{
			var region = (string)sheet.Value;
			sheet.ReadNextInRow();
			var country = (string)sheet.Value;
			sheet.ReadNextInRow();
			var type = (string)sheet.Value;
			sheet.ReadNextInRow();
			var channel = (string)sheet.Value;
			sheet.ReadNextInRow();
			var priority = (string)sheet.Value;
			sheet.ReadNextInRow();
			var orderDate = (DateTime)sheet.Value;
			sheet.ReadNextInRow();
			var id = (int)(double)sheet.Value;
			sheet.ReadNextInRow();
			var shipDate = (DateTime)sheet.Value;
			sheet.ReadNextInRow();
			var unitsSold = (int)(double)sheet.Value;
			sheet.ReadNextInRow();
			var unitPrice = (decimal)(double)sheet.Value;
			sheet.ReadNextInRow();
			var unitCost = (decimal)(double)sheet.Value;
			sheet.ReadNextInRow();
			var totalRevenue = (decimal)(double)sheet.Value;
			sheet.ReadNextInRow();
			var totalCost = (decimal)(double)sheet.Value;
			sheet.ReadNextInRow();
			var totalProfit = (decimal)(double)sheet.Value;
		}
	}


}
