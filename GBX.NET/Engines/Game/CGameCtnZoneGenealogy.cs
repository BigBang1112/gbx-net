using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0311D000)]
    public class CGameCtnZoneGenealogy : Node
    {
        public string[] Zones => GetValue<Chunk002>(x => x.Zones) as string[];
        public int BaseHeight => (int)GetValue<Chunk002>(x => x.BaseHeight);
        public string CurrentZone
        {
            get => GetValue<Chunk002>(x => x.CurrentZone) as string;
            set => SetValue<Chunk002>(x => x.CurrentZone = value);
        }

        public CGameCtnZoneGenealogy(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        public override string ToString()
        {
            return string.Join(" ", Zones);
        }

        [Chunk(0x0311D002)]
        public class Chunk002 : Chunk
        {
            public string[] Zones { get; set; }
            public int BaseHeight { get; set; }
            public int Unknown1 { get; set; }
            public string CurrentZone { get; set; }

            public override void ReadWrite(GameBoxReaderWriter rw)
            {
                Zones = rw.Array(Zones, i => rw.Reader.ReadLookbackString(), x => rw.Writer.WriteLookbackString(x));
                BaseHeight = rw.Int32(BaseHeight); // 9
                Unknown1 = rw.Int32(Unknown1);
                CurrentZone = rw.LookbackString(CurrentZone);
            }
        }
    }
}
