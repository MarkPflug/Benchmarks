using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MiniExcelLibs;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using Sylvan.Data;
using Sylvan.Data.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Runtime.CompilerServices;

namespace Benchmarks;

[MemoryDiagnoser]
public class ExcelWriterBenchmarks
{
	const string file = @"Data/65K_Records_Data.csv";

	MemoryStream ms;

	DataTable dt;

	public ExcelWriterBenchmarks()
	{
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		this.ms = new MemoryStream(10 * 0x100000); // 10mb
		this.dt = new DataTable();
		var r = GetData();
		var s = r.GetColumnSchema();
		try
		{
			dt.Load(GetData());
		}
		catch
		{
			var errors = dt.GetErrors();
			var rr = dt.Rows.Count;
			Console.WriteLine(rr.ToString());
		}
	}

	Stream GetStream([CallerMemberName] string name = null)
	{
		return File.Create(name + ".xlsx");
	}

	DbDataReader GetData()
	{
		return TestData.GetData();
	}

	[Benchmark]
	public void SylvanXlsxObj()
	{
		WriteSylvan(TestData.GetRecords());
	}

	void WriteSylvan<T>(IEnumerable<T> data)
		where T : class
	{
		using var ns = GetStream();
		using (var xw = ExcelDataWriter.Create(ns, ExcelWorkbookType.ExcelXml))
		{
			xw.Write(data.AsDataReader());
		}
	}

	[Benchmark]
	public void SylvanXlsx()
	{
		WriteSylvan(GetData());
	}

	void WriteSylvan(DbDataReader reader)
	{
		using var ns = GetStream();
		using (var xw = ExcelDataWriter.Create(ns, ExcelWorkbookType.ExcelXml))
		{
			xw.Write(reader);
		}
	}

	[Benchmark]
	public void SylvanXlsb()
	{
		WriteSylvanXlsb(GetData());
	}

	void WriteSylvanXlsb(DbDataReader reader)
	{
		using var ns = GetStream();
		using (var xw = ExcelDataWriter.Create(ns, ExcelWorkbookType.ExcelBinary))
		{
			xw.Write(reader);
		}
	}


	[Benchmark]
	public void LargeXlsx()
	{
		using var ns = GetStream();
		WriteLargeXlsx(GetData(), ns);
	}

	internal void WriteLargeXlsx(DbDataReader reader, Stream ns)
	{
		using var xw = new LargeXlsx.XlsxWriter(ns);
		xw.BeginWorksheet("sheet1");

		// write header
		xw.BeginRow();
		for (int i = 0; i < reader.FieldCount; i++)
		{
			xw.Write(reader.GetName(i));
		}

		while (reader.Read())
		{
			xw.BeginRow();
			for (int i = 0; i < reader.FieldCount; i++)
			{
				var t = reader.GetFieldType(i);
				var tc = Type.GetTypeCode(t);
				switch (tc)
				{
					case TypeCode.Boolean:
						xw.Write(reader.GetBoolean(i));
						break;
					case TypeCode.Int32:
						xw.Write(reader.GetInt32(i));
						break;
					case TypeCode.Decimal:
						xw.Write(reader.GetDecimal(i));
						break;
					case TypeCode.Double:
						xw.Write(reader.GetDouble(i));
						break;
					case TypeCode.String:
						xw.WriteSharedString(reader.GetString(i));
						break;
					case TypeCode.DateTime:
						xw.Write(reader.GetDateTime(i));
						break;
					default:
						throw new NotSupportedException();
				}
			}
		}
	}

	[Benchmark]
	public void NanoXlsxWrite()
	{
		WriteNano(GetData());
	}

	void WriteNano(DbDataReader reader)
	{
		using var ns = GetStream();
		// TODO: I don't see a way to write to an in-memory stream. Shouldn't affect the perf much though.
		var wb = new NanoXLSX.Workbook(true);
		while (reader.Read())
		{
			for (int i = 0; i < reader.FieldCount; i++)
			{
				wb.WS.Value(reader.GetValue(i));
			}
			wb.WS.Down();
		}
		wb.SaveAsStream(ns);
	}

	[Benchmark]
	public void NpoiXlsx()
	{
		WriteNPOI(GetData());
	}

