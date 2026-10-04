namespace GBX.NET.Engines.Game;

public partial class CGameCtnMediaBlockEntity
{
    public Vec3 LightTrailColor { get; set; } = new(1, 0, 0);

    public partial class Key
    {
        public Vec3? TrailColor { get; set; } = new Vec3(1, 0, 0);

        [Obsolete("Use TrailColor.X instead.")]
        public float? U01
        {
            get => TrailColor?.X;
            set => TrailColor = value.HasValue ? (TrailColor ?? new Vec3(1, 0, 0)) with { X = value.Value } : null;
        }

        [Obsolete("Use TrailColor.Y instead.")]
        public int? U02
        {
            get => TrailColor is Vec3 color ? BitConverter.ToInt32(BitConverter.GetBytes(color.Y), 0) : null;
            set => TrailColor = value.HasValue ? (TrailColor ?? new Vec3(1, 0, 0)) with { Y = BitConverter.ToSingle(BitConverter.GetBytes(value.Value), 0) } : null;
        }

        [Obsolete("Use TrailColor.Z instead.")]
        public int? U03
        {
            get => TrailColor is Vec3 color ? BitConverter.ToInt32(BitConverter.GetBytes(color.Z), 0) : null;
            set => TrailColor = value.HasValue ? (TrailColor ?? new Vec3(1, 0, 0)) with { Z = BitConverter.ToSingle(BitConverter.GetBytes(value.Value), 0) } : null;
        }
    }

    public partial class SBadge
    {
        public Vec3 Color { get; set; } = new(1, 1, 1);
    }

    public partial class SSticker
    {
        [Obsolete("Use Name instead.")]
        public string? U01 { get => Name; set => Name = value; }

        [Obsolete("Use Slot instead.")]
        public string? U02 { get => Slot; set => Slot = value; }
    }
}
