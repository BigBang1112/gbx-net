namespace GBX.NET.Engines.Game;

/// <summary>
/// Decoration of a map (0x03038000)
/// </summary>
[Node(0x03038000)]
public class CGameCtnDecoration : CGameCtnCollector
{
    #region Constructors

    protected CGameCtnDecoration()
    {

    }

    #endregion

    #region Chunks

    #region 0x011 chunk

    [Chunk(0x03038011)]
    public class Chunk03038011 : Chunk<CGameCtnDecoration>
    {
        public int U01;

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x013 chunk

    [Chunk(0x03038013)]
    public class Chunk03038013 : Chunk<CGameCtnDecoration>
    {
        public int U01;

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x014 chunk

    [Chunk(0x03038014)]
    public class Chunk03038014 : Chunk<CGameCtnDecoration>
    {
        public int U01;

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x015 chunk

    [Chunk(0x03038015)]
    public class Chunk03038015 : Chunk<CGameCtnDecoration>
    {
        public int U01;

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x016 chunk

    [Chunk(0x03038016)]
    public class Chunk03038016 : Chunk<CGameCtnDecoration>
    {
        public int U01;

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #region 0x017 chunk

    [Chunk(0x03038017)]
    public class Chunk03038017 : Chunk<CGameCtnDecoration>
    {
        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    #endregion

    #region 0x018 chunk

    [Chunk(0x03038018)]
    public class Chunk03038018 : Chunk<CGameCtnDecoration>
    {
        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    #endregion

    #region 0x019 chunk

    [Chunk(0x03038019)]
    public class Chunk03038019 : Chunk<CGameCtnDecoration>
    {
        public int U01;
        public int U02;
        public int U03;

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
        }
    }

    #endregion

    #region 0x01A chunk

    [Chunk(0x0303801A)]
    public class Chunk0303801A : Chunk<CGameCtnDecoration>
    {
        public int U01;
        public int U02;

        public override void ReadWrite(CGameCtnDecoration n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
            rw.Int32(ref U02);
        }
    }

    #endregion

    #endregion
}