	void WriteNPOI(DbDataReader reader)
	{
		using var ns = GetStream();
		IWorkbook workbook = new XSSFWorkbook();
		ISheet excelSheet = workbook.CreateSheet("Sheet1");

		var columns = new List<string>();
		IRow row = excelSheet.CreateRow(0);
		int columnIndex = 0;


		for (int i = 0; i < reader.FieldCount; i++)
		{
			var name = reader.GetName(i);
			columns.Add(name);
			row.CreateCell(columnIndex).SetCellValue(name);
			columnIndex++;
		}

		int rowIndex = 1;
		while (reader.Read())
		{
			row = excelSheet.CreateRow(rowIndex);
			for (int i = 0; i < reader.FieldCount; i++)
			{
				var cell = row.CreateCell(i);
				var t = reader.GetFieldType(i);
				var c = Type.GetTypeCode(t);
				switch (c)
				{
					case TypeCode.DateTime:
						cell.SetCellValue(reader.GetDateTime(i));
						break;
					case TypeCode.Int32:
						cell.SetCellValue(reader.GetInt32(i));
						break;
					case TypeCode.Double:
						cell.SetCellValue(reader.GetDouble(i));
						break;
					case TypeCode.String:
						cell.SetCellValue(reader.GetString(i));
						break;
					case TypeCode.Decimal:
						cell.SetCellValue((double)reader.GetDecimal(i));
						break;
					default:
						throw new Exception();
				}
			}

			rowIndex++;
		}
		workbook.Write(ns, false);
		workbook.Close();
	}

	[Benchmark]
	public void EPPlusXlsx()
	{
		WriteEPPlus(GetData());
	}

