namespace GBX.NET.Engines.Plug;

/// <remarks>ID: 0x09026000</remarks>
[Node(0x09026000)]
[Node(0x09063000)]
[NodeExtension("Shader")]
public class CPlugShaderApply : CPlugShaderGeneric
{
    private ExternalNode<CPlugBitmapAddress>[]? bitmapAddresses;

    [NodeMember]
    [AppliedWithChunk<Chunk09026001>]
    [AppliedWithChunk<Chunk09026002>]
    [AppliedWithChunk<Chunk0902600C>]
    public ExternalNode<CPlugBitmapAddress>[]? BitmapAddresses { get => bitmapAddresses; set => bitmapAddresses = value; }

    internal CPlugShaderApply()
    {

    }

    #region 0x026 class

    #region 0x001 chunk

    /// <summary>
    /// CPlugShaderApply 0x001 chunk
    /// </summary>
    [Chunk(0x09026001)]
    public class Chunk09026001 : Chunk<CPlugShaderApply>
    {
        public int U01;
        public int U02;
        public int U04;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.ArrayNode<CPlugBitmapAddress>(ref n.bitmapAddresses);
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
        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            // array of CPlugBitmapAddress
            rw.ArrayNode<CPlugBitmapAddress>(ref n.bitmapAddresses);
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

    #region 0x00A chunk

    /// <summary>
    /// CPlugShaderApply 0x00A chunk
    /// </summary>
    [Chunk(0x0902600A)]
    public class Chunk0902600A : Chunk<CPlugShaderApply>
    {
        public uint U01;
        public uint U02;
        public uint U03;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01); // DoData
            rw.UInt32(ref U02); // DoData
            rw.UInt32(ref U03); // DoData
        }
    }

    #endregion

    #region 0x00C chunk

    /// <summary>
    /// CPlugShaderApply 0x00C chunk
    /// </summary>
    [Chunk(0x0902600C)]
    public class Chunk0902600C : Chunk<CPlugShaderApply>
    {
        private int listVersion;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref listVersion);
            rw.ArrayNode<CPlugBitmapAddress>(ref n.bitmapAddresses); // May not be correct
        }
    }

    #endregion

    #region 0x010 chunk

    /// <summary>
    /// CPlugShaderApply 0x010 chunk
    /// </summary>
    [Chunk(0x09026010)]
    public class Chunk09026010 : Chunk<CPlugShaderApply>
    {
        public byte U01;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.Byte(ref U01); // DoData
        }
    }

    #endregion

    #region 0x011 chunk

    /// <summary>
    /// CPlugShaderApply 0x011 chunk
    /// </summary>
    [Chunk(0x09026011)]
    public class Chunk09026011 : Chunk<CPlugShaderApply>
    {
        private int version;

        public Node? U01;

        public int Version { get => version; set => version = value; }

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref version);
            rw.NodeRef(ref U01);
        }
    }

    #endregion

    #endregion

    #region 0x063 class

    #region 0x002 chunk

    /// <summary>
    /// CPlugShaderApply 0x002 chunk
    /// </summary>
    [Chunk(0x09063002)]
    public class Chunk09063002 : Chunk<CPlugShaderApply>
    {
        public uint U01;
        public int U02;
        public int[]? U03;

        public override void ReadWrite(CPlugShaderApply n, GameBoxReaderWriter rw)
        {
            rw.UInt32(ref U01); // DoData
            rw.Int32(ref U02);
            rw.Array<int>(ref U03, count: 5); // CPlugBitmapAddresses
        }
    }

    #endregion

    #endregion
}
