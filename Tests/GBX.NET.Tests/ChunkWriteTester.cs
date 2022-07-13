using System;
using System.IO;
using System.Linq;

namespace GBX.NET.Tests;

internal class ChunkWriteTester<TNode, TChunk> : ChunkTester<TNode, TChunk>, IDisposable
    where TNode : Node where TChunk : Chunk<TNode>
{
    private readonly MemoryStream ms;
    private readonly GameBoxWriter writer;
    private readonly byte[] expectedData;

    public ChunkWriteTester(string gameVersion, bool idWasWritten = false) : base(gameVersion, idWasWritten)
    {
        ms = new MemoryStream();
        writer = new GameBoxWriter(ms, gbx: Gbx);
        expectedData = File.ReadAllBytes(GetChunkFileName());
    }

    public void Write()
    {
        Chunk.Write(Node, writer);
    }

    public void ReadWriteWithWriter()
    {
        Chunk.ReadWrite(Node, new GameBoxReaderWriter(writer));
    }

    public bool ExpectedDataEqualActualData()
    {
        return expectedData.SequenceEqual(ms.ToArray());
    }

    public void Dispose()
    {
        writer.Dispose();
        ms.Dispose();
    }
}
