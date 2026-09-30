namespace GBX.NET.Engines.Game;

public partial class CGameCtnBlockInfo
{
    public partial class Chunk0304E02E : IVersionable
    {
        public int Version { get; set; }

        public override void ReadWrite(CGameCtnBlockInfo n, GbxReaderWriter rw)
        {
            rw.VersionInt32(this);
            n.ProdState = rw.EnumInt32<CGameCtnCollector.EProdState>(n.ProdState);
        }
    }

    internal override IHeaderChunk? NewHeaderChunk(uint chunkId)
    {
        if (chunkId == 0x090F4000)
        {
            return new CPlugGameSkin.HeaderChunk090F4000 { Node = new() };
        }

        return base.NewHeaderChunk(chunkId);
    }
}
