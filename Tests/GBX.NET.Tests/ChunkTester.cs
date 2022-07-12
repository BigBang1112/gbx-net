using GBX.NET.Managers;
using System;
using System.IO;

namespace GBX.NET.Tests;

internal abstract class ChunkTester<TNode, TChunk> where TNode : Node where TChunk : Chunk<TNode>
{
    public string GameVersion { get; }
    public TNode Node { get; }
    public TChunk Chunk { get; }
    public string NodeName { get; }
    public string ChunkName { get; }
    public GameBox<TNode> Gbx { get; }

    public ChunkTester(string gameVersion, bool idWasWritten)
    {
        GameVersion = gameVersion;
        Node = NodeCacheManager.GetNodeInstance<TNode>();
        Chunk = Node.CreateChunk<TChunk>();

        NodeName = Node.GetType().Name;
        ChunkName = Chunk.GetType().Name;

        Gbx = new GameBox<TNode>(Node)
        {
            IdIsWritten = idWasWritten
        };

        if (!File.Exists(GetChunkFileName()))
        {
            throw new FileNotFoundException($"Chunk file not found for {NodeName}.{ChunkName}, game version {gameVersion}.");
        }
    }

    public string GetBasePath()
    {
        return Path.Combine("TestData", "Chunks", $"{NodeName}.{GameVersion}");
    }

    public string GetChunkFileName()
    {
        return Path.Combine(GetBasePath(), $"{NodeName}+{ChunkName}.dat");
    }

    public string GetAdditionalFileName(string identifier)
    {
        return Path.Combine(GetBasePath(), $"{NodeName}+{ChunkName}.{identifier}.dat");
    }
}
