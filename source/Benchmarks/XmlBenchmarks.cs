using BenchmarkDotNet.Attributes;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;

namespace Benchmarks
{
	// These benchmarks explore the "performance floor" for xlsx processing
	// using ZipArchive and System.Xml.XmlReader.
	class XmlBenchmarks
	{
		const string file = @"Data/65K_Records_Data.xlsx";
		MemoryStream memStream;

		string xml;

		public XmlBenchmarks()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			memStream = new MemoryStream(File.ReadAllBytes("Data/sheet1.xml"));
			xml = File.ReadAllText("Data/sheet1.xml");
		}

		[Benchmark]
		public void Xml()
		{
			var fs = new FileStream("Data/sheet1.xml", FileMode.Open, FileAccess.Read, FileShare.Read, 0x10000, false);
			var tr = new StreamReader(fs, Encoding.UTF8, false, 0x10000);
			var r = XmlReader.Create(tr);
			while (r.Read()) ;
		}

		[Benchmark]
		public void XmlMemoryStream()
		{
			// memStream contains the file bytes
			memStream.Seek(0, SeekOrigin.Begin);
			var r = XmlReader.Create(memStream);
			while (r.Read()) ;
		}

		[Benchmark]
		public void XmlStringReader()
		{
			// xml is the string from File.ReadAllText
			var ms = new StringReader(xml);
			var r = XmlReader.Create(ms);
			while (r.Read()) ;
		}

		[Benchmark]
		public void XmlLoad()
		{
			var doc = new XmlDocument();
			doc.Load("Data/sheet1.xml");
		}

		[Benchmark]
		public void ZipXml()
		{
			using var stream = File.OpenRead(file);
			var zipArchive = new ZipArchive(stream);
			var sheetEntry = zipArchive.GetEntry("xl/worksheets/sheet1.xml");
			using var sheetStream = sheetEntry.Open();
			var r = XmlReader.Create(sheetStream);
			while (r.Read()) ;
		}

		[Benchmark]
		public void ZipXmlBuffered()
		{
			using var stream = File.OpenRead(file);
			var zipArchive = new ZipArchive(stream);
			var sheetEntry = zipArchive.GetEntry("xl/worksheets/sheet1.xml");
			using var sheetStream = sheetEntry.Open();
			using var bufferedStream = new BufferedStream(sheetStream, 0x10000);
			var r = XmlReader.Create(bufferedStream);
			while (r.Read()) ;
		}

		[Benchmark]
		public void Zip()
		{
			using var stream = File.OpenRead(file);
			var zipArchive = new ZipArchive(stream);
			var sheetEntry = zipArchive.GetEntry("xl/worksheets/sheet1.xml");
			using var sheetStream = sheetEntry.Open();
			byte[] buffer = new byte[0x10000];
			while (sheetStream.Read(buffer, 0, buffer.Length) != 0) ;
		}
	}
}
