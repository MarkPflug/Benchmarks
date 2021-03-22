using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Linq;

namespace CsvBenchmark
{
	static class DataExtensions
	{
		// this is the "fast path" for test cases that only deal in strings.
		public static void ProcessStrings(this IDataReader reader)
		{
			while (reader.Read())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					reader.GetString(i);
				}
			}
		}

		// this is the "fast path" for test cases that only deal in strings.
		public static void ProcessValues(this IDataReader reader)
		{
			while (reader.Read())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					reader.GetValue(i);
				}
			}
		}

		public static void Process(this IDataReader reader)
		{
			while (reader.Read())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					if (reader.IsDBNull(i))
						continue;
					ProcessField(reader, i);
				}
			}
		}

		public static void Process(this DbDataReader reader)
		{
			var cols = reader.GetColumnSchema();
			bool[] allowDbNull = cols.Select(c => c.AllowDBNull != false).ToArray();
			while (reader.Read())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					if (allowDbNull[i] && reader.IsDBNull(i))
						continue;
					ProcessField(reader, i);
				}
			}
		}

		public static async Task ProcessAsync(this DbDataReader reader)
		{
			var cols = reader.GetColumnSchema();
			bool[] allowDbNull = cols.Select(c => c.AllowDBNull != false).ToArray();
			while (await reader.ReadAsync())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					if (allowDbNull[i] && await reader.IsDBNullAsync(i))
						continue;

					ProcessField(reader, i);
				}
			}
		}

		static void ProcessField(this IDataReader reader, int i)
		{
			switch (Type.GetTypeCode(reader.GetFieldType(i)))
			{
				case TypeCode.Boolean:
					reader.GetBoolean(i);
					break;
				case TypeCode.Int32:
					reader.GetInt32(i);
					break;
				case TypeCode.DateTime:
					reader.GetDateTime(i);
					break;
				case TypeCode.Single:
					reader.GetFloat(i);
					break;
				case TypeCode.Double:
					reader.GetDouble(i);
					break;
				case TypeCode.Decimal:
					reader.GetDecimal(i);
					break;
				case TypeCode.String:
					reader.GetString(i);
					break;
				default:
					// no cheating
					throw new NotSupportedException();
			}
		}

		public static IEnumerable<T> GetRecordsDapper<T>(this DbDataReader dr)
		{
			var binder = dr.GetRowParser<T>();
			while (dr.Read())
			{
				yield return binder(dr);
			}
		}
	}
}
