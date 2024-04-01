namespace GBX.NET.Components;

public sealed class GbxRefTableFile : GbxRefTableNode
{
    public string FilePath { get; set; }

    public GbxRefTableFile(GbxRefTable refTable, int flags, bool useFile, string filePath) : base(refTable, flags, useFile)
    {
        FilePath = filePath;
    }

    public override string ToString()
    {
        return FilePath;
    }
}
