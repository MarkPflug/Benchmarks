using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace Benchmarks.Support
{
	// This code was copied from https://github.com/Leandromoh/RecordParser benchmarks.
	// (seems like this should be in the library itself)
	public static class RecordParserSupport
	{
		public static async IAsyncEnumerable<T> ProcessFile<T>(Stream stream, FuncSpanT<T> parser)
		{
			PipeReader reader = PipeReader.Create(stream);

			var i = 0;

			while (true)
			{
				ReadResult read = await reader.ReadAsync();
				ReadOnlySequence<byte> buffer = read.Buffer;
				ReadOnlySequence<byte> sequence;
				// read header row.
				TryReadLine(ref buffer, out sequence);

				while (TryReadLine(ref buffer, out sequence))
				{
					var item = ProcessSequence(sequence, parser);
					yield return item;
				}

				reader.AdvanceTo(buffer.Start, buffer.End);
				if (read.IsCompleted)
				{
					break;
				}
			}
		}

		private static bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
		{
			var position = buffer.PositionOf((byte)'\n');
			if (position == null)
			{
				line = default;
				return false;
			}

			line = buffer.Slice(0, position.Value);
			buffer = buffer.Slice(buffer.GetPosition(1, position.Value));

			return true;
		}

		private static T ProcessSequence<T>(ReadOnlySequence<byte> sequence, FuncSpanT<T> parser)
		{
			const int LengthLimit = 256;

			if (sequence.IsSingleSegment)
			{
				return Parse(sequence.FirstSpan, parser);
			}

			var length = (int)sequence.Length;
			if (length > LengthLimit)
			{
				throw new ArgumentException($"Line has a length exceeding the limit: {length}");
			}

			Span<byte> span = stackalloc byte[(int)sequence.Length];
			sequence.CopyTo(span);

			return Parse(span, parser);
		}

		private static T Parse<T>(ReadOnlySpan<byte> bytes, FuncSpanT<T> parser)
		{
			Span<char> chars = stackalloc char[bytes.Length];
			Encoding.UTF8.GetChars(bytes, chars);

			return parser(chars);
		}

		public static ReadOnlySpan<char> ParseChunk(ref ReadOnlySpan<char> span, ref int scanned, ref int position)
		{
			scanned += position + 1;

			position = span.Slice(scanned, span.Length - scanned).IndexOf(',');
			if (position < 0)
			{
				position = span.Slice(scanned, span.Length - scanned).Length;
			}

			return span.Slice(scanned, position);
		}
	}
}