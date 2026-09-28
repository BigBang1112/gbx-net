namespace GBX.NET.Engines.Meta;

/// <remarks>ID: 0x2F0C8000</remarks>
[Class(0x2F0C8000)]
public partial class NPlugDyna_SPrefabConstraintParams : SMetaPtr, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x2F0C8000;




    private int ent1;
    public int Ent1 { get => ent1; set => ent1 = value; }

    private int ent2;
    public int Ent2 { get => ent2; set => ent2 = value; }

    private Vec3 pos1;
    public Vec3 Pos1 { get => pos1; set => pos1 = value; }

    private Vec3 pos2;
    public Vec3 Pos2 { get => pos2; set => pos2 = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.VersionInt32(this);
        rw.Int32(ref ent1);
        rw.Int32(ref ent2);
        rw.Vec3(ref pos1);
        rw.Vec3(ref pos2);
    }




}
