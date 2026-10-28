namespace GBX.NET.Engines.Game;

public partial class CGameCtnMacroBlockInfo
{
    public partial class BlockSpawn
    {
        [Obsolete("Use Challenge instead.")]
        public CMwNod? U01 { get => Challenge; set => Challenge = (CGameCtnChallenge?)value; }

        [Obsolete("Use Color and LightmapQuality instead.")]
        public short U02 { get => (short)(Color | LightmapQuality << 8); set { Color = (byte)value; LightmapQuality = (byte)(value >> 8); } }
    }

    public partial class CardEventsSpawn
    {
        [Obsolete("Use CardEvents instead.")]
        public Ident[]? U01 { get => CardEvents; set => CardEvents = value; }

        [Obsolete("Use Coord instead.")]
        public Int3 U02 { get => Coord; set => Coord = value; }
    }

    public partial class BlockSkinSpawn
    {
        [Obsolete("Use Coord instead.")]
        public Int3 U01 { get => Coord; set => Coord = value; }
    }

    public partial class ObjectSpawn
    {
        [Obsolete("Use NeighbourSpawnIndex instead.")]
        public int U01 { get => NeighbourSpawnIndex; set => NeighbourSpawnIndex = value; }

        [Obsolete("Use Flags instead.")]
        public short U03 { get => Flags; set => Flags = value; }

        [Obsolete("Use Color instead.")]
        public byte U05 { get => Color; set => Color = value; }

        [Obsolete("Use AnimPhaseOffset instead.")]
        public byte U06 { get => AnimPhaseOffset; set => AnimPhaseOffset = value; }

        [Obsolete("Use LightmapQuality instead.")]
        public byte U07 { get => LightmapQuality; set => LightmapQuality = value; }
    }
}
