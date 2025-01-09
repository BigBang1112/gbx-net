namespace GBX.NET.Engines.Scene;

public partial class CSceneTrafficGraph
{
    public partial class Chunk0A062004
    {
        public override void ReadWrite(CSceneTrafficGraph n, GbxReaderWriter rw)
        {
            var count1 = rw.Int32();
            var count2 = rw.Int32();

            if (count1 > 0)
            {
                throw new Exception("CSceneTrafficGraph.Chunk0A062004: count1 > 0");
            }

            if (count2 > 0)
            {
                throw new Exception("CSceneTrafficGraph.Chunk0A062004: count2 > 0");
            }
        }
    }

    public partial class Chunk0A062005
    {
        public override void ReadWrite(CSceneTrafficGraph n, GbxReaderWriter rw)
        {
            // for loop on count1
        }
    }
}
