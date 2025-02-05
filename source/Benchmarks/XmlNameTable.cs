//using BenchmarkDotNet.Attributes;
//using System;
//using System.Collections.Frozen;
//using System.Collections.Generic;
//using System.Linq;
//using System.Xml;

//namespace Benchmarks;

//[MemoryDiagnoser]
//public class XmlNameTableBench
//{
//	const string File = @"C:\Users\Mark\source\Sylvan\bin\Release\net6.0\Sylvan.Data.Csv.xml";
//	NameTable nt;

//	public XmlNameTableBench()
//	{
//		this.nt = new NameTable();
//		nt.Add("summary");
//		nt.Add("returns");
//		nt.Add("remarks");
//		nt.Add("param");
//		nt.Add("inheritdocs");
//		nt.Add("name");
//	}

//	[Benchmark]
//	public void Normal()
//	{
//		Process(nt);
//	}

//	[Benchmark]
//	public void Frozen()
//	{
//		var nt = new FrozenNameTable();
//		Process(nt);
//	}

//	[Benchmark]
//	public void Custom()
//	{
//		var nt = new MyNameTable();
//		Process(nt);
//	}

//	void Process(XmlNameTable table)
//	{
//		var o = new XmlReaderSettings { NameTable = table };
//		using var r = XmlReader.Create(File, o);

//		while (r.Read())
//		{
//			if (r.IsStartElement() && r.LocalName == "member")
//			{
//				while (r.Read())
//				{
//					if (r.IsStartElement())
//					{
//						if (r.LocalName == "summary")
//						{
//							F.A++;
//						}
//						if (r.LocalName == "param")
//						{
//							r.GetAttribute("name");
//							F.B++;
//						}
//						if (r.LocalName == "returns")
//						{
//							F.C++;
//						}
//					}
//					if (r.NodeType == XmlNodeType.EndElement && r.LocalName == "member")
//					{
//						break;
//					}
//				}
//			}
//		}
//	}
//}

//sealed class FrozenNameTable : XmlNameTable
//{
//	static readonly FrozenDictionary<string, string> Names =
//		new Dictionary<string, string>
//		{
//			{ "summary", "summary" },
//			{ "returns", "returns" },
//			{ "remarks", "remarks" },
//			{ "param", "param" },
//			{ "inheritdocs", "inheritdocs" },
//			{ "name", "name" },
//		}.ToFrozenDictionary();
//	static readonly FrozenDictionary<string, string>.AlternateLookup<ReadOnlySpan<char>> Lookup = Names.GetAlternateLookup<ReadOnlySpan<char>>();

//	public FrozenNameTable()
//	{
//		this.nt = new NameTable();
//	}

//	NameTable nt;

//	public override string Add(char[] array, int offset, int length)
//	{
//		return Add(array.AsSpan(offset, length)) ?? nt.Add(array, offset, length);
//	}

//	public override string Add(string array)
//	{
//		return Add(array) ?? nt.Add(array);
//	}

//	public override string? Get(char[] array, int offset, int length)
//	{
//		return Add(array.AsSpan(offset, length)) ?? nt.Get(array, offset, length);
//	}

//	public override string? Get(string array)
//	{
//		return Add(array) ?? nt.Get(array);
//	}

//	string? Add(ReadOnlySpan<char> str)
//	{
//		if (Lookup.TryGetValue(str, out var value))
//		{
//			return value;
//		}
//		return null;
//	}
//}

//public sealed class MyNameTable : XmlNameTable
//{
//	public MyNameTable()
//	{
//		this.nt = new NameTable();
//	}

//	NameTable nt;

//	public override string Add(char[] array, int offset, int length)
//	{
//		return Add(array.AsSpan(offset, length)) ?? nt.Add(array, offset, length);
//	}

//	public override string Add(string array)
//	{
//		return Add(array) ?? nt.Add(array);
//	}

//	public override string? Get(char[] array, int offset, int length)
//	{
//		return Add(array.AsSpan(offset, length)) ?? nt.Get(array, offset, length);
//	}

//	public override string? Get(string array)
//	{
//		return Add(array) ?? nt.Get(array);
//	}

//	public static string Add(ReadOnlySpan<char> value)
//	{
//		var len = value.Length;

//		string? str = null;
//		char c;
//		switch (len)
//		{
//			case 3:
//				c = value[0];
//				switch (c)
//				{
//					case 's':
//						return "see";
//					case 'x':
//						return "xml";
//					case 'd':
//						return "doc";
//				}
//				break;
//			case 4:
//				c = value[0];
//				switch (c)
//				{
//					case 'n':
//						return "name";
//					case 'c':
//						return "cref";
//				}
//				break;
//			case 5:
//				c = value[0];
//				switch (c)
//				{
//					case 'p':
//						return "param";
//					case 'x':
//						return "xmlns";
//				}
//				break;
//			case 6:
//				return "member";
//			case 7:
//				c = value[3];
//				switch (c)
//				{
//					case 'm':
//						return "summary";
//					case 'u':
//						return "returns";
//					case 'a':
//						return "remarks";
//					case 's':
//						return "version";
//					case 'b':
//						return "members";
//				}
//				break;
//			case 8:
//				c = value[0];
//				switch (c)
//				{
//					case 'p':
//						return "paramref";
//					case 'a':
//						return "assembly";
//				}
//				break;
//			case 10:
//				return "inheritdoc";
//			case 29:
//				return "http://www.w3.org/2000/xmlns/";
//			case 36:
//				return "http://www.w3.org/XML/1998/namespace";
//		}
//		return value.SequenceEqual(str) ? str : null;
//	}
//}