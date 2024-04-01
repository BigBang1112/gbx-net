using GBX.NET.Components;

namespace GBX.NET.Serialization;

internal sealed class GbxRefTableWriter(GbxRefTable refTable, GbxWriter writer, GbxWriteSettings settings)
{
    internal bool Write()
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

        foreach (var file in nodes.OfType<GbxRefTableFile>())
        {
            var parts = file.RelativePath.Split('/', '\\');
            root.InsertPath(parts, 0);
        }

        root.Write(writer);

        return true;
    }

    private class Dir
    {
        public string Name { get; set; } = string.Empty;
        public List<Dir> Children { get; set; } = [];

        public void InsertPath(string[] parts, int index)
        {
            if (index == parts.Length - 1)
            {
                return;
            }

            var subDir = Children.FirstOrDefault(d => d.Name == parts[index]);

            if (subDir is null)
            {
                subDir = new Dir { Name = parts[index] };
                Children.Add(subDir);
            }

            subDir.InsertPath(parts, index + 1);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Children.Count);

            foreach (var dir in Children)
            {
                writer.Write(dir.Name);
                dir.Write(writer);
            }
        }
    }

    /*private void WriteChildren(IDirectory root)
    {
        writer.Write(root.Children.Count);

        foreach (var child in root.Children)
        {
            writer.Write(child.Name);
            WriteChildren(child);
        }
    }

    private IEnumerable<IDirectory> FlattenChildren()
    {
        var stack = new Stack<IDirectory>();
        stack.Push(refTable.Root);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            yield return current;

            foreach (var child in current.Children)
            {
                stack.Push(child);
            }
        }
    }

    private IEnumerable<IFile> GetFiles() => FlattenChildren().SelectMany(x => x.Files);*/
}
