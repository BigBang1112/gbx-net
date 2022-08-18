namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09026000</remarks>
[Node(0x09026000)]
public class CPlugShaderApply : CPlugShaderGeneric
{
    protected CPlugShaderApply()
    {

    }

    #region 0x001 chunk

    /// <summary>
    /// CPlugShaderApply 0x001 chunk
    /// </summary>
    [Chunk(0x09026001)]
    public class Chunk09026001 : Chunk<CPlugShaderApply>
    {
        public int U01;
        public int U02;
        public int U03;
        public int U04;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Int32(ref U04); // DoData
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CPlugShaderApply 0x002 chunk
    /// </summary>
    [Chunk(0x09026002)]
    public class Chunk09026002 : Chunk<CPlugShaderApply>
    {
        public int U01;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            // array of CPlugBitmapAddress
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x004 chunk

    /// <summary>
    /// CPlugShaderApply 0x004 chunk
    /// </summary>
    [Chunk(0x09026004)]
    public class Chunk09026004 : Chunk<CPlugShaderApply>
    {
        public int U01;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x006 chunk

    /// <summary>
    /// CPlugShaderApply 0x006 chunk
    /// </summary>
    [Chunk(0x09026006)]
    public class Chunk09026006 : Chunk<CPlugShaderApply>
    {
        public int U01;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x008 chunk

    /// <summary>
    /// CPlugShaderApply 0x008 chunk
    /// </summary>
    [Chunk(0x09026008)]
    public class Chunk09026008 : Chunk<CPlugShaderApply>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    #endregion
}
