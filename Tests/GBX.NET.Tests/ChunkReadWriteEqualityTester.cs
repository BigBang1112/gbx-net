using System;
using System.IO;

namespace GBX.NET.Tests;

internal class ChunkReadWriteEqualityTester<TNode, TChunk> : ChunkTester<TNode, TChunk>, IDisposable
    where TNode : Node where TChunk : Chunk<TNode>
{
    public byte[] InputData { get; }
    public MemoryStream OutputStream { get; }

    public ChunkReadWriteEqualityTester(string gameVersion) : base(gameVersion)
    {
        InputData = GetChunkData();
        OutputStream = new MemoryStream();
    }

    public void ReadWrite()
    {
        using var inputStream = new MemoryStream(InputData);
        using var inputReader = new GameBoxReader(inputStream);
        using var outputWriter = new GameBoxWriter(OutputStream);
        var rw = new GameBoxReaderWriter(inputReader, outputWriter);

        Chunk.ReadWrite(Node, rw);
    }

    public override void Dispose()
    {
        base.Dispose();
        OutputStream.Dispose();
    }
}
