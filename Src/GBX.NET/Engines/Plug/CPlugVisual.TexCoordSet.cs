namespace GBX.NET.Engines.Plug;

public partial class CPlugVisual
{
    public sealed class TexCoordSet : IVersionable
    {
        public int Version { get; set; }
        public TexCoord[] TexCoords { get; set; } = [];
        public int? Flags { get; set; }

        private int Kind => Version < 3 ? Version : Flags.GetValueOrDefault(256) & 0xFF;

        public void ReadWrite(GbxReaderWriter rw, int expectedCount)
        {
            rw.VersionInt32(this);
            if (Version >= 3)
            {
                var count = rw.Int32(TexCoords.Length);
                if (count != expectedCount) throw new InvalidDataException("TexCoord actualCount != expectedCount");
                Flags = rw.Int32(Flags.GetValueOrDefault(256));
            }
            if ((uint)Kind > 2) throw new InvalidDataException("TexCoord flags kind > 2");
            ReadWriteComponents(rw, expectedCount, Kind);
        }

        internal void ReadWriteComponents(GbxReaderWriter rw, int expectedCount, int kind)
        {
            if (rw.Reader is not null) TexCoords = new TexCoord[expectedCount];
            if (TexCoords.Length != expectedCount) throw new InvalidDataException("TexCoord actualCount != expectedCount");
            using var binaryReader = rw.Reader?.ForceBinary();
            for (var i = 0; i < expectedCount; i++)
            {
                TexCoords[i] = TexCoords[i].ReadWrite(rw, kind);
            }
        }

        public static TexCoordSet Read(GbxReader r, int expectedCount)
        {
            var value = new TexCoordSet();
            using var rw = new GbxReaderWriter(r);
            value.ReadWrite(rw, expectedCount);
            return value;
        }

        public void Write(GbxWriter w)
        {
            using var rw = new GbxReaderWriter(w);
            ReadWrite(rw, TexCoords.Length);
        }

        [Obsolete("Use TexCoords[i].Z and TexCoords[i].W instead.")]
        public float[]? U01
        {
            get
            {
                var values = new float[checked(TexCoords.Length * Kind)];
                for (var i = 0; i < TexCoords.Length; i++)
                {
                    if (Kind >= 1) values[i * Kind] = TexCoords[i].Z.GetValueOrDefault();
                    if (Kind >= 2) values[i * Kind + 1] = TexCoords[i].W.GetValueOrDefault();
                }
                return values;
            }
            set
            {
                for (var i = 0; i < TexCoords.Length; i++)
                {
                    var offset = i * Kind;
                    TexCoords[i] = TexCoords[i] with
                    {
                        Z = Kind >= 1 && value is not null && offset < value.Length ? value[offset] : null,
                        W = Kind >= 2 && value is not null && offset + 1 < value.Length ? value[offset + 1] : null
                    };
                }
            }
        }
    }

#pragma warning disable CS0618
    public readonly record struct TexCoord(Vec2 UV,
        [property: Obsolete("Use Z instead.")] int? U01,
        [property: Obsolete("Use W instead.")] int? U02)
    {
        public float? Z
        {
            get => U01.HasValue ? BitConverter.ToSingle(BitConverter.GetBytes(U01.Value), 0) : null;
            init => U01 = value.HasValue ? BitConverter.ToInt32(BitConverter.GetBytes(value.Value), 0) : null;
        }

        public float? W
        {
            get => U02.HasValue ? BitConverter.ToSingle(BitConverter.GetBytes(U02.Value), 0) : null;
            init => U02 = value.HasValue ? BitConverter.ToInt32(BitConverter.GetBytes(value.Value), 0) : null;
        }

        public static TexCoord Read(GbxReader r, int version)
        {
            using var rw = new GbxReaderWriter(r);
            return default(TexCoord).ReadWrite(rw, version < 3 ? version : 0);
        }

        public void Write(GbxWriter w, int version)
        {
            using var rw = new GbxReaderWriter(w);
            _ = ReadWrite(rw, version < 3 ? version : 0);
        }

        internal TexCoord ReadWrite(GbxReaderWriter rw, int kind)
        {
            var uv = rw.Vec2(UV);
            var z = kind >= 1 ? rw.Int32(U01.GetValueOrDefault()) : default(int?);
            var w = kind >= 2 ? rw.Int32(U02.GetValueOrDefault()) : default(int?);
            return new TexCoord(uv, z, w);
        }
    }
#pragma warning restore CS0618
}
