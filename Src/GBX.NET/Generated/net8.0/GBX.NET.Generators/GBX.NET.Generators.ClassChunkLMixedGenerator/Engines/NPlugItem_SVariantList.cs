namespace GBX.NET.Engines.Meta;

/// <remarks>ID: 0x2F0BC000</remarks>
[Class(0x2F0BC000)]
public partial class NPlugItem_SVariantList : CMwNod, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x2F0BC000;




    private NPlugItem_SVariant[]? variants;
    public NPlugItem_SVariant[]? Variants { get => variants; set => variants = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.VersionInt32(this);
        rw.ArrayReadableWritable<NPlugItem_SVariant>(ref variants!, version: Version);
    }




}
