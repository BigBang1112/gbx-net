namespace GBX.NET.Engines.Meta;

/// <remarks>ID: 0x2F0A9000</remarks>
[Class(0x2F0A9000)]
public partial class NPlugItemPlacement_SPlacement : SMetaPtr, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x2F0A9000;




    private int iLayout;
    public int ILayout { get => iLayout; set => iLayout = value; }

    private NPlugItemPlacement_SPlacementOption[]? options;
    public NPlugItemPlacement_SPlacementOption[]? Options { get => options; set => options = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.VersionInt32(this);
        rw.Int32(ref iLayout);
        rw.ArrayReadableWritable<NPlugItemPlacement_SPlacementOption>(ref options!);
    }




}
