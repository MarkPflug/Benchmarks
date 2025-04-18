﻿using BenchmarkDotNet.Attributes;
using Cursively;
using Microsoft.VisualBasic.FileIO;
using nietras.SeparatedValues;
using RecordParser.Extensions;
using Sylvan.Data.Csv;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Runtime.Versioning;
using System.Text;

namespace Benchmarks;

[MemoryDiagnoser]
[HideColumns("StdDev", "RatioSD", "Gen0", "Gen1", "Gen2")]
public class CsvReaderBenchmarks
{
	// buffer size for libraries that allow configuration
	const int BufferSize = 0x10000;
	readonly char[] buffer = new char[BufferSize];

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
	public void CsvHelper()
	{
		var tr = TestData.GetTextReader();
		var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture)
		{
			BufferSize = BufferSize
		};

		var r = new CsvHelper.CsvParser(tr, config);
		while (r.Read())
		{
			for (int i = 0; i < r.Count; i++)
			{
				var s = r[i];
			}
		}
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
		var config = new FastCsvParser.CsvReader.Config() { ReadinBufferSize = BufferSize };
		var csv = new FastCsvParser.CsvReader(s, Encoding.UTF8, config);
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
	public void FlameCsvStream()
	{
		var s = TestData.GetUtf8Stream();
		var csv = FlameCsv.CsvReader.Enumerate(s);
		foreach(var record in csv)
		{
			for (int i = 0; i < record.FieldCount; i++)
			{
				var str = record.ParseField<string>(i);
			}
		}
	}

	[Benchmark]
	public void FlameCsvText()
	{
		var s = TestData.GetTextReader();
		var csv = FlameCsv.CsvReader.Enumerate(s);
		foreach (var record in csv)
		{
			for (int i = 0; i < record.FieldCount; i++)
			{
				var str = record.ParseField<string>(i);
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
		var dr = new LumenWorks.Framework.IO.Csv.CsvReader(tr, true, BufferSize);
		dr.ProcessStrings();
	}

	[Benchmark]
	public void NLightCsv()
	{
		var tr = TestData.GetTextReader();
		var dr = new NLight.IO.Text.DelimitedRecordReader(tr, BufferSize);
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

	//[Benchmark] // most people won't be able to run this.
	[SupportedOSPlatform("windows")]
	public void OleDbCsv()
	{
		// NOTE: I don't know how to disable the column type detection here
		// so this is a bit inconsistent with other libraries in that it doesn't 
		// process raw string values.

		//Requires: https://www.microsoft.com/en-us/download/details.aspx?id=54920
		var connString = string.Format(
			@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=""Text;HDR=YES;FMT=Delimited""",
			Path.GetDirectoryName(Path.GetFullPath(TestData.DataFile))
		);
		using var conn = new OleDbConnection(connString);
		conn.Open();
		using var cmd = conn.CreateCommand();
		cmd.CommandText = "SELECT * FROM [" + Path.GetFileName(TestData.DataFile) + "]";
		using var dr = cmd.ExecuteReader();

		dr.ProcessValues();
	}

	[Benchmark]
	public void FlatFilesCsv()
	{
		var tr = TestData.GetTextReader();
		var opts = new FlatFiles.DelimitedOptions() { IsFirstRecordSchema = true };
		var dr = new FlatFiles.FlatFileDataReader(new FlatFiles.DelimitedReader(tr, opts));
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
	public void Fluent()
	{
		var tr = TestData.GetTextReader();

		var splitter = new global::FluentCsv.CsvParser.Splitters.Rfc4180DataSplitter();
		string line;
		while ((line = tr.ReadLine()) != null)
		{
			var row = splitter.SplitColumns(line, ",");
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
				(object obj, fastCSV.COLUMNS cols) =>
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
	public void CursivelyCsv()
	{
		var d = TestData.GetUtf8Array();
		var proc = new CursivelyStringVisitor();
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
	public void SoftCircuits()
	{
		var stream = TestData.GetUtf8Stream();
		using (var reader = new SoftCircuits.CsvParser.CsvReader(stream, Encoding.UTF8))
		{
			string[] columns = null;
			while (reader.ReadRow(ref columns))
			{
			}
		}
	}

	[Benchmark]
	public void Sep()
	{
		using var tr = TestData.GetTextReader();
		using var reader = nietras.SeparatedValues.Sep.Reader().From(tr);
		foreach (var row in reader)
		{
			var c = row.ColCount;
			for (int i = 0; i < c; i++)
			{
				var s = row[i].ToString();
			}
		}
	}

	[Benchmark(Baseline = true)]
	public void Sylvan()
	{
		using var tr = TestData.GetTextReader();	
		using var reader = CsvDataReader.Create(tr, buffer);
		var c = reader.FieldCount;
		while (reader.Read())
		{
			for (int i = 0; i < c; i++)
			{
				var s = reader.GetString(i);
			}
		}
	}

	[Benchmark]
	public void RecordParser()
	{
		RecordParserParallel(1);
	}

	[Benchmark]
	public void RecordParserX4()
	{
		RecordParserParallel(4);
	}

	void RecordParserParallel(int dop)
	{
		const int fieldCount = 13;

		var options = new VariableLengthReaderRawOptions
		{
			HasHeader = true,
			ContainsQuotedFields = false,
			ColumnCount = fieldCount,
			Separator = ",",
			ParallelismOptions = new()
			{
				Enabled = dop > 1,
				MaxDegreeOfParallelism = dop,
				EnsureOriginalOrdering = false,
			},
		};

		using var tr = TestData.GetTextReader();

		var records = tr.ReadRecordsRaw(options, getField =>
		{
			for (int i = 0; i < fieldCount; i++)
			{
				var s = getField(i);
			}

			// currently only supports Func callback
			// so must return a dummy value
			return (string)null;
		});

		foreach (var record in records) ;
	}
}
