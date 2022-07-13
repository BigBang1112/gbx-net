using System;
using System.IO;
using System.Linq;

namespace GBX.NET.Tests;

internal class ChunkReadWriteEqualityTester<TNode, TChunk> : ChunkTester<TNode, TChunk>, IDisposable
    where TNode : Node where TChunk : Chunk<TNode>
{
    private readonly byte[] inputData;
    private readonly MemoryStream outputStream;

    public ChunkReadWriteEqualityTester(string gameVersion, bool idWasWritten) : base(gameVersion, idWasWritten)
    {
        inputData = File.ReadAllBytes(GetChunkFileName());
        outputStream = new MemoryStream();
    }

    public void ReadWrite()
    {
        using var inputStream = new MemoryStream(inputData);
        using var inputReader = new GameBoxReader(inputStream);
        using var outputWriter = new GameBoxWriter(outputStream);
        var rw = new GameBoxReaderWriter(inputReader, outputWriter);

        Chunk.ReadWrite(Node, rw);
    }

    public bool ReadWriteIsEqual()
    {
        return inputData.SequenceEqual(outputStream.ToArray());
    }

    public void Dispose()
    {
        outputStream.Dispose();
    }
}
