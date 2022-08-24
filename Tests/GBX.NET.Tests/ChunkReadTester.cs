using System;
using System.IO;

namespace GBX.NET.Tests;

internal class ChunkReadTester<TNode, TChunk> : ChunkTester<TNode, TChunk>, IDisposable
    where TNode : Node where TChunk : Chunk<TNode>
{
    private readonly Stream stream;
    private readonly GameBoxReader reader;

    public ChunkReadTester(string gameVersion) : base(gameVersion)
    {
        stream = ChunkEntry.Open();
        reader = new GameBoxReader(stream, default, default, default, State);
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
