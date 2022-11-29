using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using Sylvan.Data.Excel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Runtime.CompilerServices;

namespace Benchmarks;

sealed class NoCloseStream : Stream
{
	Stream inner;
	public NoCloseStream(Stream inner)
	{
		this.inner = inner;
	}

	public override bool CanRead => this.inner.CanRead;

	public override bool CanSeek => this.inner.CanSeek;

	public override bool CanWrite => this.inner.CanWrite;

	public override long Length => this.inner.Length;

	public override long Position { get => this.inner.Position; set => this.inner.Position = value; }

	public override void Flush()
	{
		this.inner.Flush();
	}

	public override void Close()
	{
		// NOPE
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return this.inner.Read(buffer, offset, count);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return this.inner.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		this.inner.SetLength(value);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		this.inner.Write(buffer, offset, count);
	}
}

[MemoryDiagnoser]
public class XlsxWriterBenchmarks
{
	const string file = @"Data/65K_Records_Data.csv";

	MemoryStream ms;

	public XlsxWriterBenchmarks()
	{
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		this.ms = new MemoryStream(10 * 0x100000); // 10mb
	}

	Stream GetStream([CallerMemberName] string name = null)
	{
		//this.ms.Position = 0;
		//this.ms.SetLength(0);
		//return new NoCloseStream(this.ms);
		return File.Create(name + ".xlsx");
	}

	DbDataReader GetData()
	{
		return TestData.GetData();
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
