using System.IO;

namespace Benchmarks;

sealed class NoCloseStream : Stream
{
	Stream inner;
	public NoCloseStream(Stream inner)
	{
		this.inner = inner;
	}

	public override bool CanRead => this.inner.CanRead;

	public override bool CanSeek => this.inner.CanSeek;

	public override bool CanWrite => this.inner.CanWrite;

	public override long Length => this.inner.Length;

	public override long Position { get => this.inner.Position; set => this.inner.Position = value; }

	public override void Flush()
	{
		this.inner.Flush();
	}

	public override void Close()
	{
		// NOPE
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return this.inner.Read(buffer, offset, count);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return this.inner.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		this.inner.SetLength(value);
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		this.inner.Write(buffer, offset, count);
	}
}