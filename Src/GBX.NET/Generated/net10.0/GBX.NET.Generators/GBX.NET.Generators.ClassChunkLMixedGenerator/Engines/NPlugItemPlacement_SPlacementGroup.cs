namespace GBX.NET.Engines.Meta;

/// <remarks>ID: 0x2F0D8000</remarks>
[Class(0x2F0D8000)]
public partial class NPlugItemPlacement_SPlacementGroup : SMetaPtr, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x2F0D8000;




    private NPlugItemPlacement_SPlacement[]? placements;
    public NPlugItemPlacement_SPlacement[]? Placements { get => placements; set => placements = value; }

    private short[]? u01;
    public short[]? U01 { get => u01; set => u01 = value; }

    private TransQuat[]? u02;
    public TransQuat[]? U02 { get => u02; set => u02 = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.VersionInt32(this);
        rw.ArrayReadableWritable<NPlugItemPlacement_SPlacement>(ref placements!);
        rw.Array<short>(ref u01!);
        rw.Array<TransQuat>(ref u02!);
    }




}
