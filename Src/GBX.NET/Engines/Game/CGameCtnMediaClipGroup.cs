namespace GBX.NET.Engines.Game;

[Node(0x0307A000)]
public sealed class CGameCtnMediaClipGroup : CMwNod
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
    /// An array of MediaTracker clips.
    /// </summary>
    [NodeMember]
    public IList<ClipTrigger> Clips { get; set; }

    public int ClipsVersion { get; set; } = 10;

    #endregion

    #region Constructors

    private CGameCtnMediaClipGroup()
    {
        Clips = null!;
    }

    #endregion

    #region Chunks

    #region 0x001 chunk

    [Chunk(0x0307A001)]
    public class Chunk0307A001 : Chunk<CGameCtnMediaClipGroup>
    {
        public override void Read(CGameCtnMediaClipGroup n, GameBoxReader r)
        {
            n.ClipsVersion = r.ReadInt32();

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
            w.Write(n.ClipsVersion);

            w.Write(n.Clips, (x, w) => w.Write(x.Clip));
            w.Write(n.Clips, (x, w) =>
            {
                w.Write(x.Trigger.Coords, (y, w) => w.Write(y));
            });
        }
    }

    #endregion

    #region 0x002 chunk

    [Chunk(0x0307A002)]
    public class Chunk0307A002 : Chunk<CGameCtnMediaClipGroup>
    {
        public override void Read(CGameCtnMediaClipGroup n, GameBoxReader r)
        {
            n.ClipsVersion = r.ReadInt32();

            var clips = r.ReadArray(r1 => r1.ReadNodeRef<CGameCtnMediaClip>()!);
            var triggers = r.ReadArray(r1 => new Trigger()
            {
                Coords = r1.ReadArray(r2 => r2.ReadInt3()),
                U01 = r1.ReadInt32(),
                U02 = r1.ReadInt32(),
                U03 = r1.ReadInt32(),
                U04 = r1.ReadInt32()
            });

            n.Clips = clips.Select((clip, index) =>
                new ClipTrigger(clip, triggers[index])
            ).ToList();
        }

        public override void Write(CGameCtnMediaClipGroup n, GameBoxWriter w)
        {
            w.Write(n.ClipsVersion);

            w.Write(n.Clips, (x, w1) => w1.Write(x.Clip));
            w.Write(n.Clips, (x, w1) =>
            {
                w1.Write(x.Trigger.Coords, (y, w2) => w2.Write(y));
                w1.Write(x.Trigger.U01);
                w1.Write(x.Trigger.U02);
                w1.Write(x.Trigger.U03);
                w1.Write(x.Trigger.U04);
            });
        }
    }

    #endregion

    #region 0x003 chunk

    [Chunk(0x0307A003)]
    public class Chunk0307A003 : Chunk<CGameCtnMediaClipGroup>
    {
        public override void Read(CGameCtnMediaClipGroup n, GameBoxReader r)
        {
            n.ClipsVersion = r.ReadInt32();

            var clips = r.ReadArray(r1 => r1.ReadNodeRef<CGameCtnMediaClip>()!);
            var triggers = r.ReadArray(r1 =>
            {
                var u01 = r1.ReadInt32();
                var u02 = r1.ReadInt32();
                var u03 = r1.ReadInt32();
                var u04 = r1.ReadInt32();
                var condition = (ECondition)r1.ReadInt32();
                var conditionValue = r1.ReadSingle();
                var coords = r1.ReadArray(r2 => r2.ReadInt3());

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

            n.Clips = clips.Select((clip, index) =>
                new ClipTrigger(clip, triggers[index])
            ).ToList();
        }

        public override void Write(CGameCtnMediaClipGroup n, GameBoxWriter w)
        {
            w.Write(n.ClipsVersion);

            w.Write(n.Clips, (x, w1) => w1.Write(x.Clip));
            w.Write(n.Clips, (x, w1) =>
            {
                w1.Write(x.Trigger.U01);
                w1.Write(x.Trigger.U02);
                w1.Write(x.Trigger.U03);
                w1.Write(x.Trigger.U04);
                w1.Write((int)x.Trigger.Condition);
                w1.Write(x.Trigger.ConditionValue);
                w1.Write(x.Trigger.Coords, (y, w2) => w2.Write(y));
            });
        }
    }

    #endregion

    #endregion

    #region Other classes

    public class Trigger
    {
        public Int3[] Coords { get; set; } = Array.Empty<Int3>();
        public int U01 { get; set; }
        public int U02 { get; set; }
        public int U03 { get; set; }
        public int U04 { get; set; }
        public ECondition Condition { get; set; }
        public float ConditionValue { get; set; }
    }

    public class ClipTrigger
    {
        public CGameCtnMediaClip Clip { get; set; }
        public Trigger Trigger { get; set; }

        public ClipTrigger(CGameCtnMediaClip clip, Trigger trigger)
        {
            Clip = clip;
            Trigger = trigger;
        }
    }

    #endregion
}
