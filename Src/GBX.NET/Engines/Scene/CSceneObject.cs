namespace GBX.NET.Engines.Scene;

[Node(0x0A005000)]
public class CSceneObject : CMwNod
{
    protected CSceneObject()
    {

    }

    [Chunk(0x0A005001)]
    public class Chunk0A005001 : Chunk<CSceneObject>
    {
        public int U01;

        public override void ReadWrite(CSceneObject n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    [Chunk(0x0A005003)]
    public class Chunk0A005003 : Chunk<CSceneObject>
    {
        public int U01;

        public override void ReadWrite(CSceneObject n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }
}
