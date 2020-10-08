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
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnMediaClipGroup : Node
    {
        #region Properties

        /// <summary>
        /// An array of MediaTracker clips.
        /// </summary>
        public CGameCtnMediaClip[] Clips { get; set; }

        /// <summary>
        /// An array of triggers, indexes the same as <see cref="Clips"/>.
        /// </summary>
        public Trigger[] Triggers { get; set; }

        #endregion

        #region Chunks

        #region 0x002 chunk

        [Chunk(0x0307A002)]
        public class Chunk0307A002 : Chunk<CGameCtnMediaClipGroup>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaClipGroup n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);
                n.Clips = rw.Array(n.Clips,
                    i => rw.Reader.ReadNodeRef<CGameCtnMediaClip>(),
                    x => rw.Writer.Write(x));

                n.Triggers = rw.Array(n.Triggers, i =>
                {
                    var coords = rw.Reader.ReadArray(j => rw.Reader.ReadInt3());
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
                    rw.Writer.Write(x.Coords, y => rw.Writer.Write(y));
                    rw.Writer.Write(x.Unknown1);
                    rw.Writer.Write(x.Unknown2);
                    rw.Writer.Write(x.Unknown3);
                    rw.Writer.Write(x.Unknown4);
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
                n.Clips = r.ReadArray(i => r.ReadNodeRef<CGameCtnMediaClip>());

                n.Triggers = r.ReadArray(i =>
                {
                    var unknown1 = r.ReadInt32();
                    var unknown2 = r.ReadInt32();
                    var unknown3 = r.ReadInt32();
                    var unknown4 = r.ReadInt32();
                    var unknown5 = r.ReadInt32();
                    var unknown6 = r.ReadInt32();
                    var coords = r.ReadArray(j => r.ReadInt3());

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

            public override void Write(CGameCtnMediaClipGroup n, GameBoxWriter w, GameBoxReader unknownR)
            {
                w.Write(Version);
                w.Write(n.Clips, x => w.Write(x));

                w.Write(n.Triggers, x =>
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

        #endregion

        #endregion

        #region Other classes

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

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnMediaClipGroup node;

            public CGameCtnMediaClip[] Clips => node.Clips;
            public Trigger[] Triggers => node.Triggers;

            public DebugView(CGameCtnMediaClipGroup node) => this.node = node;
        }

        #endregion
    }
}
