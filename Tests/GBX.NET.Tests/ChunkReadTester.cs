using System;
using System.IO;

namespace GBX.NET.Tests;

internal class ChunkReadTester<TNode, TChunk> : ChunkTester<TNode, TChunk>, IDisposable
    where TNode : Node where TChunk : Chunk<TNode>
{
    private readonly Stream stream;
    private readonly GameBoxReader reader;

    public ChunkReadTester(string gameVersion, bool idVersionWasWritten = false) : base(gameVersion, idVersionWasWritten)
    {
        stream = ChunkEntry.Open();
        reader = new GameBoxReader(stream, gbx: Gbx);
    }

    public void Read()
    {
        Chunk.Read(Node, reader);
    }

    public void ReadWriteWithReader()
    {
        Chunk.ReadWrite(Node, new GameBoxReaderWriter(reader));
    }

    public override void Dispose()
    {
        base.Dispose();
        reader.Dispose();
        stream.Dispose();
    }
}
