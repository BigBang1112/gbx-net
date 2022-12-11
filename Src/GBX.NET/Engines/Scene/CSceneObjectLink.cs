namespace GBX.NET.Engines.Scene;

[Node(0x0A00F000)]
[Node(0x0A014000)] // Superb
public class CSceneObjectLink : CMwNod
{
    internal CSceneObjectLink()
    {
        
    }

    #region 0x001 chunk

    /// <summary>
    /// CSceneObjectLink 0x001 chunk
    /// </summary>
    [Chunk(0x0A014001)]
    public class Chunk0A014001 : Chunk<CSceneObjectLink>
    {
        public bool U01;
        public int U02;
        public string[]? U03;
        public int U05;
        public Iso4 U06;
        public bool U18;
        public Node? U19;

        public override void ReadWrite(CSceneObjectLink n, GameBoxReaderWriter rw)
        {
            // All guessed, barely anything matches the code

            rw.Boolean(ref U01);

            if (U01)
            {
                rw.Int32(ref U02);
                rw.ArrayId(ref U03);
                rw.Int32(ref U05);
            }
            else
            {
                rw.NodeRef(ref U19);
            }

            rw.Iso4(ref U06);

            rw.Boolean(ref U18);
        }
    }
    
    /// <summary>
    /// CSceneObjectLink 0x001 chunk
    /// </summary>
    [Chunk(0x0A00F001)]
    public class Chunk0A00F001 : Chunk<CSceneObjectLink>
    {
        public float U01;
        public float U02;
        public float U03;
        public float U04;
        public float U05;
        public float U06;
        public bool U07;

        public override void ReadWrite(CSceneObjectLink n, GameBoxReaderWriter rw)
        {
            rw.Single(ref U01);
            rw.Single(ref U02);
            rw.Single(ref U03);
            rw.Single(ref U04);
            rw.Single(ref U05);
            rw.Single(ref U06);
            rw.Boolean(ref U07);
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

    /// <summary>
    /// CSceneObjectLink 0x002 chunk
    /// </summary>
    [Chunk(0x0A00F002)]
    public class Chunk0A00F002 : Chunk<CSceneObjectLink>
    {
        public bool U01;

        public override void ReadWrite(CSceneObjectLink n, GameBoxReaderWriter rw)
        {
            rw.Boolean(ref U01);
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