namespace GBX.NET.Engines.Game;

/// <summary>
/// MediaTracker clip group.
/// </summary>
/// <remarks>ID: 0x0307A000</remarks>
[Node(0x0307A000)]
[NodeExtension("GameCtnMediaClipGroup")]
public partial class CGameCtnMediaClipGroup : CMwNod
{
    #region Enums

    public enum ECondition : int
    {
        None,
        RaceTimeLessThan,
        RaceTimeGreaterThan,
        AlreadyTriggered,
        SpeedLessThan,
        SpeedGreaterThan,
        NotAlreadyTriggered,
        MaxPlayCount,
        RandomOnce,
        Random
    }

    #endregion

    #region Properties

    /// <summary>
    /// List of MediaTracker clips.
    /// </summary>
    [NodeMember]
    [AppliedWithChunk(typeof(Chunk0307A001))]
    [AppliedWithChunk(typeof(Chunk0307A002))]
    [AppliedWithChunk(typeof(Chunk0307A003))]
    public IList<ClipTrigger> Clips { get; set; }

    #endregion

    #region Constructors

    protected CGameCtnMediaClipGroup()
    {
        Clips = Array.Empty<ClipTrigger>();
    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    /// <summary>
    /// CGameCtnMediaClipGroup 0x001 chunk
    /// </summary>
    [Chunk(0x0307A001)]
    public class Chunk0307A001 : Chunk<CGameCtnMediaClipGroup>
    {
        public int ClipsVersion { get; set; } = 10;

        public override void Read(CGameCtnMediaClipGroup n, GameBoxReader r)
        {
            ClipsVersion = r.ReadInt32();

            var clips = r.ReadArray(r => r.ReadNodeRef<CGameCtnMediaClip>()!);

            var triggers = r.ReadArray(r => new Trigger()
            {
                Coords = r.ReadArray(r => r.ReadInt3())
            });

            n.Clips = clips.Select((clip, index) =>
                new ClipTrigger(clip, triggers[index])
            ).ToList();
        }

        public override void Write(CGameCtnMediaClipGroup n, GameBoxWriter w)
        {
            w.Write(ClipsVersion);

            w.WriteList(n.Clips, (x, w) => w.Write(x.Clip));
            w.WriteList(n.Clips, (x, w) =>
            {
                w.WriteArray(x.Trigger.Coords, (y, w) => w.Write(y));
            });
        }

        public override async Task ReadAsync(CGameCtnMediaClipGroup n, GameBoxReader r, CancellationToken cancellationToken = default)
        {
            ClipsVersion = r.ReadInt32();

            var clips = await r.ReadArrayAsync(r => r.ReadNodeRefAsync<CGameCtnMediaClip>());

            var triggers = r.ReadArray(r => new Trigger()
            {
                Coords = r.ReadArray(r => r.ReadInt3())
            });

            n.Clips = clips.Select((clip, index) =>
                new ClipTrigger(clip!, triggers[index])
            ).ToList();
        }
    }

    #endregion

    #region 0x002 chunk

    /// <summary>
    /// CGameCtnMediaClipGroup 0x002 chunk
    /// </summary>
    [Chunk(0x0307A002)]
    public class Chunk0307A002 : Chunk<CGameCtnMediaClipGroup>
    {
        private int listVersion = 10;

        public override void Read(CGameCtnMediaClipGroup n, GameBoxReader r)
        {
            listVersion = r.ReadInt32();

            var clips = r.ReadArray(r => r.ReadNodeRef<CGameCtnMediaClip>()!);
            var triggers = ReadTriggers(r);

            n.Clips = clips.Select((clip, index) =>
                new ClipTrigger(clip, triggers[index])
            ).ToList();
        }

        private static Trigger[] ReadTriggers(GameBoxReader r)
        {
            return r.ReadArray(r => new Trigger()
            {
                Coords = r.ReadArray(r => r.ReadInt3()),
                U01 = r.ReadInt32(),
                U02 = r.ReadInt32(),
                U03 = r.ReadInt32(),
                U04 = r.ReadInt32()
            });
        }

        public override void Write(CGameCtnMediaClipGroup n, GameBoxWriter w)
        {
            w.Write(listVersion);

            w.WriteList(n.Clips, (x, w) => w.Write(x.Clip));
            w.WriteList(n.Clips, (x, w) =>
            {
                w.WriteArray(x.Trigger.Coords, (y, w) => w.Write(y));
                w.Write(x.Trigger.U01);
                w.Write(x.Trigger.U02);
                w.Write(x.Trigger.U03);
                w.Write(x.Trigger.U04);
            });
        }

        public override async Task ReadAsync(CGameCtnMediaClipGroup n, GameBoxReader r, CancellationToken cancellationToken = default)
        {
            listVersion = r.ReadInt32();

            var clips = await r.ReadArrayAsync(r => r.ReadNodeRefAsync<CGameCtnMediaClip>());
            var triggers = ReadTriggers(r);

            n.Clips = clips.Select((clip, index) =>
                new ClipTrigger(clip!, triggers[index])
            ).ToList();
        }
    }

    #endregion

    #region 0x003 chunk

    /// <summary>
    /// CGameCtnMediaClipGroup 0x003 chunk
    /// </summary>
    [Chunk(0x0307A003)]
    public class Chunk0307A003 : Chunk<CGameCtnMediaClipGroup>
    {
        private int listVersion = 10;

        public override void Read(CGameCtnMediaClipGroup n, GameBoxReader r)
        {
            listVersion = r.ReadInt32();

            var clips = r.ReadArray(r => r.ReadNodeRef<CGameCtnMediaClip>()!);
            var triggers = ReadTriggers(r);

            n.Clips = clips.Select((clip, index) =>
                new ClipTrigger(clip, triggers[index])
            ).ToList();
        }

        private static Trigger[] ReadTriggers(GameBoxReader r)
        {
            return r.ReadArray(r =>
            {
                var u01 = r.ReadInt32();
                var u02 = r.ReadInt32();
                var u03 = r.ReadInt32();
                var u04 = r.ReadInt32();
                var condition = (ECondition)r.ReadInt32();
                var conditionValue = r.ReadSingle();
                var coords = r.ReadArray(r => r.ReadInt3());

                return new Trigger()
                {
                    Coords = coords,
                    U01 = u01,
                    U02 = u02,
                    U03 = u03,
                    U04 = u04,
                    Condition = condition,
                    ConditionValue = conditionValue
                };
            });
        }

        public override void Write(CGameCtnMediaClipGroup n, GameBoxWriter w)
        {
            w.Write(listVersion);

            w.WriteList(n.Clips, (x, w) => w.Write(x.Clip));
            w.WriteList(n.Clips, (x, w) =>
            {
                w.Write(x.Trigger.U01);
                w.Write(x.Trigger.U02);
                w.Write(x.Trigger.U03);
                w.Write(x.Trigger.U04);
                w.Write((int)x.Trigger.Condition);
                w.Write(x.Trigger.ConditionValue);
                w.WriteArray(x.Trigger.Coords, (y, w) => w.Write(y));
            });
        }

        public override async Task ReadAsync(CGameCtnMediaClipGroup n, GameBoxReader r, CancellationToken cancellationToken = default)
        {
            listVersion = r.ReadInt32();

            var clips = await r.ReadArrayAsync(r => r.ReadNodeRefAsync<CGameCtnMediaClip>());
            var triggers = ReadTriggers(r);

            n.Clips = clips.Select((clip, index) =>
                new ClipTrigger(clip!, triggers[index])
            ).ToList();
        }
    }

    #endregion

    #region 0x004 skippable chunk

    /// <summary>
    /// CGameCtnMediaClipGroup 0x004 skippable chunk
    /// </summary>
    [Chunk(0x0307A004)]
    public class Chunk0307A004 : SkippableChunk<CGameCtnMediaClipGroup>
    {
        public int U01;

        public override void ReadWrite(CGameCtnMediaClipGroup n, GameBoxReaderWriter rw)
        {
            rw.Int32(ref U01);
        }
    }

    #endregion

    #endregion
}
