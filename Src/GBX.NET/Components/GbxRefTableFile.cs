namespace GBX.NET.Components;

public sealed class GbxRefTableFile : GbxRefTableNode
{
    public string FilePath { get; set; }

    public GbxRefTableFile(GbxRefTable refTable, int flags, bool useFile, string filePath) : base(refTable, flags, useFile)
    {
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    public override string ToString()
    {
        return $"{FilePath}, Flags: {Flags}, UseFile: {UseFile}";
    }

    public T? GetNode<T>(ref T? cachedNode, GbxReadSettings settings = default, bool exceptions = false) where T : CMwNod
    {
        if (cachedNode is not null)
        {
            return cachedNode;
        }

        return cachedNode = RefTable.LoadNode<T>(this, settings, exceptions);
    }

    public string GetFullPath()
    {
        return RefTable.GetFullFilePath(this);
    }
}
