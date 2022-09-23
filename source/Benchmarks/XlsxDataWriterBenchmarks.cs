using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using Sylvan;
using Sylvan.Data;
using Sylvan.Data.Csv;
using Sylvan.Data.Excel;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
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

	SalesRecord[] records;

	[GlobalSetup]
	public void Init()
	{
		var schema =
			new Sylvan.Data.Schema.Builder()
			.Add<string>()
			.Add<string>()
			.Add<string>()
			.Add<string>()
			.Add<string>()
			.Add<DateTime>()
			.Add<int>()
			.Add<DateTime>()
			.Add<int>()
			.Add<decimal>()
			.Add<decimal>()
			.Add<decimal>()
			.Add<decimal>()
			.Add<decimal>()
			.Build();

		var sp = new StringPool();
		var opts = new CsvDataReaderOptions { Schema = new CsvSchema(schema), StringFactory = sp.GetString };
		var reader = CsvDataReader.Create(file, opts);

		this.records =
			reader
			.GetRecords<SalesRecord>()
			.ToArray();

		ms = new MemoryStream(0x100000 * 10);
	}

	Stream GetStream([CallerMemberName] string name = null)
	{
		this.ms.Position = 0;
		this.ms.SetLength(0);
		return new NoCloseStream(this.ms);
		//return File.Create(name + ".xlsx");
	}

	DbDataReader GetData()
	{
		return this.records.AsDataReader();
	}

	[Benchmark]
	public void SylvanXlsx()
	{
		using var ns = GetStream();
		using (var xw = ExcelDataWriter.Create(ns, ExcelWorkbookType.ExcelXml))
		{
			xw.Write("data", GetData());
		}
	}

	[Benchmark]
	public void NanoXlsxWrite()
	{
		using var ns = GetStream();
		// TODO: I don't see a way to write to an in-memory stream. Shouldn't affect the perf much though.
		var wb = new NanoXLSX.Workbook("nano.xlsx", "sheet1");
		var data = GetData();
		while (data.Read())
		{
			for (int i = 0; i < data.FieldCount; i++)
			{
				wb.WS.Value(data.GetValue(i));
			}
			wb.WS.Down();
		}
		wb.Save();
	}

	[Benchmark]
	public void NpoiXlsx()
	{
		using var ns = GetStream();
		IWorkbook workbook = new XSSFWorkbook();
		ISheet excelSheet = workbook.CreateSheet("Sheet1");

		var columns = new List<string>();
		IRow row = excelSheet.CreateRow(0);
		int columnIndex = 0;

		var data = GetData();

		for (int i = 0; i < data.FieldCount; i++)
		{
			var name = data.GetName(i);
			columns.Add(name);
			row.CreateCell(columnIndex).SetCellValue(name);
			columnIndex++;
		}

		int rowIndex = 1;
		while (data.Read())
		{
			row = excelSheet.CreateRow(rowIndex);
			for (int i = 0; i < data.FieldCount; i++)
			{
				var cell = row.CreateCell(i);
				var t = data.GetFieldType(i);
				var c = Type.GetTypeCode(t);
				switch (c)
				{
					case TypeCode.DateTime:
						cell.SetCellValue(data.GetDateTime(i));
						break;
					case TypeCode.Int32:
						cell.SetCellValue(data.GetInt32(i));
						break;
					case TypeCode.Double:
						cell.SetCellValue(data.GetDouble(i));
						break;
					case TypeCode.String:
						cell.SetCellValue(data.GetString(i));
						break;
					case TypeCode.Decimal:
						cell.SetCellValue((double)data.GetDecimal(i));
						break;
					default:
						throw new Exception();
				}
			}

			rowIndex++;
		}
		workbook.Write(ns);
		workbook.Close();
	}

	[Benchmark]
	public void EPPlusXlsx()
	{
		using var ns = GetStream();
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		using (var pkg = new ExcelPackage(ns))
		{
			var ws = pkg.Workbook.Worksheets.Add("data");
			var r = GetData();
			var rowIdx = 1;
			while (r.Read())
			{
				for (int i = 0; i < r.FieldCount; i++)
				{
					var cell = ws.Cells[rowIdx, i + 1];
					cell.Value = r.GetValue(i);
				}

				rowIdx++;
			}
			pkg.Save();
		}
	}

	[Benchmark]
	public void AsposeXlsx()
	{
		var w = new Aspose.Cells.Workbook();
		var s = w.Worksheets[0];
		var sc = s.Cells;

		var r = GetData();
		var rowIdx = 1;
		while (r.Read())
		{
			for (int i = 0; i < r.FieldCount; i++)
			{
				var t = r.GetFieldType(i);
				var c = Type.GetTypeCode(t);
				switch (c)
				{
					case TypeCode.String:
						sc[rowIdx, i].PutValue(r.GetString(i));
						break;
					case TypeCode.Double:
						sc[rowIdx, i].PutValue(r.GetDouble(i));
						break;
					case TypeCode.Int32:
						sc[rowIdx, i].PutValue(r.GetInt32(i));
						break;
					case TypeCode.DateTime:
						sc[rowIdx, i].PutValue(r.GetDateTime(i));
						break;
					case TypeCode.Decimal:
						sc[rowIdx, i].PutValue((double)r.GetDecimal(i));
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

			var r = GetData();
			var rowIndex = 1u;
			while (r.Read())
			{
				var row = new DocumentFormat.OpenXml.Spreadsheet.Row() { RowIndex = rowIndex };

				for (int i = 0; i < r.FieldCount; i++)
				{
					var cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();

					var t = r.GetFieldType(i);
					var c = Type.GetTypeCode(t);

					if (r.IsDBNull(i))
					{
					}
					else
					{
						switch (c)
						{
							case TypeCode.DateTime:
								cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(r.GetDateTime(i));
								cell.DataType = CellValues.Date;
								break;
							case TypeCode.Int32:
								cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(r.GetInt32(i));
								cell.DataType = CellValues.Number;
								break;
							case TypeCode.String:
								cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(r.GetString(i));
								cell.DataType = CellValues.SharedString;
								break;
							case TypeCode.Decimal:
								cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(r.GetDecimal(i));
								cell.DataType = CellValues.Number;
								break;
							case TypeCode.Double:
								cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(r.GetDouble(i));
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
