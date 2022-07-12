using System;
using System.IO;

namespace GBX.NET.Tests;

internal class ChunkReadTester<TNode, TChunk> : ChunkTester<TNode, TChunk>, IDisposable
    where TNode : Node where TChunk : Chunk<TNode>
{
    private readonly FileStream fs;

    public GameBoxReader Reader { get; }

    public ChunkReadTester(string gameVersion, bool idWasWritten = false) : base(gameVersion, idWasWritten)
    {
        fs = File.OpenRead(GetChunkFileName());
        Reader = new GameBoxReader(fs, gbx: Gbx);
    }

    public void Read()
    {
        Chunk.Read(Node, Reader);
    }

    public void ReadWriteWithReader()
    {
        Chunk.ReadWrite(Node, new GameBoxReaderWriter(Reader));
    }

    public void Dispose()
    {
        Reader.Dispose();
        fs.Dispose();
    }
}
