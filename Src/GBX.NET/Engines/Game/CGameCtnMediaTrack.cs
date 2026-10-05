namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaTrack
{
    private List<CGameCtnMediaBlock>? blocks;
    public partial List<CGameCtnMediaBlock> Blocks
    {
        get => blocks ??= [];
        set => blocks = value;
    }

    private CGameCtnGhost? LegacyGhostModel => Blocks.OfType<CGameCtnMediaBlockGhost>().FirstOrDefault()?.GhostModel;

    public partial class Chunk03078002
    {
        public override void ReadWrite(CGameCtnMediaTrack n, GbxReaderWriter rw)
        {
            rw.Boolean(ref n.isKeepPlaying);

            if (rw.Reader is not null && n.Blocks.Any(x => x is CGameCtnMediaBlockMusicEffect))
            {
                n.IsKeepPlaying = true;
            }
        }
    }

    public partial class Chunk0307B000
    {
        public override void ReadWrite(CGameCtnMediaTrack n, GbxReaderWriter rw)
        {
            var ghost = n.LegacyGhostModel;
            rw.NodeRef(ref ghost);

            if (rw.Reader is null)
            {
                return;
            }

            var block = new CGameCtnMediaBlockGhost
            {
                GhostModel = ghost,
                Start = TimeSingle.Zero,
                End = new TimeSingle(1)
            };
            block.CreateChunk<CGameCtnMediaBlockGhost.Chunk030E5000>();

            if (ghost is not null)
            {
                n.Name = $"Ghost:{ghost.GhostNickname}";

                if (ghost.RawData is not null || ghost.CompressedData is not null)
                {
                    block.End = new TimeSingle((float)ghost.GetDuration().TotalSeconds);
                }
            }

            n.Blocks.Insert(0, block);
            n.IsReadOnly = true;
        }
    }
}
