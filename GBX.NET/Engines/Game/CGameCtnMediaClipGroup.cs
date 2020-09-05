using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0307A000)]
    public class CGameCtnMediaClipGroup : Node
    {
        public CGameCtnMediaClip[] Clips
        {
            get => GetValue<Chunk002, Chunk003>(
                x => x.Clips,
                x => x.Clips) as CGameCtnMediaClip[];
        }

        public Trigger[] Triggers
        {
            get => GetValue<Chunk002, Chunk003>(x => x.Triggers, x => x.Triggers) as Trigger[];
            set => SetValue<Chunk002, Chunk003>(x => x.Triggers = value, x => x.Triggers = value);
        }

        public CGameCtnMediaClipGroup(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x0307A002)]
        public class Chunk002 : Chunk
        {
            public int Version { get; set; }
            public CGameCtnMediaClip[] Clips { get; set; }
            public Trigger[] Triggers { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                Clips = rw.Array(Clips,
                    i => rw.Reader.ReadNodeRef<CGameCtnMediaClip>(),
                    x => rw.Writer.Write(x));

                Triggers = rw.Array(Triggers, i =>
                {
                    var coords = rw.Reader.ReadArray(i => rw.Reader.ReadInt3());
                    var unknown1 = rw.Reader.ReadInt32();
                    var unknown2 = rw.Reader.ReadInt32();
                    var unknown3 = rw.Reader.ReadInt32();
                    var unknown4 = rw.Reader.ReadInt32();

                    return new Trigger()
                    {
                        Coords = coords,
                        Unknown1 = unknown1,
                        Unknown2 = unknown2,
                        Unknown3 = unknown3,
                        Unknown4 = unknown4
                    };
                },
                x =>
                {
                    rw.Writer.Write(x.Coords, x => rw.Writer.Write(x));
                    rw.Writer.Write(x.Unknown1);
                    rw.Writer.Write(x.Unknown2);
                    rw.Writer.Write(x.Unknown3);
                    rw.Writer.Write(x.Unknown4);
                });
            }
        }

        [Chunk(0x0307A003)]
        public class Chunk003 : Chunk
        {
            public int Version { get; set; } = 10;
            public CGameCtnMediaClip[] Clips { get; set; }
            public Trigger[] Triggers { get; set; }

            public override void Read(GameBoxReader r, GameBoxWriter unknownW)
            {
                Version = r.ReadInt32();
                Clips = r.ReadArray(i => r.ReadNodeRef<CGameCtnMediaClip>());

                Triggers = r.ReadArray(i =>
                {
                    var unknown1 = r.ReadInt32();
                    var unknown2 = r.ReadInt32();
                    var unknown3 = r.ReadInt32();
                    var unknown4 = r.ReadInt32();
                    var unknown5 = r.ReadInt32();
                    var unknown6 = r.ReadInt32();
                    var coords = r.ReadArray(i => r.ReadInt3());

                    return new Trigger()
                    {
                        Coords = coords,
                        Unknown1 = unknown1,
                        Unknown2 = unknown2,
                        Unknown3 = unknown3,
                        Unknown4 = unknown4,
                        Unknown5 = unknown5,
                        Unknown6 = unknown6
                    };
                });
            }

            public override void Write(GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);
                w.Write(Clips, x => w.Write(x));

                w.Write(Triggers, x =>
                {
                    w.Write(x.Unknown1);
                    w.Write(x.Unknown2);
                    w.Write(x.Unknown3);
                    w.Write(x.Unknown4);
                    w.Write(x.Unknown5.GetValueOrDefault());
                    w.Write(x.Unknown6.GetValueOrDefault());
                    w.Write(x.Coords, x2 => w.Write(x2));
                });
            }
        }

        public class Trigger
        {
            public Int3[] Coords { get; set; }
            public int Unknown1 { get; set; }
            public int Unknown2 { get; set; }
            public int Unknown3 { get; set; }
            public int Unknown4 { get; set; }
            public int? Unknown5 { get; set; }
            public int? Unknown6 { get; set; }
        }
    }
}
