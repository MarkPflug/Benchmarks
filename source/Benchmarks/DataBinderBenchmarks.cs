using BenchmarkDotNet.Attributes;
using Dapper;
using Sylvan.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Benchmarks
{
	[MemoryDiagnoser]
	public class DataBinderBenchmarks
	{
		// lightweight DbDataReader to try to isolate performance measurement of the binder logic only.
		class TestReader : DbDataReader, IDbColumnSchemaGenerator
		{
			public void Reset()
			{
				this.row = 0;
			}

			Dictionary<string, int> ordinals;
			ReadOnlyCollection<DbColumn> columns;
			readonly int count;
			int row;

			class Col : DbColumn
			{
				public Col(int ordinal, string name, Type type)
				{
					this.ColumnOrdinal = ordinal;
					this.ColumnName = name;
					this.DataType = type;
					this.AllowDBNull = false;
				}
			}

			public TestReader(int count)
			{
				this.row = 0;
				this.count = count;
				this.columns = new ReadOnlyCollection<DbColumn>(
					new[]
					{

						new Col(0, "B", typeof(bool)),
						new Col(1, "D", typeof(DateTime)),
						new Col(2, "V", typeof(double)),
						new Col(3, "G", typeof(Guid)),
						new Col(4, "I", typeof(int)),
						new Col(5, "S", typeof(string)),

					}
				); ;

				this.ordinals =
					columns
					.Select(c => new { Name = c.ColumnName, Index = c.ColumnOrdinal.Value })
					.ToDictionary(p => p.Name, p => p.Index);
				this.count = count;
			} 

			public override object this[int i] => GetValue(i);

			public override object this[string name] => ordinals[name];

			public override int FieldCount => columns.Count;

			public override int Depth => 1;

			public override bool IsClosed => false;

			public override int RecordsAffected => 0;

			public override bool HasRows => true;

			public override bool GetBoolean(int i)
			{
				return true;
			}

			public override byte GetByte(int i)
			{
				throw new NotImplementedException();
			}

			public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
			{
				throw new NotImplementedException();
			}

			public override char GetChar(int i)
			{
				throw new NotImplementedException();
			}

			public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
			{
				throw new NotImplementedException();
			}

			public IDataReader GetData(int i)
			{
				throw new NotImplementedException();
			}

			public override string GetDataTypeName(int i)
			{
				return columns[i].ColumnName;
			}

			DateTime date = new DateTime(2020, 11, 12, 13, 14, 15, DateTimeKind.Utc);

			public override DateTime GetDateTime(int i)
			{
				return date;
			}

			public override decimal GetDecimal(int i)
			{
				throw new NotSupportedException();
			}

			public override double GetDouble(int i)
			{
				return 12345.5;
			}

			public override Type GetFieldType(int i)
			{
				return columns[i].DataType;
			}

			public override float GetFloat(int i)
			{
				throw new NotImplementedException();
			}

			Guid g = Guid.NewGuid();
			public override Guid GetGuid(int i)
			{
				return g;
			}

			public override short GetInt16(int i)
			{
				throw new NotImplementedException();
			}

			public override int GetInt32(int i)
			{
				return 64532;
			}

			public override long GetInt64(int i)
			{
				throw new NotImplementedException();
			}

			public override string GetName(int i)
			{
				return columns[i].ColumnName;
			}

			public override int GetOrdinal(string name)
			{
				return ordinals[name];
			}

			public override string GetString(int i)
			{
				return "This is a test string";
			}

			public override object GetValue(int i)
			{
				switch (i)
				{
					case 0: return GetBoolean(i);
					case 1: return GetDateTime(i);
					case 2: return GetDouble(i);
					case 3: return GetGuid(i);
					case 4: return GetInt32(i);
					case 5: return GetString(i);
				}
				throw new NotSupportedException();
			}

			public override int GetValues(object[] values)
			{
				throw new NotImplementedException();
			}

			public override bool IsDBNull(int i)
			{
				return false;
			}

			public override void Close()
			{
			}

			public override DataTable GetSchemaTable()
			{
				throw new NotImplementedException();
			}

			public override bool NextResult()
			{
				return false;
			}

			public override bool Read()
			{
				return row++ < count;
			}

			public override IEnumerator GetEnumerator()
			{
				throw new NotImplementedException();
			}

			public ReadOnlyCollection<DbColumn> GetColumnSchema()
			{
				return columns;
			}
		}

		const int Count = 1000000;

		DbDataReader GetReader()
		{
			testReader.Reset();
			return testReader;
		}

		public class Record
		{
			public bool B { get; set; }
			public DateTime D { get; set; }
			public double V { get; set; }
			public Guid G { get; set; }
			public int I { get; set; }
			public string S { get; set; }
		}

		public DataBinderBenchmarks()
		{
			var schema = Schema.Parse("B:Boolean,D:DateTime,V:Double,G:Guid,I:Int32,S:String");
			var record = new TestReader(1);
			this.item = new Record();
			this.compiled = DataBinder.Create<Record>(schema);
			this.dapperBinder = record.GetRowParser<Record>();
			this.testReader = new TestReader(Count);
		}

		class ManualBinder : IDataBinder<Record>
		{
			public void Bind(DbDataReader record, Record item)
			{
				item.B = record.GetBoolean(0);
				item.D = record.GetDateTime(1);
				item.V = record.GetDouble(2);
				item.G = record.GetGuid(3);
				item.I = record.GetInt32(4);
				item.S = record.GetString(5);
			}

			public void Bind(DbDataReader record, object item)
			{
				Bind(record, (Record)item);
			}
		}

		TestReader testReader;
		Record item;
		IDataBinder<Record> compiled, objBind;
		Func<IDataReader, Record> dapperBinder;


		[Benchmark]
		public void SylvanCompiledReuse()
		{
			var reader = GetReader();
			while (reader.Read())
			{
				var item = new Record();
				compiled.Bind(reader, item);
			}
		}

		[Benchmark]
		public void SylvanCompiled()
		{
			var reader = GetReader();
			var binder = DataBinder.Create<Record>(reader);
			while (reader.Read())
			{
				var item = new Record();
				binder.Bind(reader, item);
			}
		}

		//[Benchmark]
		//public void SylvanObjectBinder()
		//{
		//	var reader = new TestReader(Count);
		//	Bench(objBind, reader);
		//}

		[Benchmark]
		public void Manual()
		{
			var reader = GetReader();
			Bench(compiled, reader);
		}

		static void Bench(IDataBinder<Record> binder, DbDataReader reader)
		{
			while(reader.Read()) { 
				var item = new Record();
				binder.Bind(reader, item);
			}
		}

		[Benchmark]
		public void Dapper()
		{
			var reader = GetReader();
			var binder = reader.GetRowParser<Record>();
			while (reader.Read())
			{
				var item = binder(reader);
			}
		}
	}
}
