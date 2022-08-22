using GBX.NET.Extensions;
using GBX.NET.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace GBX.NET.Tests;

internal abstract class ChunkTester<TNode, TChunk> : IDisposable where TNode : Node where TChunk : Chunk<TNode>
{
    public string GameVersion { get; }
    public TNode Node { get; }
    public TChunk Chunk { get; }
    public string NodeName { get; }
    public string ChunkName { get; }
    public ZipArchive Zip { get; }
    public ZipArchiveEntry ChunkEntry { get; }
    public int? IdVersion { get; set; }
    public bool IdIsWritten { get; set; }
    public List<string>? IdStrings { get; set; }

    public ChunkTester(string gameVersion)
    {
        GameVersion = gameVersion;
        Node = NodeCacheManager.GetNodeInstance<TNode>();

        if (typeof(TChunk).GetInterface(nameof(IHeaderChunk)) is not null)
        {
            if (Node is not INodeHeader nodeHeader)
            {
                throw new Exception("Testing a chunk in a node that doesn't implement INodeHeader.");
            }

            Chunk = nodeHeader.HeaderChunks.Create<TChunk>();
        }
        else
        {
            Chunk = Node.CreateChunk<TChunk>();
        }

        NodeName = Node.GetType().Name;
        ChunkName = Chunk.GetType().Name;

        IdVersion = 3;
        IdIsWritten = true;

        if (!File.Exists(GetZipPath()))
        {
            throw new FileNotFoundException($"Class ZIP file not found for {NodeName}, game version {gameVersion}.");
        }

        Zip = ZipFile.OpenRead(GetZipPath());
        ChunkEntry = Zip.GetEntry($"{ChunkName}.dat") ?? throw new Exception($"Chunk entry not found for {NodeName}.{ChunkName}, game version {gameVersion}.");
    }

    public void SetIdState(int? version = 3, IEnumerable<string>? strings = null)
    {
        IdVersion = version;
        IdIsWritten = version.HasValue;

        if (strings is not null)
        {
            IdStrings = strings.ToList();
        }
    }

    public string GetZipPath()
    {
        return Path.Combine("TestData", "Chunks", $"{NodeName}.{GameVersion}.zip");
    }

    public byte[] GetChunkData()
    {
        return GetEntryData(ChunkEntry);
    }

    public ZipArchiveEntry? GetAdditionalEntry(string entryName)
    {
        return Zip.GetEntry($"{ChunkName}.{entryName}.dat");
    }

    public byte[]? GetAdditionalEntryData(string entryName)
    {
        var entry = GetAdditionalEntry(entryName);

        if (entry is null)
        {
            return null;
        }
        
        return GetEntryData(entry);
    }

    public virtual void Dispose()
    {
        Zip.Dispose();
    }

    private static byte[] GetEntryData(ZipArchiveEntry entry)
    {
        using var ms = new MemoryStream();
        using var entryStream = entry.Open();
        entryStream.CopyTo(ms);
        return ms.ToArray();
    }
}
