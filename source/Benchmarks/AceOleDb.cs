using System.Data.Common;
using System.Data.OleDb;
using System.Runtime.Versioning;

namespace Benchmarks;

static class AceOleDb
{
	const string fmt = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='{0}';OLE DB Services = -4;Extended Properties=\"Excel 12.0 XML;HDR=Yes\"";

	[SupportedOSPlatform("windows")]
	public static void ProcessFile(string file, int rowCount = int.MaxValue)
	{
		var connStr = string.Format(fmt, file);
		using var c = new OleDbConnection(connStr);
		c.Open();
		using var cmd = c.CreateCommand();
		cmd.CommandText = "select * from [500000 Sales Records$]";
		using var r = cmd.ExecuteReader();

		while (r.Read())
		{
			r.ProcessSalesRecordAce();
		}

	}

	static void ProcessSalesRecordAce(this DbDataReader reader)
	{
		var region = reader.GetString(0);
		var country = reader.GetString(1);
		var type = reader.GetString(2);
		var channel = reader.GetString(3);
		var priority = reader.GetString(4);
		var orderDate = reader.GetDateTime(5);
		var id = (int)reader.GetDouble(6);
		var shipDate = reader.GetDateTime(7);
		var unitsSold = (int)reader.GetDouble(8);
		var unitPrice = (decimal)reader.GetDouble(9);
		var unitCost = (decimal)reader.GetDouble(10);
		var totalRevenue = (decimal)reader.GetDouble(11);
		var totalCost = (decimal)reader.GetDouble(12);
		var totalProfit = (decimal)reader.GetDouble(13);
	}
}
