using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using System.IO;
using System.Runtime.Versioning;
using System.Text;

namespace Benchmarks;

[MemoryDiagnoser]
[HideColumns("StdDev", "RatioSD", "Gen0", "Gen1", "Gen2")]
public class XlsbReaderBenchmarks
{
	const string file = @"Data/65K_Records_Data.xlsb";

	public XlsbReaderBenchmarks()
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	}

	[Benchmark]
	public void SylvanXlsb()
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

	[Benchmark]
	[SupportedOSPlatform("windows")]
	public void AceOleDbXlsb()
	{
		AceOleDb.ProcessFile(file);
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
					reader.ProcessSalesRecordEDR();
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
