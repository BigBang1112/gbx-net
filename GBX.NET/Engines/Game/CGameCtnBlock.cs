using System;
using System.Collections.Generic;
using System.Text;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Block on a map (0x03057000)
    /// </summary>
    /// <remarks>A block placed on a map.</remarks>
    [Node(0x03057000)]
    public class CGameCtnBlock : Node
    {
        public Meta BlockInfo { get; set; }

        public Direction? Direction { get; set; }

        public Byte3? Coord { get; set; }

        public int? Flags { get; set; }

        public CGameCtnBlock(ILookbackable lookbackable, uint classID) : base(lookbackable, classID)
        {

        }

        public CGameCtnBlock(ILookbackable lookbackable) : base(lookbackable, 0x03057000)
        {

        }

        public override string ToString()
        {
            return $"{BlockInfo?.ID} {Coord}";
        }

        #region Chunks

        #region 0x002 chunk

        /// <summary>
        /// CGameCtnBlock 0x002 chunk
        /// </summary>
        [Chunk(0x03057002)]
        public class Chunk03057002 : Chunk<CGameCtnBlock>
        {
            public override void ReadWrite(CGameCtnBlock n, GameBoxReaderWriter rw)
            {
                n.BlockInfo = rw.Meta(n.BlockInfo);
                n.Direction = (Direction)rw.Byte((byte)n.Direction.GetValueOrDefault());
                n.Coord = rw.Byte3(n.Coord.GetValueOrDefault());
                n.Flags = rw.Int32(n.Flags.GetValueOrDefault());
            }
        }

        #endregion

        #endregion
    }
}
