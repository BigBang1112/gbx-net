namespace GBX.NET.Tests.Mocks;

internal sealed class AsyncReadTrackingStream(byte[] data, CancellationTokenSource? cancelOnRead = null) : MemoryStream(data, writable: false)
{
    public int AsyncReadCount { get; private set; }
    public CancellationToken LastCancellationToken { get; private set; }

    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        OnAsyncRead(cancellationToken);
        return base.ReadAsync(buffer, offset, Math.Min(count, 7), cancellationToken);
    }

    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        OnAsyncRead(cancellationToken);
        return base.ReadAsync(buffer[..Math.Min(buffer.Length, 7)], cancellationToken);
    }

    private void OnAsyncRead(CancellationToken cancellationToken)
    {
        AsyncReadCount++;
        LastCancellationToken = cancellationToken;
        cancelOnRead?.Cancel();
        cancellationToken.ThrowIfCancellationRequested();
    }
}
