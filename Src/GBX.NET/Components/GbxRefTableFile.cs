namespace GBX.NET.Components;

public sealed class GbxRefTableFile : GbxRefTableNode
{
    public string RelativePath { get; set; }

    public GbxRefTableFile(GbxRefTable refTable, int flags, bool useFile, string relativePath) : base(refTable, flags, useFile)
    {
        RelativePath = relativePath;
    }

    public override string ToString()
    {
        return RelativePath;
    }
}
