using BenchmarkDotNet.Attributes;
using FluentCsv.FluentReader;
using Microsoft.VisualBasic.FileIO;
using Sylvan;
using Sylvan.Data.Csv;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using static fastCSV;

namespace CsvBenchmark
{
	[MemoryDiagnoser]
	public class CsvReaderBenchmarks
	{
		const int BufferSize = 0x10000;

		char[] buffer = new char[BufferSize];

		[Benchmark(Baseline = true)]
		public void CsvHelper()
		{
			var tr = TestData.GetTextReader();
			var dr = new CsvHelper.CsvDataReader(new CsvHelper.CsvReader(tr, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture)));
			dr.ProcessStrings();
		}

		[Benchmark]
		public void NotVBTextFieldParser()
		{
			var tr = TestData.GetTextReader();
			var csv = new NotVisualBasic.FileIO.CsvTextFieldParser(tr);
			// what an absolutely amazing API: requires negation for the common case. lul.
			while (!csv.EndOfData)
			{
				var fields = csv.ReadFields();
			}
		}

		[Benchmark]
		public void FastCsvParser()
		{
			var s = TestData.GetUtf8Stream();
			var csv = new CsvParser.CsvReader(s, System.Text.Encoding.UTF8);
			while (csv.MoveNext())
			{
				var row = csv.Current;
				for (int i = 0; i < row.Count; i++)
				{
					var str = row[i];
				}
			}
		}

		[Benchmark]
		public void CsvBySteve()
		{
			var s = TestData.GetUtf8Stream();
			var rows = global::Csv.CsvReader.ReadFromStream(s);

			foreach (var row in rows)
			{
				for (int i = 0; i < row.ColumnCount; i++)
				{
					var str = row[i];
				}
			}
		}

		[Benchmark]
		public void Lumenworks()
		{
			var tr = TestData.GetTextReader();
			var dr = new LumenWorks.Framework.IO.Csv.CsvReader(tr, true);
			dr.ProcessStrings();
		}

		[Benchmark]
		public void NaiveBroken()
		{
			var tr = TestData.GetTextReader();
			string line;
			while ((line = tr.ReadLine()) != null)
			{
				var cols = line.Split(',');
				for (int i = 0; i < cols.Length; i++)
				{
					var s = cols[i];
				}
			}
		}

		[Benchmark]
		public void NLightCsv()
		{
			var tr = TestData.GetTextReader();
			var dr = new NLight.IO.Text.DelimitedRecordReader(tr, 0x10000);
			dr.ProcessStrings();
		}

		[Benchmark]
		public void VisualBasic()
		{
			var tr = TestData.GetTextReader();
			var dr = new TextFieldParser(tr);
			dr.SetDelimiters(",");
			dr.HasFieldsEnclosedInQuotes = true;
			// I see... someone (above) copied this abomination.
			// fair enough, actually; at least *they* made it fast.
			while (!dr.EndOfData)
			{
				var cols = dr.ReadFields();
				for (int i = 0; i < cols.Length; i++)
				{
					var s = cols[i];
				}
			}
		}

