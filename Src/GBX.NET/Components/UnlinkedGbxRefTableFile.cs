namespace GBX.NET.Components;

public sealed class UnlinkedGbxRefTableFile(int flags, bool useFile, int nodeIndex, string filePath) : UnlinkedGbxRefTableNode(flags, useFile, nodeIndex)
{
    public string FilePath { get; set; } = filePath;

    public override string ToString()
    {
        return FilePath;
    }
}
