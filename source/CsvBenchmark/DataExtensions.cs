using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace CsvBenchmark
{
	static class DataExtensions
	{
		public static void Process(this IDataReader reader)
		{
			while (reader.Read())
			{
				reader.ProcessRecord();
			}
		}

		public static void ProcessRecord(this IDataRecord record)
		{
			for (int i = 0; i < record.FieldCount; i++)
			{
				if (record.IsDBNull(i))
					continue;

				switch (Type.GetTypeCode(record.GetFieldType(i)))
				{
					case TypeCode.Boolean:
						record.GetBoolean(i);
						break;
					case TypeCode.Int32:
						record.GetInt32(i);
						break;
					case TypeCode.DateTime:
						record.GetDateTime(i);
						break;
					case TypeCode.Double:
						record.GetDouble(i);
						break;
					case TypeCode.Decimal:
						record.GetDecimal(i);
						break;
					case TypeCode.String:
						record.GetString(i);
						break;
					default:
						continue;
				}
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
