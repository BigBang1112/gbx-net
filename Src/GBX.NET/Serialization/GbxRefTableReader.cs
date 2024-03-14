﻿using GBX.NET.Components;

namespace GBX.NET.Serialization;

internal sealed class GbxRefTableReader(GbxReader reader, GbxHeader header, GbxReadSettings settings)
{
    public GbxRefTable? Parse()
    {
        var numExternalNodes = reader.ReadInt32();

        if (numExternalNodes == 0)
        {
            return null;
        }

        var ancestorLevel = reader.ReadInt32();

        var root = new GbxRefTableDirectory();

        var refTable = new GbxRefTable(root, ancestorLevel);

        var directoryList = ReadChildren(root).ToList();

        var resources = new Dictionary<int, GbxRefTableResource>();
        var refTableForReader = new Dictionary<int, GbxRefTableFile>();

        for (var i = 0; i < numExternalNodes; i++)
        {
            var flags = reader.ReadInt32();
            var name = default(string);
            var resourceIndex = default(int?);
            var folderIndex = default(int?);

            if ((flags & 4) == 0)
            {
                name = reader.ReadString();
            }
            else
            {
                resourceIndex = reader.ReadInt32();
            }

            var nodeIndex = reader.ReadInt32();
            var useFile = header.Basic.Version >= 5 && reader.ReadBoolean();
            
            if ((flags & 4) == 0)
            {
                folderIndex = reader.ReadInt32();
            }

            if (name is not null && folderIndex.HasValue)
            {
                var dir = directoryList[folderIndex.Value - 1];
                var file = new GbxRefTableFile(refTable, name, dir);
                dir.Files.Add(file);
                refTableForReader.Add(nodeIndex, file);
            }
            else if (resourceIndex.HasValue)
            {
                resources.Add(nodeIndex, new GbxRefTableResource());
            }
        }

        reader.LoadRefTable(refTableForReader);

        return refTable;
    }

    private IEnumerable<IDirectory> ReadChildren(IDirectory currentDir)
    {
        var numChildren = reader.ReadInt32();

        for (var i = 0; i < numChildren; i++)
        {
            var name = reader.ReadString();
            var subDir = new GbxRefTableDirectory { Name = name, Parent = currentDir };
            currentDir.Children.Add(subDir);

            yield return subDir;
            
            foreach (var dir in ReadChildren(subDir))
            {
                yield return dir;
            }
        }
    }
}
