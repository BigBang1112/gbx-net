namespace GBX.NET.Tests.Mocks;

internal class NonSeekableStream : MemoryStream
{
    public override bool CanSeek => false;

    public NonSeekableStream(byte[] buffer, int position = 0) : base(buffer)
    {
        Position = position;
    }
}