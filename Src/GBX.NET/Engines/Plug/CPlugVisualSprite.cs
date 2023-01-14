namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09010000</remarks>
[Node(0x09010000)]
public class CPlugVisualSprite : CPlugVisual3D
{
    private CPlugSpriteParam? spriteParam;

    [NodeMember(ExactlyNamed = true)]
    [AppliedWithChunk<Chunk09010008>]
    public CPlugSpriteParam? SpriteParam { get => spriteParam; set => spriteParam = value; }

    internal CPlugVisualSprite()
    {

    }

    /// <summary>
    /// CPlugVisualSprite 0x005 chunk
    /// </summary>
    [Chunk(0x09010005)]
    public class Chunk09010005 : Chunk<CPlugVisualSprite>
    {
        public int U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;

        public override void ReadWrite(CPlugVisualSprite n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
        }
    }

    /// <summary>
    /// CPlugVisualSprite 0x006 chunk
    /// </summary>
    [Chunk(0x09010006)]
    public class Chunk09010006 : Chunk<CPlugVisualSprite>
    {
        public short U01;
        public short U02;

        public override void ReadWrite(CPlugVisualSprite n, GameBoxReaderWriter rw)
        {
            rw.Int16(ref U01);
            rw.Int16(ref U02);
        }
    }

    #region 0x008 chunk

    /// <summary>
    /// CPlugVisualSprite 0x008 chunk
    /// </summary>
    [Chunk(0x09010008)]
    public class Chunk09010008 : Chunk<CPlugVisualSprite>
    {
        public override void Read(CPlugVisualSprite n, GameBoxReader r)
        {
            n.spriteParam = Parse<CPlugSpriteParam>(r, 0x090AC000, progress: null, ignoreZeroIdChunk: true); // direct node
        }

        public override void Write(CPlugVisualSprite n, GameBoxWriter w)
        {
            if (n.spriteParam is null)
            {
                w.Write(0);
            }
            else
            {
                n.spriteParam.Write(w);
            }
        }
    }

    #endregion

    #region 0x009 chunk

    /// <summary>
    /// CPlugVisualSprite 0x009 chunk
    /// </summary>
    [Chunk(0x09010009)]
    public class Chunk09010009 : Chunk<CPlugVisualSprite>
    {
        public Rect[]? U01;

        public override void ReadWrite(CPlugVisualSprite n, GameBoxReaderWriter rw)
        {
            rw.Array<Rect>(ref U01);
        }
    }

    #endregion
}
