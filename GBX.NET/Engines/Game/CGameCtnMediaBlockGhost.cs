using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030E5000)]
    public class CGameCtnMediaBlockGhost : CGameCtnMediaBlock
    {
        public float? Start { get; set; }
        public float? End { get; set; }

        public Key[] Keys { get; set; }

        public CGameGhost Ghost { get; set; }

        public float? Offset { get; set; }

        public bool NoDamage { get; set; }

        public bool ForceLight { get; set; }

        public bool ForceHue { get; set; }

        public CGameCtnMediaBlockGhost(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x030E5001)]
        public class Chunk030E5001 : Chunk<CGameCtnMediaBlockGhost>
        {
            public override void ReadWrite(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw)
            {
                n.Start = rw.Single(n.Start.GetValueOrDefault());
                n.End = rw.Single(n.End.GetValueOrDefault(3));
                n.Ghost = rw.NodeRef<CGameCtnGhost>(n.Ghost, true);
                n.Offset = rw.Single(n.Offset.GetValueOrDefault());
            }
        }

        [Chunk(0x030E5002)]
        public class Chunk030E5002 : Chunk<CGameCtnMediaBlockGhost>
        {
            public int Version { get; set; }

            public override void ReadWrite(CGameCtnMediaBlockGhost n, GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                n.Keys = rw.Array(n.Keys, i => new Key()
                {
                    Time = rw.Reader.ReadSingle(),
                    Unknown = rw.Reader.ReadSingle()
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Unknown);
                });

                n.Ghost = rw.NodeRef<CGameCtnGhost>(n.Ghost, true);
                n.Offset = rw.Single(n.Offset.GetValueOrDefault());
                n.NoDamage = rw.Boolean(n.NoDamage);
                n.ForceLight = rw.Boolean(n.ForceLight);
                n.ForceHue = rw.Boolean(n.ForceHue);
            }
        }

        public class Key : MediaBlockKey
        {
            public float Unknown { get; set; }
        }
    }
}
