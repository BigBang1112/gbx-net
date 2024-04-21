using GBX.NET.Components;
using Microsoft.Extensions.Logging;

namespace GBX.NET.Serialization;

internal sealed class GbxRefTableReader(GbxReader reader, GbxHeader header, string? fileSystemPath)
{
    private readonly ILogger? logger = reader.Settings.Logger;

    private GbxReadSettings Settings => reader.Settings;

    public GbxRefTable? Parse()
    {
        var numExternalNodes = reader.ReadInt32();

        logger?.LogDebug("Number of external nodes: {NumExternalNodes}", numExternalNodes);

        if (numExternalNodes == 0)
        {
            return null;
        }

        using var _ = logger?.BeginScope("RefTable");

        var refTable = new GbxRefTable
        {
            AncestorLevel = reader.ReadInt32(),
            FileSystemPath = fileSystemPath
        };

        logger?.LogDebug("Ancestor level: {AncestorLevel}", refTable.AncestorLevel);

        var root = new Dir(string.Empty, Parent: null);

        var directoryList = ReadChildren(root).ToList();

        var refTableForReader = new Dictionary<int, GbxRefTableNode>();

        var unlinkedFiles = new List<UnlinkedGbxRefTableFile>();
        var unlinkedResources = new List<UnlinkedGbxRefTableResource>();

        for (var i = 0; i < numExternalNodes; i++)
        {
            var flags = reader.ReadInt32();
            var isResource = (flags & 4) != 0;

            var name = string.Empty;
            var resourceIndex = default(int?);

            if (isResource)
            {
                resourceIndex = reader.ReadInt32();
            }
            else
            {
                name = reader.ReadString();
            }

            var nodeIndex = reader.ReadInt32();
            var useFile = header.Basic.Version >= 5 && reader.ReadBoolean();

            if (isResource)
            {
                var resource = new GbxRefTableResource(refTable, flags, useFile, resourceIndex.GetValueOrDefault());
                logger?.LogInformation("External resource: {Resource}", resource);

                refTableForReader.Add(nodeIndex, resource);
                unlinkedResources.Add(new UnlinkedGbxRefTableResource(flags, useFile, nodeIndex, resourceIndex.GetValueOrDefault()));
                continue;
            }

            var dirIndex = reader.ReadInt32() - 1;
            var filePath = dirIndex == -1 ? name : Path.Combine(directoryList[dirIndex].ToString(), name);

            var file = new GbxRefTableFile(refTable, flags, useFile, filePath);
            logger?.LogInformation("External file: {File}", file);

            refTableForReader.Add(nodeIndex, file);

            unlinkedFiles.Add(new UnlinkedGbxRefTableFile(flags, useFile, nodeIndex, filePath));
        }

        reader.LoadRefTable(refTableForReader);

        refTable.Files = unlinkedFiles;
        refTable.Resources = unlinkedResources;

        return refTable;
    }

    private IEnumerable<Dir> ReadChildren(Dir currentDir)
    {
        var numChildren = reader.ReadInt32();

        for (var i = 0; i < numChildren; i++)
        {
            var name = reader.ReadString();
            var subDir = new Dir(name, currentDir);

            logger?.LogTrace("Dir: {Dir}", subDir);

            yield return subDir;
            
            foreach (var dir in ReadChildren(subDir))
            {
                yield return dir;
            }
        }
    }

    private sealed record Dir(string Name, Dir? Parent)
    {
        public override string ToString()
        {
            if (Parent is null)
            {
                return Name;
            }

            return Path.Combine(Parent.ToString(), Name);
        }
    }
}
