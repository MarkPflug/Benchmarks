using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using System.Data;
using System.IO;
using System.Text;

namespace Benchmarks
{
	[MemoryDiagnoser]
	public class XlsbBenchmarks
	{
		const string file = @"Data/65K_Records_Data.xlsb";

		public XlsbBenchmarks()
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
		public void SylvanXlsb()
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

		[Benchmark]
		public void ExcelDataReaderXlsb()
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
		public void AsposeXlsb()
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
	}
}
