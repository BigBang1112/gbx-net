namespace GBX.NET.Engines.Plug;

/// <summary>
/// Bitmap sampler.
/// </summary>
/// <remarks>ID: 0x0907E000</remarks>
[Node(0x0907E000)]
public class CPlugBitmapSampler : CPlug
{
    private string name = "";

    [NodeMember]
    [AppliedWithChunk<Chunk0907E002>]
    [AppliedWithChunk<Chunk0907E005>]
    [AppliedWithChunk<Chunk0907E006>]
    [AppliedWithChunk<Chunk0907E008>]
    public string Name { get => name; set => name = value; }

    internal CPlugBitmapSampler()
    {

    }

    /// <summary>
    /// CPlugBitmapSampler 0x002 chunk
    /// </summary>
    [Chunk(0x0907E002)]
    public class Chunk0907E002 : Chunk<CPlugBitmapSampler>
    {
        public CMwNod? U02;
        public GameBoxRefTable.File? U02File;
        public int U03;
        public float U04;

        public override void ReadWrite(CPlugBitmapSampler n, GameBoxReaderWriter rw)
        {
            rw.Id(ref n.name!);
            rw.NodeRef(ref U02, ref U02File);
            rw.Int32(ref U03); // DoData
            rw.Single(ref U04);
        }
    }

    /// <summary>
    /// CPlugBitmapSampler 0x005 chunk
    /// </summary>
    [Chunk(0x0907E005)]
    public class Chunk0907E005 : Chunk0907E002
    {
        
    }

    /// <summary>
    /// CPlugBitmapSampler 0x006 chunk
    /// </summary>
    [Chunk(0x0907E006)]
    public class Chunk0907E006 : Chunk0907E002
    {
        
    }

    #region 0x008 chunk

    /// <summary>
    /// CPlugBitmapSampler 0x008 chunk
    /// </summary>
    [Chunk(0x0907E008)]
    public class Chunk0907E008 : Chunk0907E002
    {
        public uint U05;

        public override void ReadWrite(CPlugBitmapSampler n, GameBoxReaderWriter rw)
        {
            base.ReadWrite(n, rw);

            // CPlugBitmapSampler::IsBorderColorUsed
            if (((((byte)U03 & 0x18) != 0x18) && (((byte)U03 & 0x60) != 0x60)) && ((U03 & 0x1800) != 0x1800))
            {
                return;
            }

            rw.UInt32(ref U05);
        }
    }

    #endregion
}