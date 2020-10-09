using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    [Node(0x0311D000)]
    public class CGameCtnZoneGenealogy : Node
    {
        #region Properties

        [NodeMember]
        public string[] Zones { get; set; }

        [NodeMember]
        public int BaseHeight { get; set; }

        [NodeMember]
        public string CurrentZone { get; set; }

        #endregion

        #region Methods

        public override string ToString() => string.Join(" ", Zones);

        #endregion

        #region Chunks

        #region 0x002 chunk

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

        #endregion

        #endregion

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnZoneGenealogy node;

            public string[] Zones => node.Zones;
            public int BaseHeight => node.BaseHeight;
            public string CurrentZone => node.CurrentZone;

            public DebugView(CGameCtnZoneGenealogy node) => this.node = node;
        }

        #endregion
    }
}
