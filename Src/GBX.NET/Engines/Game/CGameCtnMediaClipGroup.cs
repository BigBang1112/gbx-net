using System;
using System.Collections.Generic;
using System.Linq;

using GBX.NET.Engines.MwFoundations;

namespace GBX.NET.Engines.Game
{
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
        public List<Tuple<CGameCtnMediaClip, Trigger>> Clips { get; set; }

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

                var clips = r.ReadArray(r1 => r1.ReadNodeRef<CGameCtnMediaClip>()!);

                var triggers = r.ReadArray(r1 => new Trigger()
                {
                    Coords = r1.ReadArray(r2 => r2.ReadInt3())
                });

                n.Clips = clips.Select((clip, index) =>
                    Tuple.Create(clip, triggers[index])
                ).ToList();
            }

            public override void Write(CGameCtnMediaClipGroup n, GameBoxWriter w)
            {
                w.Write(n.ClipsVersion);

                w.Write(n.Clips, (x, w1) => w1.Write(x.Item1));
                w.Write(n.Clips, (x, w1) =>
                {
                    w1.Write(x.Item2.Coords, (y, w2) => w2.Write(y));
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
                    Tuple.Create(clip, triggers[index])
                ).ToList();
            }

            public override void Write(CGameCtnMediaClipGroup n, GameBoxWriter w)
            {
                w.Write(n.ClipsVersion);

                w.Write(n.Clips, (x, w1) => w1.Write(x.Item1));
                w.Write(n.Clips, (x, w1) =>
                {
                    var trigger = x.Item2;

                    w1.Write(trigger.Coords, (y, w2) => w2.Write(y));
                    w1.Write(trigger.U01);
                    w1.Write(trigger.U02);
                    w1.Write(trigger.U03);
                    w1.Write(trigger.U04);
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
                    Tuple.Create(clip, triggers[index])
                ).ToList();
            }

            public override void Write(CGameCtnMediaClipGroup n, GameBoxWriter w)
            {
                w.Write(n.ClipsVersion);

                w.Write(n.Clips, (x, w1) => w1.Write(x.Item1));
                w.Write(n.Clips, (x, w1) =>
                {
                    var trigger = x.Item2;

                    w1.Write(trigger.U01);
                    w1.Write(trigger.U02);
                    w1.Write(trigger.U03);
                    w1.Write(trigger.U04);
                    w1.Write((int)trigger.Condition);
                    w1.Write(trigger.ConditionValue);
                    w1.Write(trigger.Coords, (y, w2) => w2.Write(y));
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Trigger
        {
            public Int3[] Coords { get; set; } = new Int3[0];
            public int U01 { get; set; }
            public int U02 { get; set; }
            public int U03 { get; set; }
            public int U04 { get; set; }
            public ECondition Condition { get; set; }
            public float ConditionValue { get; set; }
        }

        #endregion
    }
}
