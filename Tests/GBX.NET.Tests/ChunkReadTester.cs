using System;
using System.IO;

namespace GBX.NET.Tests;

internal class ChunkReadTester<TNode, TChunk> : ChunkTester<TNode, TChunk>, IDisposable
    where TNode : Node where TChunk : Chunk<TNode>
{
    private readonly FileStream fs;
    private readonly GameBoxReader reader;

    public ChunkReadTester(string gameVersion, bool idWasWritten = false) : base(gameVersion, idWasWritten)
    {
        fs = File.OpenRead(GetChunkFileName());
        reader = new GameBoxReader(fs, gbx: Gbx);
    }

    public void Read()
    {
        Chunk.Read(Node, reader);
    }

    public void ReadWriteWithReader()
    {
        Chunk.ReadWrite(Node, new GameBoxReaderWriter(reader));
    }

    public void Dispose()
    {
        reader.Dispose();
        fs.Dispose();
    }
}
