namespace GBX.NET.Components;

public abstract class GbxRefTableNode
{
    protected GbxRefTable RefTable { get; }

    public int Flags { get; }
    public bool UseFile { get; }

    protected GbxRefTableNode(GbxRefTable refTable, int flags, bool useFile)
    {
        RefTable = refTable;
        Flags = flags;
        UseFile = useFile;
    }
}
