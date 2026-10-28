using static GBX.NET.BitHelper;

namespace GBX.NET.Engines.Plug;

public partial class CPlugShaderGeneric
{
    public CPlugShaderGeneric()
    {
        Material = new SMaterial();
    }

    public CPlugShaderGeneric(GameVersion gameVersion) : base(gameVersion)
    {
        Material = new SMaterial(gameVersion);
    }

    public partial class SMaterial
    {
        public void ReadWrite(GbxReaderWriter rw, int v = 0)
        {
            if (v >= 1 && rw.Writer?.Format == GbxFormat.Text)
            {
                using var data = new MemoryStream();
                using var writer = new GbxWriter(data);
                using var binary = new GbxReaderWriter(writer);
                ReadWrite(binary, v);
                rw.Writer.Write(data.ToArray());
                return;
            }

            // DoData stores the material as binary data even in text-format Gbx files.
            using var binaryReader = v >= 1 ? rw.Reader?.ForceBinary() : null;
            rw.Vec3(ref ambientRGB);
            rw.Vec3(ref diffuseRGB);
            rw.Single(ref diffuseAlpha);
            rw.Vec3(ref specularRGB);
            rw.Single(ref specularExp);
            rw.Vec3(ref emissiveRGB);
            rw.Vec3(ref emissiveModifier);
            rw.Vec3(ref wireRGB);
            rw.Single(ref wireAlpha);
            rw.DataUInt32(ref flags);
        }

        public bool UseAmbient
        {
            get => GetBit(Flags, 0);
            set => Flags = SetBit(Flags, 0, value);
        }

        public bool UseAmbientColorVertex
        {
            get => GetBit(Flags, 1);
            set => Flags = SetBit(Flags, 1, value);
        }

        public bool UseDiffuse
        {
            get => GetBit(Flags, 2);
            set => Flags = SetBit(Flags, 2, value);
        }

        public bool UseDiffuseColorVertex
        {
            get => GetBit(Flags, 3);
            set => Flags = SetBit(Flags, 3, value);
        }

        public bool UseSpecular
        {
            get => GetBit(Flags, 4);
            set => Flags = SetBit(Flags, 4, value);
        }

        public bool UseEmissive
        {
            get => GetBit(Flags, 5);
            set => Flags = SetBit(Flags, 5, value);
        }

        public bool UseEmissiveColorVertex
        {
            get => GetBit(Flags, 6);
            set => Flags = SetBit(Flags, 6, value);
        }

        public bool NormalizeNs
        {
            get => GetBit(Flags, 7);
            set => Flags = SetBit(Flags, 7, value);
        }

        public bool OnlyInfiniteLights
        {
            get => GetBit(Flags, 8);
            set => Flags = SetBit(Flags, 8, value);
        }

        public bool DblSidedLighting
        {
            get => GetBit(Flags, 9);
            set => Flags = SetBit(Flags, 9, value);
        }
    }
}
