namespace GBX.NET.Engines.Scene;

[Node(0x0A014000)]
public class CSceneObjectLink : CMwNod
{
    protected CSceneObjectLink()
    {
        
    }

    #region 0x001 chunk

    /// <summary>
    /// CSceneObjectLink 0x001 chunk
    /// </summary>
    [Chunk(0x0A014001)]
    public class Chunk0A014001 : Chunk<CSceneObjectLink>
    {
        public int U01;
        public int U02;
        public int U03;
        public string? U04;
        public int U05;
        public int U06;
        public int U07;
        public int U08;
        public int U09;
        public int U10;
        public int U11;
        public int U12;
        public int U13;
        public int U14;
        public int U15;
        public int U16;
        public int U17;
        public int U18;

        public override void ReadWrite(CSceneObjectLink n, GameBoxReaderWriter rw)
        {
            // All guessed, barely anything matches the code

            rw.Int32(ref U01);
            rw.Int32(ref U02);
            rw.Int32(ref U03);
            rw.Id(ref U04);
            rw.Int32(ref U05);
            rw.Int32(ref U06);
            rw.Int32(ref U07);
            rw.Int32(ref U08);
            rw.Int32(ref U09);
            rw.Int32(ref U10);
            rw.Int32(ref U11);
            rw.Int32(ref U12);
            rw.Int32(ref U13);
            rw.Int32(ref U14);
            rw.Int32(ref U15);
            rw.Int32(ref U16);
            rw.Int32(ref U17);
            rw.Int32(ref U18);
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CSceneObjectLink 0x002 chunk
    /// </summary>
    [Chunk(0x0A014002)]
    public class Chunk0A014002 : Chunk<CSceneObjectLink>
    {
        public string? U01;
        public bool U02;
        public bool U03;

        public override void ReadWrite(CSceneObjectLink n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
            rw.Boolean(ref U02);
            rw.Boolean(ref U03);
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CSceneObjectLink 0x003 chunk
    /// </summary>
    [Chunk(0x0A014003)]
    public class Chunk0A014003 : Chunk<CSceneObjectLink>
    {
        public string? U01;

        public override void ReadWrite(CSceneObjectLink n, GameBoxReaderWriter rw)
        {
            rw.Id(ref U01);
        }
    }

    #endregion
}