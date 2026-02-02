using GBX.NET.Exceptions;
using System.Buffers.Binary;
using System.Text;

namespace GBX.NET.PAK;

internal sealed partial class AsyncGbxReader : IDisposable, IAsyncDisposable
{
    private readonly Stream stream;
    private readonly bool leaveOpen;

    private const long NadeoPakMagic = 0x6B61506F6564614E; // "NadeoPak" in little-endian

    private const int MaxDataSize = 0x10000000; // ~268MB

    private readonly byte[] primitiveBuffer = new byte[16];

    public AsyncGbxReader(Stream stream, bool leaveOpen = true)
    {
        this.stream = stream;
        this.leaveOpen = leaveOpen;
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<bool> ReadPakMagicAsync(CancellationToken cancellationToken = default)
    {
        return await ReadInt64Async(cancellationToken) == NadeoPakMagic;
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<short> ReadInt16Async(CancellationToken cancellationToken = default)
    {
        await stream.ReadExactlyAsync(primitiveBuffer.AsMemory(0, sizeof(short)), cancellationToken);
        return BinaryPrimitives.ReadInt16LittleEndian(primitiveBuffer);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<int> ReadInt32Async(CancellationToken cancellationToken = default)
    {
        await stream.ReadExactlyAsync(primitiveBuffer.AsMemory(0, sizeof(int)), cancellationToken);
        return BinaryPrimitives.ReadInt32LittleEndian(primitiveBuffer);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<long> ReadInt64Async(CancellationToken cancellationToken = default)
    {
        await stream.ReadExactlyAsync(primitiveBuffer.AsMemory(0, sizeof(long)), cancellationToken);
        return BinaryPrimitives.ReadInt64LittleEndian(primitiveBuffer);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<uint> ReadUInt32Async(CancellationToken cancellationToken = default)
    {
        await stream.ReadExactlyAsync(primitiveBuffer.AsMemory(0, sizeof(uint)), cancellationToken);
        return BinaryPrimitives.ReadUInt32LittleEndian(primitiveBuffer);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<ulong> ReadUInt64Async(CancellationToken cancellationToken = default)
    {
        await stream.ReadExactlyAsync(primitiveBuffer.AsMemory(0, sizeof(ulong)), cancellationToken);
        return BinaryPrimitives.ReadUInt64LittleEndian(primitiveBuffer);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<UInt128> ReadUInt128Async(CancellationToken cancellationToken = default)
    {
        await stream.ReadExactlyAsync(primitiveBuffer.AsMemory(0, 16), cancellationToken);
        return BinaryPrimitives.ReadUInt128LittleEndian(primitiveBuffer);
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<DateTime> ReadFileTimeAsync(CancellationToken cancellationToken = default)
    {
        return DateTime.FromFileTime(await ReadInt64Async(cancellationToken));
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<string> ReadStringAsync(CancellationToken cancellationToken = default)
    {
        var length = await ReadInt32Async(cancellationToken);

        if (length == 0)
        {
            return string.Empty;
        }

        if (length < 0)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(length);
        }

        if (length > MaxDataSize) // ~268MB
        {
            throw new LengthLimitException(length);
        }

        return Encoding.UTF8.GetString(await ReadBytesAsync(length, cancellationToken)); 
    }

    [Zomp.SyncMethodGenerator.CreateSyncVersion]
    public async Task<byte[]> ReadBytesAsync(int count, CancellationToken cancellationToken = default)
    {
        var buffer = new byte[count];
        await stream.ReadExactlyAsync(buffer.AsMemory(0, count), cancellationToken);
        return buffer;
    }

    public void Dispose()
    {
        if (leaveOpen)
        {
            return;
        }

        stream.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        if (leaveOpen)
        {
            return ValueTask.CompletedTask;
        }

        return stream.DisposeAsync();
    }
}
