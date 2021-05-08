using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0307A000)]
    public class CGameCtnMediaClipGroup : Node
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
        public List<Tuple<CGameCtnMediaClip, Trigger>> Clips { get; set; } = new List<Tuple<CGameCtnMediaClip, Trigger>>();

        #endregion

        #region Chunks

        #region 0x001 chunk

        [Chunk(0x0307A001)]
        public class Chunk0307A001 : Chunk<CGameCtnMediaClipGroup>
        {
            public int Version { get; set; }

            public override void Read(CGameCtnMediaClipGroup n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                var clips = r.ReadArray(() => r.ReadNodeRef<CGameCtnMediaClip>());

                var triggers = r.ReadArray(() => new Trigger()
                {
                    Coords = r.ReadArray(() => r.ReadInt3())
                });

                n.Clips = clips.Select((clip, index) =>
                    Tuple.Create(clip, triggers[index])
                ).ToList();
            }

            public override void Write(CGameCtnMediaClipGroup n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                w.Write(n.Clips, x => w.Write(x.Item1));
                w.Write(n.Clips, x =>
                {
                    w.Write(x.Item2.Coords, y => w.Write(y));
                });
            }
        }

        #endregion

        #region 0x002 chunk

        [Chunk(0x0307A002)]
        public class Chunk0307A002 : Chunk<CGameCtnMediaClipGroup>
        {
            public int Version { get; set; }

            public override void Read(CGameCtnMediaClipGroup n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                var clips = r.ReadArray(() => r.ReadNodeRef<CGameCtnMediaClip>());
                var triggers = r.ReadArray(() => new Trigger()
                {
                    Coords = r.ReadArray(() => r.ReadInt3()),
                    U01 = r.ReadInt32(),
                    U02 = r.ReadInt32(),
                    U03 = r.ReadInt32(),
                    U04 = r.ReadInt32()
                });

                n.Clips = clips.Select((clip, index) =>
                    Tuple.Create(clip, triggers[index])
                ).ToList();
            }

            public override void Write(CGameCtnMediaClipGroup n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                w.Write(n.Clips, x => w.Write(x.Item1));
                w.Write(n.Clips, x =>
                {
                    var trigger = x.Item2;

                    w.Write(trigger.Coords, y => w.Write(y));
                    w.Write(trigger.U01);
                    w.Write(trigger.U02);
                    w.Write(trigger.U03);
                    w.Write(trigger.U04);
                });
            }
        }

        #endregion

        #region 0x003 chunk

        [Chunk(0x0307A003)]
        public class Chunk0307A003 : Chunk<CGameCtnMediaClipGroup>
        {
            public int Version { get; set; } = 10;

            public override void Read(CGameCtnMediaClipGroup n, GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();

                var clips = r.ReadArray(() => r.ReadNodeRef<CGameCtnMediaClip>());
                var triggers = r.ReadArray(() =>
                {
                    var u01 = r.ReadInt32();
                    var u02 = r.ReadInt32();
                    var u03 = r.ReadInt32();
                    var u04 = r.ReadInt32();
                    var condition = (ECondition)r.ReadInt32();
                    var conditionValue = r.ReadSingle();
                    var coords = r.ReadArray(() => r.ReadInt3());

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

            public override void Write(CGameCtnMediaClipGroup n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);

                w.Write(n.Clips, x => w.Write(x.Item1));
                w.Write(n.Clips, x =>
                {
                    var trigger = x.Item2;

                    w.Write(trigger.U01);
                    w.Write(trigger.U02);
                    w.Write(trigger.U03);
                    w.Write(trigger.U04);
                    w.Write((int)trigger.Condition);
                    w.Write(trigger.ConditionValue);
                    w.Write(trigger.Coords, y => w.Write(y));
                });
            }
        }

        #endregion

        #endregion

        #region Other classes

        public class Trigger
        {
            public Int3[] Coords { get; set; }
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
