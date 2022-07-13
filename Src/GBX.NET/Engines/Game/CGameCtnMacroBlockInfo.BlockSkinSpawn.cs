namespace GBX.NET.Engines.Game;

public partial class CGameCtnMacroBlockInfo
{
    public class BlockSkinSpawn : IReadableWritable, IVersionable
    {
        private int ver;
        private CGameCtnBlockSkin? skin;
        private int blockSpawnIndex;

        public Int3? U01;

        public int Version { get => ver; set => ver = value; }
        public CGameCtnBlockSkin? Skin { get => skin; set => skin = value; }
        public int BlockSpawnIndex { get => blockSpawnIndex; set => blockSpawnIndex = value; }

        public void ReadWrite(GameBoxReaderWriter rw, int version = 0)
        {
            rw.Int32(ref ver);
            rw.NodeRef(ref skin);

            if (ver == 0)
            {
                rw.Int3(ref U01); // its position?
            }

            rw.Int32(ref blockSpawnIndex);
        }
    }
}
