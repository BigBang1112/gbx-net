using GBX.NET.Components;
using Microsoft.Extensions.Logging;

namespace GBX.NET.Serialization;

internal sealed class GbxRefTableWriter(GbxRefTable refTable, GbxHeader header, GbxWriter writer)
{
    private readonly ILogger? logger = writer.Logger;

    internal bool Write(bool rawBody)
    {
        return rawBody ? WriteUsingRefTableData() : WriteUsingNodeDictionary();
    }

    private bool WriteUsingRefTableData()
    {
        var numExternalNodes = refTable.Files.Count + refTable.Resources.Count;
        writer.Write(numExternalNodes);

        logger?.LogDebug("Number of external nodes: {NumExternalNodes}", numExternalNodes);

        if (numExternalNodes == 0)
        {
            return true;
        }

        using var _ = logger?.BeginScope("RefTable");

        writer.Write(refTable.AncestorLevel);

        logger?.LogDebug("Ancestor level: {AncestorLevel}", refTable.AncestorLevel);

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

        root.Write(writer, logger);

        var dirIndexDict = new Dictionary<Dir, int>();

        foreach (var dir in root.Flatten())
        {
            dirIndexDict[dir] = dirIndexDict.Count;
        }

        foreach (var file in refTable.Files)
        {
            logger?.LogInformation("External file: {File}, Flags: {Flags}, UseFile: {UseFile}", file.FilePath, file.Flags, file.UseFile);

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
            logger?.LogInformation("External resource: Index: {ResourceIndex}, Flags: {Flags}, UseFile: {UseFile}", resource.ResourceIndex, resource.Flags, resource.UseFile);

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

        logger?.LogDebug("Number of external nodes: {NumExternalNodes}", externalNodesCount);

        if (externalNodesCount == 0)
        {
            return true;
        }

        using var _ = logger?.BeginScope("RefTable");

        writer.Write(refTable.AncestorLevel);

        logger?.LogDebug("Ancestor level: {AncestorLevel}", refTable.AncestorLevel);

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

        root.Write(writer, logger);

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
                    logger?.LogInformation("External resource: {Resource}", resource);
                    writer.Write(resource.ResourceIndex);
                    break;
                case GbxRefTableFile file:
                    logger?.LogInformation("External file: {File}", file);
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

        public void Write(GbxWriter writer, ILogger? logger, string path = "")
        {
            writer.Write(Children.Count);

            foreach (var dir in Children)
            {
                writer.Write(dir.Name);

                var subPath = Path.Combine(path, dir.Name);
                logger?.LogTrace("Dir: {Dir}", subPath);

                dir.Write(writer, logger, subPath);
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