		[Benchmark] // skip this, as most people won't be able to run it.
		public void OleDbCsv()
		{
			//Requires: https://www.microsoft.com/en-us/download/details.aspx?id=54920
			var connString = string.Format(
				@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=""Text;HDR=YES;FMT=Delimited""",
				Path.GetDirectoryName(Path.GetFullPath(TestData.DataFile))
			);
			using var conn = new OleDbConnection(connString);
			conn.Open();
			var cmd = conn.CreateCommand();
			cmd.CommandText = "SELECT * FROM [" + Path.GetFileName(TestData.DataFile) + "]";
			var dr = cmd.ExecuteReader();

			dr.ProcessValues();
		}

		[Benchmark]
		public void FlatFilesCsv()
		{
			var tr = TestData.GetTextReader();
			var opts = new FlatFiles.SeparatedValueOptions() { IsFirstRecordSchema = true };
			var dr = new FlatFiles.FlatFileDataReader(new FlatFiles.SeparatedValueReader(tr, opts));
			dr.ProcessValues();
		}

		[Benchmark]
		public void FSharpData()
		{
			var tr = TestData.GetTextReader();
			var csv = FSharp.Data.CsvFile.Load(tr);

			foreach (var row in csv.Rows)
			{
				for (int i = 0; i < row.Columns.Length; i++)
				{
					var s = row.Columns[i];
				}
			}
		}

		[Benchmark]
		public void FluentSelect()
		{
			var rows =
				Read.Csv.FromString(TestData.CachedData)
				.ThatReturns.ArrayOf<(int id, string name, int count)>()
				.Put.Column(0).As<int>().Into(a => a.id)
				.Put.Column(10).Into(a => a.name)
				.Put.Column(20).As<int>().Into(a => a.count)
				.GetAll();

			foreach (var row in rows.ResultSet)
			{

			}
		}

		[Benchmark]
		public void MgholamFastCSV()
		{
			string[] values = null;
			var rows =
				fastCSV.ReadStream<object>(
					TestData.GetTextReader(),
					',',
					(object obj, COLUMNS cols) =>
					{
						var c = cols.Count;
						if (values == null)
							values = new string[c];
						for (int i = 0; i < c; i++)
						{
							values[i] = cols[i];
						}
						return false;
					}
				);
		}

		[Benchmark]
		public void NReco()
		{
			var tr = TestData.GetTextReader();
			var dr = new NReco.Csv.CsvReader(tr);
			dr.BufferSize = BufferSize;
			dr.Read(); // strip the header row
			while (dr.Read())
			{
				for (int i = 0; i < dr.FieldsCount; i++)
				{
					var s = dr[i];
				}
			}
		}

		[Benchmark]
		public void Sylvan()
		{
			using var tr = TestData.GetTextReader();
			using var dr = CsvDataReader.Create(tr);
			dr.ProcessStrings();
		}

		static readonly string[] pool = new string[128];

		static CsvReaderBenchmarks()
		{
			for(int i = 0; i < pool.Length; i++)
			{
				pool[i] = ((char)i).ToString();
			}
		}

		static string Pool(char[] buf, int offset, int length)
		{
			if(length == 1)
			{
				var c = buf[offset];
				if (c < 128)
					return pool[c];				
			}
			return new string(buf, offset, length);
		}

		[Benchmark]
		public void SylvanSimplePool()
		{
			using var tr = TestData.GetTextReader();
			using var dr = CsvDataReader.Create(tr, new CsvDataReaderOptions { StringFactory = Pool });
			dr.ProcessStrings();
		}

		[Benchmark]
		public void SylvanSchema()
		{
			using var tr = TestData.GetTextReader();
			using var dr = CsvDataReader.Create(tr, new CsvDataReaderOptions { Schema = TestData.TestDataSchema });
			dr.Process();
		}

		[Benchmark]
		public void MgholamFastCSVSelect()
		{
			string id, name, val;
			var rows =
				fastCSV.ReadStream<object>(
					TestData.GetTextReader(),
					',',
					(object obj, COLUMNS cols) =>
					{
						id = cols[0];
						name = cols[10];
						val = cols[20];
						return false;
					}
				);
		}

		[Benchmark]
		public void NRecoSelect()
		{
			using var tr = TestData.GetTextReader();
			var dr = new NReco.Csv.CsvReader(tr);
			dr.BufferSize = BufferSize;
			dr.Read(); // read the headers
			while (dr.Read())
			{
				var id = dr[0];
				var name = dr[10];
				var val = dr[20];
			}
		}

		[Benchmark]
		public void SylvanSelect()
		{
			using var tr = TestData.GetTextReader();
			using var dr = CsvDataReader.Create(tr);
			while (dr.Read())
			{
				var id = dr.GetString(0);
				var name = dr.GetString(10);
				var val = dr.GetString(20);
			}
		}
	}
}
