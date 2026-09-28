namespace GBX.NET.Engines.Meta;

/// <remarks>ID: 0x2F0D9000</remarks>
[Class(0x2F0D9000)]
public partial class NPlugStaticObjectModel_SInstanceParams : SMetaPtr, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x2F0D9000;




    private float phase01;
    public float Phase01 { get => phase01; set => phase01 = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.VersionInt32(this);
        rw.Single(ref phase01);
    }




}
