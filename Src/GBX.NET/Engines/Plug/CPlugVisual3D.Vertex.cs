namespace GBX.NET.Engines.Plug;

#pragma warning disable CS0618
public partial class CPlugVisual3D
{
    public readonly record struct Vertex
    {
        public Vec3 Position { get; init; }
        /// <summary>For sprites, the three components contain size and rotation data.</summary>
        public Vec3? Normal { get; init; }
        public Vec4? Color { get; init; }

        private int? PackedNormal { get; init; }

        public Vertex(Vec3 Position, Vec3? Normal = null, Vec4? Color = null)
        {
            this.Position = Position;
            this.Normal = Normal;
            this.Color = Color;
        }

        [Obsolete("Use the constructor with Position, Normal and Color instead.")]
        public Vertex(Vec3 Position, Vec3? Normal, Vec3? U02, float? U03, Vec4? Color, float? U08, int? U09)
            : this(Position, Normal, Color ?? (U02.HasValue || U03.HasValue
                ? new Vec4(U02.GetValueOrDefault().X, U02.GetValueOrDefault().Y, U02.GetValueOrDefault().Z, U03.GetValueOrDefault())
                : null))
        {
            this.U08 = U08;
            this.U09 = U09;
        }

        [Obsolete("Use Color.X, Color.Y and Color.Z instead.")]
        public Vec3? U02
        {
            get => Color.HasValue ? new Vec3(Color.Value.X, Color.Value.Y, Color.Value.Z) : null;
            init => Color = value.HasValue
                ? new Vec4(value.Value.X, value.Value.Y, value.Value.Z, Color.GetValueOrDefault().W)
                : Color.HasValue ? new Vec4(0, 0, 0, Color.Value.W) : null;
        }

        [Obsolete("Use Color.W instead.")]
        public float? U03
        {
            get => Color?.W;
            init => Color = value.HasValue
                ? new Vec4(Color.GetValueOrDefault().X, Color.GetValueOrDefault().Y, Color.GetValueOrDefault().Z, value.Value)
                : Color.HasValue ? new Vec4(Color.Value.X, Color.Value.Y, Color.Value.Z, 0) : null;
        }

        [Obsolete("Native sprite vertices have no additional float after Color. Use Normal for sprite data.")]
        public float? U08 { get; init; }

        [Obsolete("Native sprite vertices have no additional integer after Color. Use Normal for sprite data.")]
        public int? U09 { get; init; }

        [Obsolete("Use Position, Normal and Color instead.")]
        public void Deconstruct(out Vec3 Position, out Vec3? Normal, out Vec3? U02, out float? U03,
            out Vec4? Color, out float? U08, out int? U09)
        {
            Position = this.Position;
            Normal = this.Normal;
            U02 = this.U02;
            U03 = this.U03;
            Color = this.Color;
            U08 = this.U08;
            U09 = this.U09;
        }

        public static Vertex Read(GbxReader r, bool u01, bool u02, bool u03, bool u04, bool isSprite)
        {
            using var rw = new GbxReaderWriter(r);
            return default(Vertex).ReadWrite(rw, u01, u02, u03 && !isSprite, u04);
        }

        public void Write(GbxWriter w, bool u01, bool u02, bool u03, bool u04, bool isSprite)
        {
            using var rw = new GbxReaderWriter(w);
            _ = ReadWrite(rw, u01, u02, u03 && !isSprite, u04);
        }

        internal Vertex ReadWrite(GbxReaderWriter rw, bool hasNormals, bool hasColors, bool compressNormals, bool compressColors)
        {
            var position = rw.Vec3(Position);
            var normal = new Vec3(0, 1, 0);
            var color = new Vec4(1, 1, 1, 1);
            var packedNormal = default(int?);
            if (hasNormals)
            {
                if (compressNormals)
                {
                    var value = Normal.GetValueOrDefault(normal);
                    // Keep an unchanged packed value exact, including its two unused bits.
                    packedNormal = rw.Int32(PackedNormal.HasValue && DecodeNormal(PackedNormal.Value) == value
                        ? PackedNormal.Value : EncodeNormal(value));
                    normal = DecodeNormal(packedNormal.Value);
                }
                else normal = rw.Vec3(Normal.GetValueOrDefault(normal));
            }
            if (hasColors)
            {
                if (compressColors)
                {
                    var value = Color.GetValueOrDefault(color);
                    var packedColor = rw.Int32((RoundColor(value.W) << 24) | (RoundColor(value.X) << 16)
                        | (RoundColor(value.Y) << 8) | RoundColor(value.Z));
                    const float scale = 1f / 255;
                    color = new Vec4(((packedColor >> 16) & 255) * scale, ((packedColor >> 8) & 255) * scale,
                        (packedColor & 255) * scale, ((packedColor >> 24) & 255) * scale);
                }
                else color = rw.Vec4(Color.GetValueOrDefault(color));
            }
            return rw.Reader is null ? this
                : this with { Position = position, Normal = normal, Color = color, PackedNormal = packedNormal };
        }

        private static int RoundColor(float value) => (value >= 0
            ? (int)(value * 255f + 0.5f) : -(int)(-value * 255f + 0.5f)) & 255;

        private static int EncodeNormal(Vec3 value) => ((int)(value.X * 511f) & 1023)
            | (((int)(value.Y * 511f) & 1023) << 10) | (((int)(value.Z * 511f) & 1023) << 20);

        private static Vec3 DecodeNormal(int value) => new(((value << 22) >> 22) / 511f,
            ((value << 12) >> 22) / 511f, (unchecked(value << 2) >> 22) / 511f);

        public override string ToString() => Position.ToString();
    }
}
#pragma warning restore CS0618
