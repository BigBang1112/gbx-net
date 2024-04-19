namespace GBX.NET.Components;

public sealed class UnlinkedGbxRefTableResource(int flags, bool useFile, int nodeIndex, int resourceIndex) : UnlinkedGbxRefTableNode(flags, useFile, nodeIndex)
{
    public int ResourceIndex { get; set; } = resourceIndex;
}
