namespace GBX.NET.Engines.Scene;

/// <remarks>ID: 0x0A011000</remarks>
[Node(0x0A011000)]
[NodeExtension("Mobil")]
public class CSceneMobil : CSceneObject
{
    private CHmsItem? item;

    public CHmsItem? Item { get => item; set => item = value; }

    protected CSceneMobil()
    {
        
    }

    /// <summary>
    /// CSceneMobil 0x003 chunk
    /// </summary>
    [Chunk(0x0A011003)]
    public class Chunk0A011003 : Chunk<CSceneMobil>
    {
        public int U01;

        public override void ReadWrite(CSceneMobil n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // CSceneObjectLink array
        }
    }

    /// <summary>
    /// CSceneMobil 0x004 chunk
    /// </summary>
    [Chunk(0x0A011004)]
    public class Chunk0A011004 : Chunk<CSceneMobil>
    {
        public CMwNod? U01;

        public override void ReadWrite(CSceneMobil n, GameBoxReaderWriter rw)
        {
            rw.NodeRef(ref U01);
        }
    }

    /// <summary>
    /// CSceneMobil 0x005 chunk
    /// </summary>
    [Chunk(0x0A011005)]
    public class Chunk0A011005 : Chunk<CSceneMobil>
    {
        public override void Read(CSceneMobil n, GameBoxReader r, ILogger? logger)
        {
            n.item = Parse<CHmsItem>(r, 0x06003000, progress: null, logger, ignoreZeroIdChunk: true); // direct node
        }

        public override void Write(CSceneMobil n, GameBoxWriter w, ILogger? logger)
        {
            if (n.item is null)
            {
                w.Write(0);
            }
            else
            {
                n.item.Write(w, logger);
            }
        }
    }

    /// <summary>
    /// CSceneMobil 0x006 chunk
    /// </summary>
    [Chunk(0x0A011006)]
    public class Chunk0A011006 : Chunk<CSceneMobil>
    {
        public int U01;

        public override void ReadWrite(CSceneMobil n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }
}
