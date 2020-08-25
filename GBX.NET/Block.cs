using GBX.NET.Engines.Game;
using GBX.NET.Engines.GameData;

namespace GBX.NET
{
    /// <summary>
    /// Block on a map.
    /// </summary>
    public class Block
    {
        const int isGroundBit = 12;
        const int isGhostBit = 28;
        const int isFreeBit = 29;

        /// <summary>
        /// Name of the block.
        /// </summary>
        public string Name
        {
            get => Meta.ID;
            set
            {
                if (Meta == null)
                    Meta = new Meta(value, "", "");
                else Meta.ID = value;
            }
        }

        public Meta Meta { get; set; }

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

        public Block()
        {

        }

        public Block(string name, Direction direction, Int3 coord, int flags) : this(name, direction, coord, flags, null, null, null)
        {

        }

        public Block(string name, Direction direction, Int3 coord, int flags, string author, CGameCtnBlockSkin skin, CGameWaypointSpecialProperty parameters)
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
    }
}
