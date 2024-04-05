namespace GBX.NET.Components;

public sealed class GbxRefTableResource(GbxRefTable refTable, int flags, bool useFile, int resourceIndex)
    : GbxRefTableNode(refTable, flags, useFile)
{
    public int ResourceIndex { get; } = resourceIndex;

    public override string ToString()
    {
        return $"Index: {ResourceIndex}, Flags: {Flags}, UseFile: {UseFile}";
    }
}
