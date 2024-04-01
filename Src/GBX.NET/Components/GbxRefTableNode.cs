namespace GBX.NET.Components;

public abstract class GbxRefTableNode
{
    private readonly GbxRefTable refTable;

    public int Flags { get; }
    public bool UseFile { get; }

    protected GbxRefTableNode(GbxRefTable refTable, int flags, bool useFile)
    {
        this.refTable = refTable;

        Flags = flags;
        UseFile = useFile;
    }
}
