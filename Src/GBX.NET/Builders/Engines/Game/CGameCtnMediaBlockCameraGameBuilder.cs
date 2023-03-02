namespace GBX.NET.Builders.Engines.Game;

public class CGameCtnMediaBlockCameraGameBuilder : Builder
{
    public TimeSingle Start { get; set; }
    public TimeSingle End { get; set; } = TimeSingle.FromSeconds(3);

    public CGameCtnMediaBlockCameraGameBuilder StartingAt(TimeSingle start)
    {
        Start = start;
        return this;
    }

    public CGameCtnMediaBlockCameraGameBuilder EndingAt(TimeSingle end)
    {
        End = end;
        return this;
    }

    public CGameCtnMediaBlockCameraGameBuilder WithTimeRange(TimeSingle start, TimeSingle end)
    {
        Start = start;
        End = end;
        return this;
    }

    public TMUF ForTMUF() => new(this, NewNode());

    internal CGameCtnMediaBlockCameraGame NewNode() => new()
    {
        Start = Start,
        End = End
    };

    public class TMUF : GameBuilder<CGameCtnMediaBlockCameraGameBuilder, CGameCtnMediaBlockCameraGame>
    {
        public int ClipEntId { get; set; }
        public CGameCtnMediaBlockCameraGame.EGameCam GameCam { get; set; }

        public TMUF(CGameCtnMediaBlockCameraGameBuilder baseBuilder, CGameCtnMediaBlockCameraGame node) : base(baseBuilder, node)
        {

        }

        public TMUF WithClipEntId(int id)
        {
            ClipEntId = id;
            return this;
        }

        public TMUF WithGameCam(CGameCtnMediaBlockCameraGame.EGameCam cam)
        {
            GameCam = cam;
            return this;
        }

        public override CGameCtnMediaBlockCameraGame Build()
        {
            Node.CreateChunk<CGameCtnMediaBlockCameraGame.Chunk03084001>();

            Node.ClipEntId = ClipEntId;
            Node.GameCam1 = GameCam;

            return Node;
        }
    }
}
