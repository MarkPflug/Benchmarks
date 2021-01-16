using BenchmarkDotNet.Attributes;

#if NET5_0
using Cesil;
#endif

using Sylvan.Data;
using Sylvan.Data.Csv;
using System.Globalization;
using Dapper;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using System.Data;

namespace CsvBenchmark
{
	[MemoryDiagnoser]
	public class CsvDataBinderBenchmarks
	{
		public CsvDataBinderBenchmarks()
		{
		}

		class ManualBinder
		{
			public CovidRecord Bind(IDataRecord dr)
			{
				var record = new CovidRecord();
				record.UID = dr.GetInt32(0);
				record.iso2 = dr.GetString(1);
				record.iso3 = dr.GetString(2);
				
				if(!dr.IsDBNull(3))
					record.code3 = dr.GetInt32(3);
				
				if (!dr.IsDBNull(4))
					record.FIPS = dr.GetFloat(4);

				record.Admin2 = dr.GetString(5);
				record.Province_State = dr.GetString(6);
				record.Country_Region = dr.GetString(7);
				
				if (!dr.IsDBNull(8))
					record.Lat = dr.GetFloat(8);

				if (!dr.IsDBNull(9))
					record.Long_ = dr.GetFloat(9);

				record.Country_Region = dr.GetString(10);
				return record;
			}
		}

		[Benchmark(Baseline = true)]
		public void CsvHelperAuto()
		{
			var tr = TestData.GetTextReader();
			var csv = new CsvHelper.CsvReader(tr, new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture));
			var data = csv.GetRecords<CovidRecord>();

			foreach (var record in data)
			{
			}
		}

		[Benchmark]
		public void CsvHelperManual()
		{
			var tr = TestData.GetTextReader();
			var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture);
			
			var strOpts = new CsvHelper.TypeConversion.TypeConverterOptions();
			strOpts.NullValues.Add(""); // this is needed to IsDbNull() detects empty fields as null
			config.TypeConverterOptionsCache.AddOptions(typeof(string), strOpts);

			var csv = new CsvHelper.CsvReader(tr, config);
			var dr = new CsvHelper.CsvDataReader(csv);
			
			var binder = new ManualBinder();

			while (dr.Read())
			{
				var record = binder.Bind(dr);			
			}
		}			

#if NET5_0

		[Benchmark]
		public void CesilAuto()
		{
			var tr = TestData.GetTextReader();

			var opts =
				Options
				.CreateBuilder(Options.Default)
				.WithRowEnding(RowEnding.LineFeed)
				.ToOptions();

			var data = CesilUtils.Enumerate<CovidRecord>(tr, opts);
			foreach (var record in data)
			{

			}
		}
#endif

		[Benchmark]
		public void MgholamCsvManual()
		{
			var data = fastCSV.ReadStream<CovidRecord>(TestData.GetTextReader(), ',',
				(obj, cols) =>
				{
					obj.UID = fastCSV.ToInt(cols[0]);
					obj.iso2 = cols[1];
					obj.iso3 = cols[2];

					var str = cols[3];
					if (!string.IsNullOrEmpty(str))
						obj.code3 = fastCSV.ToInt(str);

					str = cols[4];
					if (!string.IsNullOrEmpty(str))
						obj.FIPS = fastCSV.ToInt(str);

					obj.Admin2 = cols[5];
					obj.Province_State = cols[6];
					obj.Country_Region = cols[7];

					str = cols[8];
					if (!string.IsNullOrEmpty(str))
						obj.Lat = float.Parse(str);

					str = cols[9];
					if (!string.IsNullOrEmpty(str))
						obj.Long_ = float.Parse(str);

					obj.Country_Region = cols[10];
					return true;
				}
			);
		}

		[Benchmark]
		public void TinyCsvManual()
		{
			var csvP = new CsvParser<CovidRecord>(new CsvParserOptions(true, ','), new CovidMapping());
			var dr = csvP.ReadFromString(new CsvReaderOptions(new[] { "\r\n", "\n", "\r" }), TestData.CachedData);
			foreach (var record in dr)
			{
			}
		}

		[Benchmark]
		public void SylvanData()
		{
			var dr = (CsvDataReader)TestData.GetDataWithSchema();

			var binder = DataBinder<CovidRecord>.Create(dr.GetColumnSchema());
			while (dr.Read())
			{
				var record = new CovidRecord();
				binder.Bind(dr, record);
			}
		}

		[Benchmark]
		public void SylvanManual()
		{
			var dr = (CsvDataReader)TestData.GetDataWithSchema();
			var binder = new ManualBinder();
			while (dr.Read())
			{
				var record = binder.Bind(dr);
			}
		}

		[Benchmark]
		public void SylvanDapper()
		{
			var dr = (CsvDataReader)TestData.GetDataWithSchema();
			var parser = dr.GetRowParser<CovidRecord>();
			while (dr.Read())
			{
				var record = parser(dr);
			}
		}

		static void ValidateRowCount(int c)
		{
			// validate that the correct number of rows were read
			const int RowCount = 3253;
			if (c != RowCount)
			{
				throw new System.Exception("Invalid row count " + c);
			}
		}
	}

	class CovidMapping : CsvMapping<CovidRecord>
	{
		public CovidMapping()
		{
			this.MapProperty(0, r => r.UID);
			this.MapProperty(1, r => r.iso2);
			this.MapProperty(2, r => r.iso3);
			this.MapProperty(3, r => r.code3);
			this.MapProperty(4, r => r.FIPS);
			this.MapProperty(5, r => r.Admin2);
			this.MapProperty(6, r => r.Province_State);
			this.MapProperty(7, r => r.Country_Region);
			this.MapProperty(8, r => r.Lat);
			this.MapProperty(9, r => r.Long_);
			this.MapProperty(10, r => r.Combined_Key);
		}
	}
}
