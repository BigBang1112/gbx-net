using GBX.NET.Components;

namespace GBX.NET.Serialization;

internal sealed class GbxRefTableWriter(GbxRefTable refTable, GbxHeader header, GbxWriter writer, GbxWriteSettings settings)
{
    internal bool Write(bool rawBody)
    {
        return rawBody ? WriteUsingRefTableData() : WriteUsingNodeDictionary();
    }

    private bool WriteUsingRefTableData()
    {
        var numExternalNodes = refTable.Files.Count + refTable.Resources.Count;
        writer.Write(numExternalNodes);

        if (numExternalNodes == 0)
        {
            return true;
        }

        writer.Write(refTable.AncestorLevel);

        var root = new Dir();

        var fileDirDict = new Dictionary<UnlinkedGbxRefTableFile, Dir?>();

        foreach (var file in refTable.Files)
        {
            var parts = file.FilePath.Split('/', '\\');

            if (parts.Length == 1)
            {
                fileDirDict[file] = null;
                continue;
            }

            var dir = root.InsertPath(parts, 0) ?? throw new Exception("Failed to insert path into directory tree");

            fileDirDict[file] = dir;
        }

        root.Write(writer);

        var dirIndexDict = new Dictionary<Dir, int>();

        foreach (var dir in root.Flatten())
        {
            dirIndexDict[dir] = dirIndexDict.Count;
        }

        foreach (var file in refTable.Files)
        {
            writer.Write(file.Flags);
            writer.Write(Path.GetFileName(file.FilePath));
            writer.Write(file.NodeIndex);

            if (header.Basic.Version >= 5)
            {
                writer.Write(file.UseFile);
            }

            var dir = fileDirDict[file];
            writer.Write(dir is null ? 0 : dirIndexDict[dir] + 1);
        }

        foreach (var resource in refTable.Resources)
        {
            writer.Write(resource.Flags);
            writer.Write(resource.ResourceIndex);
            writer.Write(resource.NodeIndex);

            if (header.Basic.Version >= 5)
            {
                writer.Write(resource.UseFile);
            }
        }

        return true;
    }

    private bool WriteUsingNodeDictionary()
    {
        var nodes = writer.NodeDict.Keys;

        var externalNodesCount = nodes.OfType<GbxRefTableNode>().Count();

        writer.Write(externalNodesCount);

        if (externalNodesCount == 0)
        {
            return true;
        }

        writer.Write(refTable.AncestorLevel);

        var root = new Dir();

        var fileDirDict = new Dictionary<GbxRefTableFile, Dir?>();

        foreach (var file in nodes.OfType<GbxRefTableFile>())
        {
            var parts = file.FilePath.Split('/', '\\');

            if (parts.Length == 1)
            {
                fileDirDict[file] = null;
                continue;
            }

            var dir = root.InsertPath(parts, 0) ?? throw new Exception("Failed to insert path into directory tree");

            fileDirDict[file] = dir;
        }

        root.Write(writer);

        var dirIndexDict = new Dictionary<Dir, int>();

        foreach (var dir in root.Flatten())
        {
            dirIndexDict[dir] = dirIndexDict.Count;
        }

        foreach (var pair in writer.NodeDict)
        {
            if (pair.Key is not GbxRefTableNode node)
            {
                continue;
            }

            var nodeIndex = pair.Value;

            writer.Write(node.Flags);

            switch (node)
            {
                case GbxRefTableResource resource:
                    writer.Write(resource.ResourceIndex);
                    break;
                case GbxRefTableFile file:
                    writer.Write(Path.GetFileName(file.FilePath));
                    break;
                default:
                    throw new InvalidOperationException("Unknown external node type");
            }

            writer.Write(nodeIndex);

            if (header.Basic.Version >= 5)
            {
                writer.Write(node.UseFile);
            }

            if (node is GbxRefTableFile fileNode)
            {
                var dir = fileDirDict[fileNode];
                writer.Write(dir is null ? 0 : dirIndexDict[dir] + 1);
            }
        }

        return true;
    }

    private class Dir
    {
        public string Name { get; set; } = string.Empty;
        public List<Dir> Children { get; set; } = [];

        public Dir? InsertPath(string[] parts, int partIndex, Dir? subDir = null)
        {
            if (partIndex == parts.Length - 1)
            {
                return subDir;
            }

            subDir = Children.FirstOrDefault(d => d.Name == parts[partIndex]);

            if (subDir is null)
            {
                subDir = new Dir { Name = parts[partIndex] };
                Children.Add(subDir);
            }

            return subDir.InsertPath(parts, partIndex + 1, subDir);
        }

        public void Write(GbxWriter writer)
        {
            writer.Write(Children.Count);

            foreach (var dir in Children)
            {
                writer.Write(dir.Name);
                dir.Write(writer);
            }
        }

        public IEnumerable<Dir> Flatten()
        {
            foreach (var dir in Children)
            {
                yield return dir;

                foreach (var child in dir.Flatten())
                {
                    yield return child;
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
