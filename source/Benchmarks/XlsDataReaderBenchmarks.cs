using BenchmarkDotNet.Attributes;
using ExcelDataReader;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;

namespace Benchmarks;

[MemoryDiagnoser]
[HideColumns("StdDev", "RatioSD", "Gen0", "Gen1", "Gen2")]
public class XlsReaderBenchmarks
{
	const string file = @"Data/65K_Records_Data.xls";

	public XlsReaderBenchmarks()
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	}

	[Benchmark(Baseline = true)]
	public void SylvanXls()
	{
		var reader = Sylvan.Data.Excel.ExcelDataReader.Create(file);
		while (reader.Read())
		{
			reader.ProcessSalesRecord();
		}
	}

	[Benchmark]
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
			reader.Read();//skip headers
			while (reader.Read())
			{
				reader.ProcessSalesRecordEDR();
			}
		}
	}

	[Benchmark]
	public void NpoiXls()
	{
		using var stream = File.OpenRead(file);
		using var wb = new HSSFWorkbook(stream);
		wb.ProcessSalesDataNpoi();
	}

	[Benchmark]
	public void AsposeXls()
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
}
