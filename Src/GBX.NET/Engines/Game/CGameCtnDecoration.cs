namespace GBX.NET.Engines.Game;

public partial class CGameCtnDecoration
{
    /// <summary>
    /// Refers from <see cref="CGameCtnDecorationMood"/>.
    /// </summary>
    public string RemapFolder { get; set; } = "";

    /// <summary>
    /// Refers from <see cref="CGameCtnDecorationMood"/>.
    /// </summary>
    public CPlugGameSkin? Remapping { get; set; }

    public partial class HeaderChunk03038000
    {
        public byte U01;

        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            rw.Byte(ref U01);
            n.RemapFolder = rw.String(n.RemapFolder);

            if (rw.Reader is not null)
            {
                n.Remapping = new CPlugGameSkin();
                var headerChunk = new CPlugGameSkin.HeaderChunk090F4000 { Node = n.Remapping };
                n.Remapping.Chunks.Add(headerChunk);
                headerChunk.ReadWrite(n.Remapping, rw);
            }

            if (rw.Writer is not null)
            {
                var remapping = n.Remapping ?? new();
                (remapping.Chunks
                    .OfType<CPlugGameSkin.HeaderChunk090F4000>()
                    .FirstOrDefault() ?? new CPlugGameSkin.HeaderChunk090F4000())
                    .ReadWrite(remapping, rw);
            }
        }
    }

    public override IHeaderChunk? CreateHeaderChunk(uint chunkId)
    {
        if (chunkId == 0x090F4000)
        {
            var chunk = new CPlugGameSkin.HeaderChunk090F4000 { Node = new() };
            Chunks.Add(chunk);
            return chunk;
        }

        return base.CreateHeaderChunk(chunkId);
    }
}
