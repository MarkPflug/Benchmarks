using BenchmarkDotNet.Attributes;
using Cursively;
using FluentCsv.FluentReader;
using Microsoft.VisualBasic.FileIO;
using Sylvan.Data.Csv;
using System;
using System.Buffers.Text;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Text;
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

			while (!dr.EndOfData)
			{
				var cols = dr.ReadFields();
				for (int i = 0; i < cols.Length; i++)
				{
					var s = cols[i];
				}
			}
		}

		[Benchmark] // most people won't be able to run it.
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

		class CursivelyStringVisitor : CsvReaderVisitorBase
		{
			readonly bool doPooling;
			readonly byte[] bytes = new byte[1024];
			int bytesUsed = 0;

			// in any realistic scenario we'd need to at least know the column oridnal to do anything with the record
			int ordinal = 0;

			public CursivelyStringVisitor(bool doPooling)
			{
				this.doPooling = doPooling;
			}

			public override void VisitEndOfField(System.ReadOnlySpan<byte> chunk)
			{
				if (bytesUsed != 0)
				{
					chunk.CopyTo(bytes.AsSpan(bytesUsed, chunk.Length));
					chunk = new ReadOnlySpan<byte>(bytes, 0, bytesUsed + chunk.Length);
					bytesUsed = 0;
				}
				var str = chunk.Length == 1 && chunk[0] < 128 && doPooling
					? pool[chunk[0]]
					: Encoding.UTF8.GetString(chunk);
				ordinal++;
			}

			public override void VisitEndOfRecord()
			{
				ordinal = 0;
			}

			public override void VisitPartialFieldContents(System.ReadOnlySpan<byte> chunk)
			{
				chunk.CopyTo(bytes.AsSpan(bytesUsed, chunk.Length));
				bytesUsed += chunk.Length;
			}
		}

		[Benchmark]
		[Arguments(false)]
		[Arguments(true)]
		public void CursivelyCsv(bool doPooling)
		{
			var d = TestData.GetUtf8Array();
			var proc = new CursivelyStringVisitor(doPooling);
			CsvSyncInput
				.ForMemory(d)
				.Process(proc);
		}

		[Benchmark]
		public void CtlData()
		{
			var s = TestData.GetTextReader();
			var opts = new Ctl.Data.CsvObjectOptions()
			{
				BufferLength = BufferSize,
				Separator = ',',
				ReadHeader = true,
			};
			var csv = new Ctl.Data.CsvReader(s, opts);

			while (csv.Read())
			{
				var row = csv.CurrentRow;
				var c = row.Count;
				for (int i = 0; i < c; i++)
				{
					var str = row[i].Value;
				}
			}
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
			for (int i = 0; i < pool.Length; i++)
			{
				pool[i] = ((char)i).ToString();
			}
		}

		static string Pool(char[] buf, int offset, int length)
		{
			if (length == 1)
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
		public void MgholamFastCsvSelect()
		{
			int id;
			string name;
			int val;
			var rows =
				fastCSV.ReadStream<object>(
					TestData.GetTextReader(),
					',',
					(object obj, COLUMNS cols) =>
					{
						id = int.Parse(cols[0]);
						name = cols[10];
						val = int.Parse(cols[20]);
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
				var id = int.Parse(dr[0]);
				var name = dr[10];
				var val = int.Parse(dr[20]);
			}
		}

		class CursivelySelectVisitor : CsvReaderVisitorBase
		{
			readonly byte[] bytes = new byte[1024];
			int bytesUsed = 0;

			int ordinal = 0;
			int row = 0;

			int id;
			string name;
			int value;

			public override void VisitEndOfField(ReadOnlySpan<byte> chunk)
			{
				if (bytesUsed != 0)
				{
					chunk.CopyTo(bytes.AsSpan(bytesUsed, chunk.Length));
					chunk = new ReadOnlySpan<byte>(bytes, 0, bytesUsed + chunk.Length);
					bytesUsed = 0;
				}
				if (row != 0) // skip the header row
				{
					switch (ordinal)
					{
						case 0:
							if (!Utf8Parser.TryParse(chunk, out id, out _))
							{
								throw new FormatException();
							}
							break;
						case 10:
							name = Encoding.UTF8.GetString(chunk);
							break;
						case 20:
							if (!Utf8Parser.TryParse(chunk, out value, out _))
							{
								throw new FormatException();
							}
							break;
					}
				}
				ordinal++;
			}

			public override void VisitEndOfRecord()
			{
				if (row > 0)
				{
					// presumably, at this point something would be done with the field values collected for this row.
					// Dealing with the data as an IEnumerable, for example would be difficult
				}
				ordinal = 0;
				row++;
			}

			public override void VisitPartialFieldContents(System.ReadOnlySpan<byte> chunk)
			{
				if (row > 0)
				{
					chunk.CopyTo(bytes.AsSpan(bytesUsed, chunk.Length));
					bytesUsed += chunk.Length;
				}
			}
		}

		[Benchmark]
		public void CursivelyCsvSelect()
		{
			var d = TestData.GetUtf8Array();
			var proc = new CursivelySelectVisitor();
			CsvSyncInput
				.ForMemory(d)
				.Process(proc);
		}

		[Benchmark]
		public void SylvanSelect()
		{
			using var tr = TestData.GetTextReader();
			using var dr = CsvDataReader.Create(tr);
			while (dr.Read())
			{
				var id = dr.GetInt32(0);
				var name = dr.GetString(10);
				var val = dr.GetInt32(20);
			}
		}
	}
}
