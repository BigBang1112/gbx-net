using GBX.NET.Engines.GameData;
using System.Diagnostics;

namespace GBX.NET.Engines.Game
{
    /// <summary>
    /// Block on a map (0x03057000)
    /// </summary>
    /// <remarks>A block placed on a map.</remarks>
    [Node(0x03057000)]
    [DebuggerTypeProxy(typeof(DebugView))]
    public class CGameCtnBlock : Node
    {
        #region Constants

        const int isGroundBit = 12;
        const int isGhostBit = 28;
        const int isFreeBit = 29;

        #endregion

        #region Properties

        /// <summary>
        /// Name of the block.
        /// </summary>
        [NodeMember]
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

        [NodeMember]
        public Meta BlockInfo { get; set; }

        /// <summary>
        /// Facing direction of the block.
        /// </summary>
        [NodeMember]
        public Direction Direction { get; set; }

        /// <summary>
        /// Position of the block on the map in block coordination. This value get's explicitly converted to <see cref="Byte3"/> in the serialized form. Values below 0 or above 255 should be avoided.
        /// </summary>
        [NodeMember]
        public Int3 Coord { get; set; }

        /// <summary>
        /// Flags of the block. If the chunk version is <see cref="null"/>, this value can be presented as <see cref="short"/>.
        /// </summary>
        [NodeMember]
        public int Flags { get; set; }

        /// <summary>
        /// Author of the block, usually of a custom one made in Mesh Modeller.
        /// </summary>
        [NodeMember]
        public string Author { get; }

        /// <summary>
        /// Used skin on the block.
        /// </summary>
        [NodeMember]
        public CGameCtnBlockSkin Skin { get; }

        /// <summary>
        /// Additional block parameters.
        /// </summary>
        [NodeMember]
        public CGameWaypointSpecialProperty Parameters { get; }

        [NodeMember]
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
        /// If this block is a free block. Feature available since TM®. You can't set this property because of the strict ordering these blocks follow. Taken from flags.
        /// </summary>
        [NodeMember]
        public bool IsFree
        {
            get => Flags > -1 && (Flags & (1 << isFreeBit)) != 0;
        }

        /// <summary>
        /// If the block should use the ground variant. Taken from flags.
        /// </summary>
        [NodeMember]
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
        /// Determines the hill ground variant in TM®. Taken from flags.
        /// </summary>
        [NodeMember]
        public bool Bit21
        {
            get => Flags > -1 && (Flags & (1 << 21)) != 0;
            set
            {
                if (value) Flags |= 1 << 21;
                else Flags &= ~(1 << 21);
            }
        }

        /// <summary>
        /// Taken from flags.
        /// </summary>
        [NodeMember]
        public bool Bit17
        {
            get => Flags > -1 && (Flags & (1 << 17)) != 0;
            set
            {
                if (value) Flags |= 1 << 17;
                else Flags &= ~(1 << 17);
            }
        }

        /// <summary>
        /// If the block is considered as clip. Taken from flags.
        /// </summary>
        [NodeMember]
        public bool IsClip => Flags > -1 && ((Flags >> 6) & 63) == 63;

        /// <summary>
        /// Variant of the block. Taken from flags.
        /// </summary>
        [NodeMember]
        public int Variant
        {
            get => Flags > -1 ? Flags & 15 : -1;
            set => Variant = (int)(Flags & 0xFFFFFFF0) + (value & 15);
        }

        #endregion

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

        #region Debug view

        private class DebugView
        {
            private readonly CGameCtnBlock node;

            public string Name => node.Name;
            public Meta BlockInfo => node.BlockInfo;
            public Direction Direction => node.Direction;
            public Int3 Coord => node.Coord;
            public int Flags => node.Flags;
            public string Author => node.Author;
            public CGameCtnBlockSkin Skin => node.Skin;
            public CGameWaypointSpecialProperty Parameters => node.Parameters;
            public bool IsGhost => node.IsGhost;
            public bool IsFree => node.IsFree;
            public bool IsGround => node.IsGround;
            public bool Bit21 => node.Bit21;
            public bool Bit17 => node.Bit17;
            public bool IsClip => node.IsClip;
            public int Variant => node.Variant;

            public DebugView(CGameCtnBlock node) => this.node = node;
        }

        #endregion

        #endregion
    }
}
