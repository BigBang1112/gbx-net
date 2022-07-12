using System;
using System.IO;
using System.Linq;

namespace GBX.NET.Tests;

internal class ChunkWriteTester<TNode, TChunk> : ChunkTester<TNode, TChunk>, IDisposable
    where TNode : Node where TChunk : Chunk<TNode>
{
    private readonly MemoryStream ms;

    public GameBoxWriter Writer { get; }
    public byte[] ExpectedData { get; }

    public ChunkWriteTester(string gameVersion, bool idWasWritten = false) : base(gameVersion, idWasWritten)
    {
        ms = new MemoryStream();
        Writer = new GameBoxWriter(ms, gbx: Gbx);
        ExpectedData = File.ReadAllBytes(GetChunkFileName());
    }

    public void Write()
    {
        Chunk.Write(Node, Writer);
    }

    public void ReadWriteWithWriter()
    {
        Chunk.ReadWrite(Node, new GameBoxReaderWriter(Writer));
    }

    public bool ExpectedDataEqualActualData()
    {
        return ExpectedData.SequenceEqual(ms.ToArray());
    }

    public void Dispose()
    {
        Writer.Dispose();
        ms.Dispose();
    }
}
