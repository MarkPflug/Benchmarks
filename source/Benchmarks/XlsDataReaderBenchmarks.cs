using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ExcelDataReader;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Benchmarks
{
	[MemoryDiagnoser]
	public class XlsBenchmarks
	{
		const string file = @"Data/65K_Records_Data.xls";

		public XlsBenchmarks()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}

		[Benchmark]
		public void SylvanXls()
		{
			var reader = Sylvan.Data.Excel.ExcelDataReader.Create(file);
			do
			{
				while (reader.Read())
				{
					for (int i = 0; i < reader.FieldCount; i++)
					{
						reader.GetString(i);
					}
				}
			} while (reader.NextResult());
		}

		[Benchmark]
		public void ExcelDataReaderXls()
		{
			using var stream = File.OpenRead(file);
			using (var reader = ExcelReaderFactory.CreateReader(stream))
			{
				do
				{
					while (reader.Read())
					{
						for (int i = 0; i < reader.FieldCount; i++)
						{
							reader.GetValue(i);
						}
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
}
