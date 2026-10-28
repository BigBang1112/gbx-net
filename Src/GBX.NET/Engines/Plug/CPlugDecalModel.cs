namespace GBX.NET.Engines.Plug;

public partial class CPlugDecalModel
{
    public partial class MacroDecalSet
    {
        private static readonly BoxAligned DefaultBounds = new(0, 0, 0, -1, -1, -1);

        [Obsolete("Use Id instead.")]
        public string? U01
        {
            get => Id;
            set => Id = value;
        }

        [Obsolete("Use Bounds instead.")]
        public BoxAligned U02
        {
            get => Bounds;
            set => Bounds = value;
        }
    }

    public partial class MacroDecal3d
    {
        [Obsolete("Use DecalModelIndex instead.")]
        public int U01
        {
            get => DecalModelIndex;
            set => DecalModelIndex = value;
        }

        [Obsolete("Use SpriteGroupId instead.")]
        public string? U02
        {
            get => SpriteGroupId;
            set => SpriteGroupId = value;
        }

        [Obsolete("Use Scale instead.")]
        public float U03
        {
            get => Scale;
            set => Scale = value;
        }

        [Obsolete("Use Position.X instead.")]
        public float U04
        {
            get => Position.X;
            set => Position = Position with { X = value };
        }

        [Obsolete("Use Position.Y instead.")]
        public float U05
        {
            get => Position.Y;
            set => Position = Position with { Y = value };
        }
    }
}
