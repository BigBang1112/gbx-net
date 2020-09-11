using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0311D000)]
    public class CGameCtnZoneGenealogy : Node
    {
        public string[] Zones { get; set; }
        public int BaseHeight { get; set; }
        public string CurrentZone { get; set; }

        public CGameCtnZoneGenealogy(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        public override string ToString()
        {
            return string.Join(" ", Zones);
        }

        [Chunk(0x0311D002)]
        public class Chunk0311D002 : Chunk<CGameCtnZoneGenealogy>
        {
            public int Unknown1 { get; set; }

            public override void ReadWrite(CGameCtnZoneGenealogy n, GameBoxReaderWriter rw)
            {
                n.Zones = rw.Array(n.Zones, i => rw.Reader.ReadLookbackString(), x => rw.Writer.WriteLookbackString(x));
                n.BaseHeight = rw.Int32(n.BaseHeight); // 9
                Unknown1 = rw.Int32(Unknown1);
                n.CurrentZone = rw.LookbackString(n.CurrentZone);
            }
        }
    }
}
