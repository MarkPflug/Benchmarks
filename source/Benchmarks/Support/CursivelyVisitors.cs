using Cursively;
using System;
using System.Buffers.Text;
using System.Text;

namespace Benchmarks;

class CursivelyStringVisitor : CsvReaderVisitorBase
{
	readonly byte[] bytes = new byte[1024];
	int bytesUsed = 0;

	// in any realistic scenario we'd need to at least know the column oridnal to do anything with the record
	int ordinal = 0;

	public CursivelyStringVisitor()
	{
	}

	public override void VisitEndOfField(System.ReadOnlySpan<byte> chunk)
	{
		if (bytesUsed != 0)
		{
			chunk.CopyTo(bytes.AsSpan(bytesUsed, chunk.Length));
			chunk = new ReadOnlySpan<byte>(bytes, 0, bytesUsed + chunk.Length);
			bytesUsed = 0;
		}
		var str = Encoding.UTF8.GetString(chunk);
		ordinal++;
	}

	public override void VisitEndOfRecord()
	{
		ordinal = 0;
	}

	public override void VisitPartialFieldContents(System.ReadOnlySpan<byte> chunk)
	{
		chunk.CopyTo(bytes.AsSpan(bytesUsed, chunk.Length));
		bytesUsed += chunk.Length;
	}
}

class CursivelySelectVisitor : CsvReaderVisitorBase
{
	readonly byte[] bytes = new byte[1024];
	int bytesUsed = 0;

	int ordinal = 0;
	int row = 0;

	int id;
	DateTime orderDate;
	string type;
	decimal profit;

	public override void VisitEndOfField(ReadOnlySpan<byte> chunk)
	{
		Span<char> scratch = stackalloc char[32];

		if (bytesUsed != 0)
		{
			chunk.CopyTo(bytes.AsSpan(bytesUsed, chunk.Length));
			chunk = new ReadOnlySpan<byte>(bytes, 0, bytesUsed + chunk.Length);
			bytesUsed = 0;
		}
		if (row != 0) // skip the header row
		{
			switch (ordinal)
			{
				case 2:
					type = Encoding.UTF8.GetString(chunk);
					break;
				case 5:
					// Utf8Parser doesn't support all date formats
					// apparently.
					var len = Encoding.UTF8.GetChars(chunk, scratch);
					if (!DateTime.TryParse(scratch.Slice(0, len), out orderDate))
					{
						throw new FormatException();
					}
					break;
				case 6:
					if (!Utf8Parser.TryParse(chunk, out id, out _))
					{
						throw new FormatException();
					}
					break;
				case 20:
					if (!Utf8Parser.TryParse(chunk, out profit, out _))
					{
						throw new FormatException();
					}
					break;
			}
		}
		ordinal++;
	}

	public override void VisitEndOfRecord()
	{
		if (row > 0)
		{
			// presumably, at this point something would be done with the field values collected for this row.
			// Dealing with the data as an IEnumerable, for example would be difficult
		}
		ordinal = 0;
		row++;
	}

	public override void VisitPartialFieldContents(System.ReadOnlySpan<byte> chunk)
	{
		if (row > 0)
		{
			chunk.CopyTo(bytes.AsSpan(bytesUsed, chunk.Length));
			bytesUsed += chunk.Length;
		}
	}
}
