using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using System.Text;
using Sylvan.Data.Excel;
using System.IO;
using System.Linq;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using OfficeOpenXml;

namespace Benchmarks
{
	[MemoryDiagnoser]
	[SimpleJob(1, 2, 4, 1)]
	public class XlsxBenchmarks
	{
		const string file = @"\data\excel\Excel Pkdx V5.14.xlsx";
		//const string file = @"\data\excel\itcont.xlsx";
		public XlsxBenchmarks()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}

		[Benchmark]
		public void SylvanXlsx()
		{
			var reader = Sylvan.Data.Excel.ExcelDataReader.Create(file);
			do
			{
				int c = 0;
				while (reader.Read())
				{
					for (int i = 0; i < reader.FieldCount; i++)
					{
						var s = reader.GetString(i);
					}
					if (c++ == 10000)
					{
						;
					}

				}
			} while (reader.NextResult());
		}

		[Benchmark(Baseline = true)]
		public void ExcelDataReaderXlsx()
		{
			using var stream = File.OpenRead(file);
			using (var reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream))
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
		public void ClosedXmlsXlsx()
		{
			var pkg = new ClosedXML.Excel.XLWorkbook(file);
			var ws = pkg.Worksheets.First();

			// can't figure out how to get these values...
			var rc = 664;// ws.RowCount();
			var cc = 35;// ws.ColumnCount();
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

		[Benchmark]
		public void AsposeXlsx()
		{
			var wb = new Workbook(file);
			foreach (var ws in wb.Worksheets)
			{
				var cells = ws.Cells;
				var rowCount = cells.GetLastDataRow(0);
				foreach (Row row in cells.Rows)
				{
					foreach (Cell ce in row)
					{
						var value = ce.Value;
						//switch (ce.Type)
						//{
						//	case CellValueType.IsString:
						//		var s = ce.StringValue;
						//		break;
						//	case CellValueType.IsNumeric:
						//		var n = ce.DoubleValue;
						//		break;
						//	case CellValueType.IsNull:
						//		break;
						//	default:
						//		throw new NotSupportedException();
						//}
					}
				}
			}
		}
	}
}
