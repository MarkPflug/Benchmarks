using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ExcelDataReader;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Sylvan.Data.Excel;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;

namespace Benchmarks;

[MemoryDiagnoser]
public class XlsReaderBenchmarks
{
	const string file = @"Data/65K_Records_Data.xls";

	public XlsReaderBenchmarks()
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	}

	[Benchmark]
	public void SylvanStringXls()
	{
		var reader = Sylvan.Data.Excel.ExcelDataReader.Create(file);
		do
		{
			while (reader.Read())
			{
				reader.Process();
			}
		} while (reader.NextResult());
	}

	[Benchmark]
	public void SylvanSchemaXls()
	{
		var opts = new ExcelDataReaderOptions { Schema = new ExcelSchema(true, TestData.GetSchema()) };
		var reader = Sylvan.Data.Excel.ExcelDataReader.Create(file, opts);
		do
		{
			reader.Process();
		} while (reader.NextResult());
	}

	//[Benchmark]
	[SupportedOSPlatform("windows")]
	public void AceOleDbXls()
	{
		AceOleDb.ProcessFile(file);
	}

	[Benchmark]
	public void ExcelDataReaderXls()
	{
		using var stream = File.OpenRead(file);
		using (var reader = ExcelReaderFactory.CreateReader(stream))
		{
			do
			{
				reader.Read();//skip headers
				while (reader.Read())
				{
					reader.ProcessSalesRecordEDR();
				}
			} while (reader.NextResult());
		}
	}

	[Benchmark]
	public void NpoiXls()
	{
		using var stream = File.OpenRead(file);
		var wb = new HSSFWorkbook(stream);
		for (int sheetIdx = 0; sheetIdx < wb.NumberOfSheets; sheetIdx++)
		{
			var sheet = wb.GetSheetAt(sheetIdx);
			IRow headerRow = sheet.GetRow(0);
			if (headerRow == null) continue;
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
	}

	[Benchmark]
	public void AsposeXls()
	{
		var wb = new Aspose.Cells.Workbook(file);
		foreach (var ws in wb.Worksheets)
		{
			var cells = ws.Cells;
			var rowCount = cells.GetLastDataRow(0);
			foreach (Aspose.Cells.Row row in cells.Rows)
			{
				foreach (Aspose.Cells.Cell ce in row)
				{
					switch (ce.Type)
					{
						case CellValueType.IsString:
							var s = ce.StringValue;
							break;
						case CellValueType.IsNumeric:
							var n = ce.DoubleValue;
							break;
					}
				}
			}
		}
	}
}
