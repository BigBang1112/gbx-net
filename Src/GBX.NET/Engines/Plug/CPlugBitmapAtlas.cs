namespace GBX.NET.Engines.Plug;

public partial class CPlugBitmapAtlas
{
    public partial class SubTex
    {
        // These ids are often numbers, and both names and numbers must be written back as stored.
        private GBX.NET.Id? u01;
        public string? U01 { get => GBX.NET.Id.ToStoredString(u01); set => u01 = GBX.NET.Id.FromStoredString(value, u01); }

        private GBX.NET.Id? u02;
        public string? U02 { get => GBX.NET.Id.ToStoredString(u02); set => u02 = GBX.NET.Id.FromStoredString(value, u02); }
    }
}
