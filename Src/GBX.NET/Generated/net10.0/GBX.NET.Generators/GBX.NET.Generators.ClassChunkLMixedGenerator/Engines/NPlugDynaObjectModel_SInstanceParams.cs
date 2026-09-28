namespace GBX.NET.Engines.Meta;

/// <remarks>ID: 0x2F0B6000</remarks>
[Class(0x2F0B6000)]
public partial class NPlugDynaObjectModel_SInstanceParams : SMetaPtr, IClass, IReadableWritable
{
    [Hexadecimal] public static new uint Id => 0x2F0B6000;




    private float periodSc;
    public float PeriodSc { get => periodSc; set => periodSc = value; }

    private int textureId;
    public int TextureId { get => textureId; set => textureId = value; }

    private bool isKinematic;
    public bool IsKinematic { get => isKinematic; set => isKinematic = value; }

    private float periodScMax;
    public float PeriodScMax { get => periodScMax; set => periodScMax = value; }

    private float phase01;
    public float Phase01 { get => phase01; set => phase01 = value; }

    private float phase01Max;
    public float Phase01Max { get => phase01Max; set => phase01Max = value; }

    private bool castStaticShadow;
    public bool CastStaticShadow { get => castStaticShadow; set => castStaticShadow = value; }

    public void ReadWrite(GbxReaderWriter rw, int v = 0)
    {
        rw.VersionInt32(this);
        rw.Single(ref periodSc);
        rw.Int32(ref textureId);
        rw.Boolean(ref isKinematic);
        if (Version>=1)
        {
            rw.Single(ref periodScMax);
            rw.Single(ref phase01);
            rw.Single(ref phase01Max);
            if (Version>=2)
            {
                rw.Boolean(ref castStaticShadow);
            }
        }
    }




}
