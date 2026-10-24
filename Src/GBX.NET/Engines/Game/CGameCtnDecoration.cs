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

    public partial class Chunk03038001
    {
        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            new CGameCtnDecorationAudio.Chunk03039000().ReadWrite(n.DecoAudio ??= new(), rw);
        }
    }

    public partial class Chunk03038003
    {
        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            new CGameCtnDecorationSize.Chunk0303B000().ReadWrite(n.DecoSize ??= new(), rw);
        }
    }

    public partial class Chunk03038004
    {
        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            new CGameCtnDecorationAudio.Chunk03039002().ReadWrite(n.DecoAudio ??= new(), rw);
        }
    }

    public partial class Chunk03038006
    {
        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            new CGameCtnDecorationMood.Chunk0303A000().ReadWrite(n.DecoMood ??= new(), rw);
        }
    }

    public partial class Chunk03038007
    {
        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            new CGameCtnDecorationAudio.Chunk03039001().ReadWrite(n.DecoAudio ??= new(), rw);
        }
    }

    public partial class Chunk03038008
    {
        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            var decoSize = n.DecoSize ??= new();
            decoSize.BaseHeightBase = rw.Int32(decoSize.BaseHeightBase);
        }
    }

    public partial class Chunk03038009
    {
        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            new CGameCtnDecorationSize.Chunk0303B001().ReadWrite(n.DecoSize ??= new(), rw);
        }
    }

    public partial class Chunk0303800C
    {
        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            new CGameCtnDecorationMood.Chunk0303A001().ReadWrite(n.DecoMood ??= new(), rw);
        }
    }

    public partial class Chunk0303800F
    {
        public override void ReadWrite(CGameCtnDecoration n, GbxReaderWriter rw)
        {
            new CGameCtnDecorationMood.Chunk0303A002().ReadWrite(n.DecoMood ??= new(), rw);
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
