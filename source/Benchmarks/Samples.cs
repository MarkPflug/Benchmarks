using Dapper;
using Sylvan.Data.Csv;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace Benchmarks
{
	class TypedCsvColumn : DbColumn
	{
		public TypedCsvColumn(Type type, bool allowNull)
		{
			// if you assign ColumnName here, it will override whatever is in the csv header
			this.DataType = type;
			this.AllowDBNull = allowNull;
		}
	}

	class TypedCsvSchema : ICsvSchemaProvider
	{
		List<TypedCsvColumn> columns;

		public TypedCsvSchema()
		{
			this.columns = new List<TypedCsvColumn>();
		}

		public TypedCsvSchema Add(Type type, bool allowNull = false)
		{
			this.columns.Add(new TypedCsvColumn(type, allowNull));
			return this;
		}

		DbColumn ICsvSchemaProvider.GetColumn(string name, int ordinal)
		{
			return ordinal < columns.Count ? columns[ordinal] : null;
		}
	}

	class MyDataRecord
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public double? Value { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
	}

	static class Samples
	{
		public static void SylvanWithDapper()
		{
			var csvData = "Id,Name,Value,CreateDate,ModifiedDate\n1,Hello,1.1,2020-12-15,\n2,World,,2020-12-14,2020-12-15\n";

			var schema = new TypedCsvSchema()
				.Add(typeof(int))
				.Add(typeof(string))
				.Add(typeof(double), true)
				.Add(typeof(DateTime))
				.Add(typeof(DateTime), true);

			var options = new CsvDataReaderOptions
			{
				Schema = schema
			};

			var reader = new StringReader(csvData);
			//var reader = File.OpenText("mydata.csv");
			using var csv = CsvDataReader.Create(reader, options);
			foreach (MyDataRecord record in csv.GetRecordsDapper<MyDataRecord>())
			{
				Console.WriteLine($"{record.Name} {record.Value}");
			}
		}
	}
}
