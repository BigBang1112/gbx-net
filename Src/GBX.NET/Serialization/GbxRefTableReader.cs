using GBX.NET.Components;
using static System.Net.Mime.MediaTypeNames;

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

        var refTable = new GbxRefTable
        {
            AncestorLevel = reader.ReadInt32()
        };

        var root = new Dir(string.Empty, Parent: null);

        var directoryList = ReadChildren(root).ToList();

        var refTableForReader = new Dictionary<int, GbxRefTableNode>();

        for (var i = 0; i < numExternalNodes; i++)
        {
            var flags = reader.ReadInt32();
            var isResource = (flags & 4) != 0;

            var name = default(string);
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
                refTableForReader.Add(nodeIndex, new GbxRefTableResource(refTable, flags, useFile, resourceIndex.GetValueOrDefault()));
                continue;
            }

            var dir = directoryList[reader.ReadInt32() - 1];
            var relativePath = Path.Combine(dir.ToString(), name);

            refTableForReader.Add(nodeIndex, new GbxRefTableFile(refTable, flags, useFile, relativePath));
        }

        reader.LoadRefTable(refTableForReader);

        return refTable;
    }

    private IEnumerable<Dir> ReadChildren(Dir currentDir)
    {
        var numChildren = reader.ReadInt32();

        for (var i = 0; i < numChildren; i++)
        {
            var name = reader.ReadString();
            var subDir = new Dir(name, currentDir);

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
