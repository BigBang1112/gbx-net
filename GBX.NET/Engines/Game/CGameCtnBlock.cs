using GBX.NET.Engines.GameData;
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
        const int isGroundBit = 12;
        const int isGhostBit = 28;
        const int isFreeBit = 29;

        /// <summary>
        /// Name of the block.
        /// </summary>
        public string Name
        {
            get => BlockInfo.ID;
            set
            {
                if (BlockInfo == null)
                    BlockInfo = new Meta(value, "", "");
                else BlockInfo.ID = value;
            }
        }

        public Meta BlockInfo { get; set; }

        /// <summary>
        /// Facing direction of the block.
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Position of the block on the map in block coordination.
        /// </summary>
        public Int3 Coord { get; set; }

        /// <summary>
        /// Flags of the block. If the chunk version is <see cref="null"/>, this value can be presented as <see cref="short"/>.
        /// </summary>
        public int Flags { get; set; }

        /// <summary>
        /// Author of the block, usually of a custom one made in Mesh Modeller.
        /// </summary>
        public string Author { get; }

        /// <summary>
        /// Used skin on the block.
        /// </summary>
        public CGameCtnBlockSkin Skin { get; }

        /// <summary>
        /// Additional block parameters.
        /// </summary>
        public CGameWaypointSpecialProperty Parameters { get; }

        public bool IsGhost
        {
            get => Flags > -1 && (Flags & (1 << isGhostBit)) != 0;
            set
            {
                if (value) Flags |= 1 << isGhostBit;
                else Flags &= ~(1 << isGhostBit);
            }
        }

        /// <summary>
        /// If this block is a free block. Feature available since TM®. You can't set this property because of the strict ordering these blocks follow.
        /// </summary>
        public bool IsFree
        {
            get => Flags > -1 && (Flags & (1 << isFreeBit)) != 0;
        }

        public bool IsGround // ground: bit 12
        {
            get => Flags > -1 && (Flags & (1 << isGroundBit)) != 0;
            set
            {
                if (value) Flags |= 1 << isGroundBit;
                else Flags &= ~(1 << isGroundBit);
            }
        }

        /// <summary>
        /// Determines the hill ground variant in TM 2020
        /// </summary>
        public bool Bit21
        {
            get => Flags > -1 && (Flags & (1 << 21)) != 0;
            set
            {
                if (value) Flags |= 1 << 21;
                else Flags &= ~(1 << 21);
            }
        }

        public bool Bit17
        {
            get => Flags > -1 && (Flags & (1 << 17)) != 0;
            set
            {
                if (value) Flags |= 1 << 17;
                else Flags &= ~(1 << 17);
            }
        }

        public bool IsClip => Flags > -1 && ((Flags >> 6) & 63) == 63;

        public int Variant
        {
            get => Flags > -1 ? Flags & 15 : -1;
            set => Variant = (int)(Flags & 0xFFFFFFF0) + (value & 15);
        }

        public CGameCtnBlock()
        {

        }

        public CGameCtnBlock(string name, Direction direction, Int3 coord, int flags) : this(name, direction, coord, flags, null, null, null)
        {

        }

        public CGameCtnBlock(string name, Direction direction, Int3 coord, int flags, string author, CGameCtnBlockSkin skin, CGameWaypointSpecialProperty parameters)
        {
            Name = name;
            Direction = direction;
            Coord = coord;
            Flags = flags;
            Author = author;
            Skin = skin;
            Parameters = parameters;
        }

        public override string ToString()
        {
            return $"{Name} {Coord}";
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
                n.Direction = (Direction)rw.Byte((byte)n.Direction);
                n.Coord = (Int3)rw.Byte3((Byte3)n.Coord);
                n.Flags = rw.Int32(n.Flags);
            }
        }

        #endregion

        #endregion
    }
}
