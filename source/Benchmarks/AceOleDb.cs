using System.Data.OleDb;
using System.Runtime.Versioning;

namespace Benchmarks
{
	static class AceOleDb
	{
		const string fmt = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';OLE DB Services = -4;Extended Properties=\"Excel 12.0 XML;HDR=Yes\"";

		[SupportedOSPlatform("windows")]
		public static void ProcessFile(string file)
		{
			var connStr = string.Format(fmt, file);
			using var c = new OleDbConnection(connStr);
			c.Open();
			using var cmd = c.CreateCommand();
			cmd.CommandText = "select * from [500000 Sales Records$]";
			using var r = cmd.ExecuteReader();
			var count = r.FieldCount;
			while (r.Read())
			{
				for (int i = 0; i < count; i++)
				{
					var s = r.GetValue(i);
				}
			}
		}
	}
}
