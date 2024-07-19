namespace GBX.NET.Engines.Meta;

public partial class NPlugItemPlacement_SPlacement : IVersionable
{
    public int Version { get; set; }

#if NET8_0_OR_GREATER
    static void IClass.Read<T>(T node, GbxReaderWriter rw)
    {
        node.ReadWrite(rw);
    }
#endif

    public override void ReadWrite(GbxReaderWriter rw)
    {
        ReadWrite(rw, v: 0);
    }
}
