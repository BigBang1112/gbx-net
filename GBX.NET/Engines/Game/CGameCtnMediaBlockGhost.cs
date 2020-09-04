using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x030E5000)]
    public class CGameCtnMediaBlockGhost : CGameCtnMediaBlock
    {
        public Key[] Keys
        {
            get => GetValue<Chunk002>(x => x.Keys) as Key[];
            set => SetValue<Chunk002>(x => x.Keys = value);
        }

        public CGameGhost Ghost
        {
            get => GetValue<Chunk001, Chunk002>(x => x.Ghost, x => x.Ghost) as CGameGhost;
        }

        public float? Offset
        {
            get => GetValue<Chunk001, Chunk002>(x => x.Offset, x => x.Offset) as float?;
            set => SetValue<Chunk001, Chunk002>(x => x.Offset = value.GetValueOrDefault(), x => x.Offset = value.GetValueOrDefault());
        }

        public bool NoDamage
        {
            get => (bool)GetValue<Chunk002>(x => x.NoDamage);
            set => SetValue<Chunk002>(x => x.NoDamage = value);
        }

        public bool ForceLight
        {
            get => (bool)GetValue<Chunk002>(x => x.ForceLight);
            set => SetValue<Chunk002>(x => x.ForceLight = value);
        }

        public bool ForceHue
        {
            get => (bool)GetValue<Chunk002>(x => x.ForceHue);
            set => SetValue<Chunk002>(x => x.ForceHue = value);
        }

        public CGameCtnMediaBlockGhost(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        [Chunk(0x030E5001)]
        public class Chunk001 : Chunk
        {
            public float Start { get; set; }
            public float End { get; set; }
            public CGameCtnGhost Ghost { get; set; }
            public float Offset { get; set; }

            public Chunk001(CGameCtnMediaBlockGhost node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Start = rw.Single(Start);
                End = rw.Single(End);
                Ghost = rw.NodeRef<CGameCtnGhost>(Ghost, true);
                Offset = rw.Single(Offset);
            }
        }

        [Chunk(0x030E5000, 0x002)]
        public class Chunk002 : Chunk
        {
            public int Version { get; set; }
            public Key[] Keys { get; set; }
            public CGameCtnGhost Ghost { get; set; }
            public float Offset { get; set; }
            public bool NoDamage { get; set; }
            public bool ForceLight { get; set; }
            public bool ForceHue { get; set; }

            public Chunk002(CGameCtnMediaBlockGhost node) : base(node)
            {
                
            }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Version = rw.Int32(Version);

                Keys = rw.Array(Keys, i =>
                {
                    var time = rw.Reader.ReadSingle();
                    var unknown = rw.Reader.ReadSingle();

                    return new Key() { Time = time, Unknown = unknown };
                },
                x =>
                {
                    rw.Writer.Write(x.Time);
                    rw.Writer.Write(x.Unknown);
                });

                Ghost = rw.NodeRef<CGameCtnGhost>(Ghost, true);
                Offset = rw.Single(Offset);
                NoDamage = rw.Boolean(NoDamage);
                ForceLight = rw.Boolean(ForceLight);
                ForceHue = rw.Boolean(ForceHue);
            }
        }

        public class Key : MediaBlockKey
        {
            public float Unknown { get; set; }
        }
    }
}
