namespace GBX.NET.Components;

public sealed class GbxRefTableFile(GbxRefTable refTable, string name, IDirectory parent) : IFile
{
    internal GbxRefTable RefTable { get; } = refTable;

    public string Name { get; set; } = name;
    public IDirectory Parent { get; set; } = parent;
    public CMwNod? Node { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
