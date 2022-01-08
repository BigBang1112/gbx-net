namespace GBX.NET.Engines.Scene;

/// <summary>
/// CSceneMobil (0x0A011000)
/// </summary>
[Node(0x0A011000)]
[NodeExtension("Mobil")]
public class CSceneMobil : CSceneObject
{
    private CHmsItem item;

    public CHmsItem Item
    {
        get => item;
        set => item = value;
    }

    protected CSceneMobil()
    {
        item = null!;
    }

    [Chunk(0x0A011003)]
    public class Chunk0A011003 : Chunk<CSceneMobil>
    {
        public int U01;

        public override void ReadWrite(CSceneMobil n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01); // CSceneObject array?
        }
    }

    [Chunk(0x0A011004)]
    public class Chunk0A011004 : Chunk<CSceneMobil>
    {
        public int U01;

        public override void ReadWrite(CSceneMobil n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x0A011005)]
    public class Chunk0A011005 : Chunk<CSceneMobil>
    {
        public override void Read(CSceneMobil n, GameBoxReader r, ILogger? logger)
        {
            n.item = Parse<CHmsItem>(r, 0x06003000, progress: null, logger)!;
        }
    }

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
