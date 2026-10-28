namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaTrack
{
    private NodeRefList<CGameCtnMediaBlock>? blocks;
    public partial NodeRefList<CGameCtnMediaBlock> Blocks
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
            var reference = n.blocks is null ? null : Enumerable.Range(0, n.blocks.Count)
                .Select(i => n.blocks.Get(i).CachedNode).OfType<CGameCtnMediaBlockGhost>().FirstOrDefault()?.References.GhostModel;
            rw.NodeRef(ref reference);

            if (rw.Reader is null)
            {
                return;
            }

            var block = new CGameCtnMediaBlockGhost
            {
                Start = TimeSingle.Zero,
                End = new TimeSingle(1)
            };
            if (reference?.Descriptor is { } descriptor) block.References.GhostModel.Descriptor = descriptor;
            else block.GhostModel = reference?.CachedNode;
            var ghost = reference?.CachedNode;
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