	void WriteEPPlus(DbDataReader reader)
	{
		using var ns = GetStream();
		using (var pkg = new ExcelPackage(ns))
		{
			var ws = pkg.Workbook.Worksheets.Add("data");
			var rowIdx = 1;
			while (reader.Read())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					var cell = ws.Cells[rowIdx, i + 1];
					cell.Value = reader.GetValue(i);
				}

				rowIdx++;
			}
			pkg.Save();
		}
	}

	[Benchmark]
	public void EPPlusViaDataReader()
	{
		WriteEPPlusViaDataReader(GetData());
	}

	void WriteEPPlusViaDataReader(DbDataReader reader)
	{
		using var ns = GetStream();
		using (var package = new ExcelPackage(ns))
		{
			var sheet = package.Workbook.Worksheets.Add("TestSheet");
			sheet.Cells["A1"].LoadFromDataReader(reader, true);
			package.Save();
		}
	}

	[Benchmark]
	public void AsposeXlsx()
	{
		WriteAspose(GetData());
	}

	void WriteAspose(DbDataReader reader)
	{
		var w = new Aspose.Cells.Workbook();
		var s = w.Worksheets[0];
		var sc = s.Cells;

		var rowIdx = 1;
		while (reader.Read())
		{
			for (int i = 0; i < reader.FieldCount; i++)
			{
				var t = reader.GetFieldType(i);
				var c = Type.GetTypeCode(t);
				switch (c)
				{
					case TypeCode.String:
						sc[rowIdx, i].PutValue(reader.GetString(i));
						break;
					case TypeCode.Double:
						sc[rowIdx, i].PutValue(reader.GetDouble(i));
						break;
					case TypeCode.Int32:
						sc[rowIdx, i].PutValue(reader.GetInt32(i));
						break;
					case TypeCode.DateTime:
						sc[rowIdx, i].PutValue(reader.GetDateTime(i));
						break;
					case TypeCode.Decimal:
						sc[rowIdx, i].PutValue((double)reader.GetDecimal(i));
						break;
					default:
						throw new NotSupportedException();
				}
			}

			rowIdx++;
		}

		using var ns = GetStream();
		w.Save(ns, SaveFormat.Xlsx);
	}

	[Benchmark]
	public void SwiftExcelXlsx()
	{
		var reader = GetData();

		using (var ew = new SwiftExcel.ExcelWriter("swift.xlsx"))
		{
			var row = 1;
			while (reader.Read())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					var t = reader.GetFieldType(i);
					var tc = Type.GetTypeCode(t);
					var c = i + 1;
					SwiftExcel.DataType dt;
					switch (tc)
					{
						case TypeCode.Int32:
						case TypeCode.Decimal:
						case TypeCode.Double:
							dt = SwiftExcel.DataType.Number;
							break;
						case TypeCode.String:
						case TypeCode.Boolean:
						case TypeCode.DateTime:
							dt = SwiftExcel.DataType.Text;
							break;
						default:
							throw new NotSupportedException();
					}
					ew.Write(reader.GetString(i), c, row, dt);
				}
				row++;
			}
			ew.Save();
		}
	}

	//[Benchmark]
	public void SpreadsheetLightXlsx()
	{
		var reader = GetData();

		using (var sld = new SpreadsheetLight.SLDocument())
		{
			sld.ImportDataTable("A1", this.dt, true);
			sld.SaveAs("SL.xlsx");
		}
	}

	[Benchmark]
	public void MiniXl()
	{
		using var stream = new MemoryStream();
		stream.SaveAs(TestData.GetRecords());
	}

	[Benchmark]
	public void AsposeXlsb()
	{
		WriteAsposeXlsb(GetData());
	}

	void WriteAsposeXlsb(DbDataReader reader)
	{
		var w = new Aspose.Cells.Workbook();
		var s = w.Worksheets[0];
		var sc = s.Cells;

		var rowIdx = 1;
		while (reader.Read())
		{
			for (int i = 0; i < reader.FieldCount; i++)
			{
				var t = reader.GetFieldType(i);
				var c = Type.GetTypeCode(t);
				switch (c)
				{
					case TypeCode.String:
						sc[rowIdx, i].PutValue(reader.GetString(i));
						break;
					case TypeCode.Double:
						sc[rowIdx, i].PutValue(reader.GetDouble(i));
						break;
					case TypeCode.Int32:
						sc[rowIdx, i].PutValue(reader.GetInt32(i));
						break;
					case TypeCode.DateTime:
						sc[rowIdx, i].PutValue(reader.GetDateTime(i));
						break;
					case TypeCode.Decimal:
						sc[rowIdx, i].PutValue((double)reader.GetDecimal(i));
						break;
					default:
						throw new NotSupportedException();
				}
			}

			rowIdx++;
		}

		using var ns = GetStream();
		w.Save(ns, SaveFormat.Xlsb);
	}

	[Benchmark]
	public void OpenXmlXlsx()
	{
		WriteOpenXml(GetData());
	}

	void WriteOpenXml(DbDataReader reader)
	{
		using var ns = GetStream();

		using (var spreadSheet = SpreadsheetDocument.Create(ns, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
		{
			var wbp = spreadSheet.AddWorkbookPart();
			wbp.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

			WorksheetPart worksheetPart = wbp.AddNewPart<WorksheetPart>();
			var sheetData = new SheetData();
			worksheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

			Sheets sheets = wbp.Workbook.AppendChild(new Sheets());
			Sheet sheet = new Sheet() { Id = wbp.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Test Sheet" };

			var rowIndex = 1u;
			while (reader.Read())
			{
				var row = new DocumentFormat.OpenXml.Spreadsheet.Row() { RowIndex = rowIndex };

				for (int i = 0; i < reader.FieldCount; i++)
				{
					var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();

					var t = reader.GetFieldType(i);
					var c = Type.GetTypeCode(t);

					if (reader.IsDBNull(i))
					{
					}
					else
					{
						switch (c)
						{
							case TypeCode.DateTime:
								cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(reader.GetDateTime(i));
								cell.DataType = CellValues.Date;
								break;
							case TypeCode.Int32:
								cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(reader.GetInt32(i));
								cell.DataType = CellValues.Number;
								break;
							case TypeCode.String:
								cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(reader.GetString(i));
								cell.DataType = CellValues.SharedString;
								break;
							case TypeCode.Decimal:
								cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(reader.GetDecimal(i));
								cell.DataType = CellValues.Number;
								break;
							case TypeCode.Double:
								cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(reader.GetDouble(i));
								cell.DataType = CellValues.Number;
								break;
							default:
								throw new NotSupportedException();
						}
					}
					row.Append(cell);
				}
				sheetData.Append(row);

				rowIndex++;
			}

			spreadSheet.WorkbookPart.Workbook.Save();
		}
	}
}
