using Dapper;
using System.Globalization;
using System.IO;

namespace Benchmarks
{
	static class Experiments
	{
		const string CsvData =
			@"Name,Value
Red,1.4
Green,3.3
Blue,2.8
Black,
";
		class DataRecord
		{
			public string Name { get; set; }
			public double? Value { get; set; }
		}

		public static void DapperBinderTest()
		{
			var tr = new StringReader(CsvData);
			var csv = new CsvHelper.CsvReader(tr, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture));
			var dr = new CsvHelper.CsvDataReader(csv);
			var st = dr.GetSchemaTable();
			var parser = dr.GetRowParser<DataRecord>();
			while (dr.Read())
			{
				var a = dr.GetValue(0);
				var b = dr.GetValue(1);
				// is there a way to make dapper
				// bind the empty field to float? Value?
				var row = parser(dr);
			}
		}
	}
}
